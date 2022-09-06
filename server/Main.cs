using System;
using GTANetworkAPI;

namespace car_dirty
{
    public class Main : Script
    {
        [Command("veh", "Создать машину")]
        private static void CreateVehicle(Player sender)
        {
            var veh = NAPI.Vehicle.CreateVehicle(VehicleHash.T20, sender.Position, sender.Rotation, 70, 70);

            #region Init from DB
            veh.SetSharedData("dirtness:available", true);
            veh.SetSharedData("dirtness:value", 95f);
            veh.SetSharedData("dirtness:modificator", 1f);

            veh.SetData("dirtness:available", true);
            veh.SetData("dirtness:value", 95f);
            veh.SetData("dirtness:modificator", 1f);
            veh.SetData("dirtness:ceramic_installed", false);
            #endregion
        }
    }
}
