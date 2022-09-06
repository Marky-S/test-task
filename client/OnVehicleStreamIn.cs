using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using static RAGE.Events;
using static RAGE.Ui.Cursor;

namespace car_dirty
{
    internal class OnVehicleStreamIn : Script
    {
        public OnVehicleStreamIn()
        {
            OnEntityStreamIn += OnClientStreamedEntity;
        }

        private void OnClientStreamedEntity(Entity streamedEntity)
        {
            bool isEntityVehicle = streamedEntity is Vehicle;
            if (!isEntityVehicle) return;

            bool isVehicleCanBeDirt = (bool)streamedEntity.GetSharedData("dirtness:available");
            if(!isVehicleCanBeDirt) return;

            Vehicle streamedVehicle = streamedEntity as Vehicle;

            float vehicleDirtnessLevel = (float)streamedEntity.GetSharedData("dirtness:value");
            float vehicleDirtnessValue = MathUtils.GetMappedRangeValue(new Vector2(0, 100), new Vector2(0, 15), vehicleDirtnessLevel);

            streamedVehicle.SetDirtLevel(vehicleDirtnessValue);
        }
    }
}
