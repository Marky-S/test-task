using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using static RAGE.Events;
using static RAGE.Game.Graphics;
using static RAGE.Game.Ui;

namespace car_dirty
{
    public class DebugText : Script
    {
        private static readonly string[,] TextToDraw = new string[50, 2];
        private static readonly Dictionary<string, int> KeyToArrayIndex = new Dictionary<string, int>();
        private static int AvailableIndex = 0;

        private static bool RenderEnabled;

        private static void DrawDebugText(List<TickNametagData> _)
        {
            for (int i = 0; i < TextToDraw.GetLength(0); i++)
            {
                if (TextToDraw[i, 0] == null || TextToDraw[i, 1] == null) continue;

                string textToDraw = TextToDraw[i, 0];

                SetTextScale(0.35f, 0.35f);
                SetTextFont(4);
                SetTextOutline();
                SetTextProportional(true);
                SetTextColour(255, 255, 255, 255);
                BeginTextCommandDisplayText("STRING");
                SetTextCentre(false);
                AddTextComponentSubstringPlayerName(textToDraw);
                EndTextCommandDisplayText(0.1f, i * 0.02f + 0.1f, 0);

                textToDraw = TextToDraw[i, 1];

                SetTextScale(0.35f, 0.35f);
                SetTextFont(4);
                SetTextOutline();
                SetTextProportional(true);
                SetTextColour(255, 255, 255, 255);
                BeginTextCommandDisplayText("STRING");
                SetTextCentre(false);
                AddTextComponentSubstringPlayerName(textToDraw);
                EndTextCommandDisplayText(0.2f, i * 0.02f + 0.1f, 0);
            }
        }

        public static void Add(string key, string value)
        {
            if (KeyToArrayIndex.ContainsKey(key)) return;
            if (AvailableIndex == 49) return;

            KeyToArrayIndex.Add(key, AvailableIndex);
            TextToDraw[AvailableIndex, 0] = key;
            TextToDraw[AvailableIndex, 1] = value;

            AvailableIndex++;

            if (!RenderEnabled)
            {
                RenderEnabled = true;
                Tick += DrawDebugText;
            }
        }

        public static void Remove(string key)
        {
            if (!KeyToArrayIndex.ContainsKey(key)) return;

            int indexToRemove = KeyToArrayIndex[key];

            TextToDraw[indexToRemove, 0] =
            TextToDraw[indexToRemove, 1] = null;

            AvailableIndex--;

            if (RenderEnabled && AvailableIndex == 0)
            {
                RenderEnabled = false;
                Tick -= DrawDebugText;
            }
        }

        public static void Set(string key, string newValue)
        {
            if (!KeyToArrayIndex.ContainsKey(key)) return;

            int indexToChange = KeyToArrayIndex[key];

            TextToDraw[indexToChange, 1] = newValue;
        }

        public static void ClearAll()
        {
            for (int i = 0; i < TextToDraw.GetLength(0); i++)
            {
                TextToDraw[i, 0] =
                TextToDraw[i, 1] = null;
            }

            AvailableIndex = 0;
            KeyToArrayIndex.Clear();
            RenderEnabled = false;
            Tick -= DrawDebugText;
        }
    }
}
