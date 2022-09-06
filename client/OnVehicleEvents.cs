using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using static RAGE.Events;
using static RAGE.Task;
using static RAGE.Ui.Cursor;

namespace car_dirty
{
    internal class OnVehicleEvents : Script
    {
        private const int SurfaceUpdateDelay = 1000;
        private const int ToServerUpdateDelay = 1000;

        private string DebugCanBeDirtKey = "Загрязняемая:";
        private string DebugDirtness = "Ур. загрязнения:";
        private string DebugDirtness2 = "Ур. загрязнения2:";
        private string DebugSurfaceKey = "Поверхность:";
        private string DebugSurfaceDirtnessKey = "Влияние:";
        private string DebugBaseModificatorKey = "Баз. модиф.:";

        private static Vector3 LastVehiclePosition;

        private readonly ulong SharedVehicleDirtnessValue = 14184508368535040139;

        public OnVehicleEvents()
        {
            EnableKeyDataChangeEvent = true;
            OnEntityDataChangeByKey += OnClientReceiveEntityDataChanges;

            OnPlayerEnterVehicle += OnClientEnterVehicle;
            OnPlayerLeaveVehicle += OnClientLeaveVehicle;

            Add("Client:Vehicles:StartWash", OnVehicleStartWash);
            Add("Client:Sync:Vehicles:SetDirtnessBaseModificator", ChangeVehicleBaseModificator);
        }

        #region Events From Server
        private void ChangeVehicleBaseModificator(object[] args)
        {
            var veh = (Entity)args[0];
            float newBaseDirtnessModificator = (float)args[1];

            veh.SetData("client:dirtness:modificator", newBaseDirtnessModificator);
            DebugText.Set(DebugBaseModificatorKey, newBaseDirtnessModificator.ToString());
        }

        private async void OnVehicleStartWash(object[] args)
        {
            RAGE.Chat.Output("Wash started");

            var initialPos = args[0] as Vector3;
            var initialRot = args[1] as Vector3;
            var finalPos = args[2] as Vector3;

            var localPlayer = RAGE.Elements.Player.LocalPlayer;
            var veh = localPlayer.Vehicle;

            veh.Position = initialPos;
            veh.SetRotation(initialRot.X, initialRot.Y, initialRot.Z, 2, true);

            while (Vector3.Distance(veh.Position, finalPos) > 3f)
            {
                await Task.WaitAsync(1);

                veh.Rpm = 0.3f;
                veh.SetForwardSpeed(2f);
            }

            CallRemote("Server:Washers:VehicleWashStopped");
        }
        #endregion

        #region Client Events
        private void OnClientReceiveEntityDataChanges(ulong key, Entity entity, object arg)
        {
            if (entity is Vehicle vehicle)
            {
                if (!RAGE.Game.Entity.DoesEntityExist(vehicle.Handle)) return;

                if(key == SharedVehicleDirtnessValue)
                {
                    RAGE.Chat.Output($"Received dirtness changes for vehicle[{vehicle.Handle}]: {arg}");

                    float newDirtnessLevel = (float)arg;
                    float newDirtnessValue = Math.Clamp(MathUtils.GetMappedRangeValue(new Vector2(0, 100), new Vector2(0, 15), newDirtnessLevel), 0, 15);

                    vehicle.SetDirtLevel(newDirtnessValue);
                }
            }
        }

        private void OnClientLeaveVehicle(Vehicle fromVeh, int seat)
        {
            DebugText.ClearAll();

            UpdateVehicleSurfaceStatus = false;
            SyncVehicleDirtnessToServerStatus = false;
        }

        private void OnClientEnterVehicle(Vehicle toVeh, int seat)
        {
            bool isDriver = seat == -1;
            bool isVehicleCanBeDirt = (bool)toVeh.GetSharedData("dirtness:available");

            if (!isVehicleCanBeDirt || !isDriver) return;

            float baseDirtModificator = (float)toVeh.GetSharedData("dirtness:modificator");
            float dirtnessLevel = (float)toVeh.GetSharedData("dirtness:value");

            DebugText.Add(DebugCanBeDirtKey, "true");
            DebugText.Add(DebugDirtness, $"{dirtnessLevel}%");
            DebugText.Add(DebugDirtness2, toVeh.GetDirtLevel().ToString());
            DebugText.Add(DebugSurfaceKey, "none");
            DebugText.Add(DebugSurfaceDirtnessKey, "0%");
            DebugText.Add(DebugBaseModificatorKey, baseDirtModificator.ToString());

            LastVehiclePosition = toVeh.Position;

            toVeh.SetData("client:dirtness:value", dirtnessLevel);
            toVeh.SetData("client:dirtness:modificator", baseDirtModificator);

            UpdateVehicleSurface();
            SyncVehicleDirtnessToServer();
        }
        #endregion

        #region Loop Updates
        private bool SyncVehicleDirtnessToServerStatus;
        private async void SyncVehicleDirtnessToServer()
        {
            SyncVehicleDirtnessToServerStatus = true;
            while (SyncVehicleDirtnessToServerStatus)
            {
                await WaitAsync(ToServerUpdateDelay);
                if (!SyncVehicleDirtnessToServerStatus) return;

                var veh = Player.LocalPlayer.Vehicle;

                CallRemote("Server:Sync:Vehicles:Dirtness:SetVehicleDirtnesLevel", veh.NetworkGetNetworkIdFrom(), veh.GetData<float>("client:dirtness:value"));
            }
        }


        private bool UpdateVehicleSurfaceStatus;
        private async void UpdateVehicleSurface()
        {
            UpdateVehicleSurfaceStatus = true;

            while (UpdateVehicleSurfaceStatus)
            {
                await WaitAsync(SurfaceUpdateDelay);

                if (!UpdateVehicleSurfaceStatus) return;

                var veh = Player.LocalPlayer.Vehicle;
                var vehPos = veh.Position;

                var rayCastResult = WorldUtils.Raycast(vehPos + new Vector3(0, 0, 1f), new Vector3(0, 0, -1f), 10f, IntersectOptions.Everything, veh);

                if(!rayCastResult.DitHit)
                {
                    DebugText.Set(DebugSurfaceKey, "none");
                    DebugText.Add(DebugSurfaceDirtnessKey, "0%");
                    continue;
                }

                DebugText.Set(DebugSurfaceKey, rayCastResult.Material.ToString().ToLower());

                float dirtnessImpact = WorldUtils.GetDirtnessImpactFromSurfaceMaterial(rayCastResult.Material);

                DebugText.Set(DebugSurfaceDirtnessKey, $"+{dirtnessImpact}%");

                float passedDistanced = Vector3.Distance(vehPos, LastVehiclePosition);
                LastVehiclePosition = vehPos;

                float dirtnessImpactModificator = veh.GetData<float>("client:dirtness:modificator");
                
                float additionalDirtnessLevel = MathUtils.GetMappedRangeValue(new Vector2(0, 1000), new Vector2(0, dirtnessImpact * dirtnessImpactModificator), passedDistanced);
                float currentDirtnessLevel = veh.GetData<float>("client:dirtness:value");
                float newDirtnessLevel = Math.Clamp(currentDirtnessLevel + additionalDirtnessLevel, 0, 100);

                veh.SetData("client:dirtness:value", newDirtnessLevel);
                DebugText.Set(DebugDirtness, $"{newDirtnessLevel}%");

                if(newDirtnessLevel == 100)
                {
                    UpdateVehicleSurfaceStatus = false;
                }
            }
        }
        #endregion
    }
}
