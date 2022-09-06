using System;
using System.Collections.Generic;
using System.Text;
using car_dirty.Businesses.VehicleWashers.Enums;
using car_dirty.Businesses.Washers.Class;
using GTANetworkAPI;

namespace car_dirty.Businesses.VehicleWashers.Data
{
    public class VehicleWashers : Script
    {
        public static Dictionary<string, VehicleWasher> ServerVehicleWashers = new Dictionary<string, VehicleWasher>();

        public VehicleWashers()
        {
            var initialWasher = new VehicleWasher
            {
                TriggerPosition = new Vector3(-424.4f, 1127.3f, 325.4f),

                WashInitialPosition = new Vector3(-424.4f, 1127.3f, 325.4f),
                WashInitialRotation = new Vector3(-0.1f, 0.34f, -16.2f),

                WashFinalPosition = new Vector3(-419.8f, 1143.9f, 325.4f),
            };

            initialWasher.Initialize();

            ServerVehicleWashers.Add(initialWasher.UniqueKey, initialWasher);
        }

        private bool CanWashersAcceptPlayer(Player source)
        {
            if (!source.IsInVehicle)
            {
                Console.WriteLine("Not in veh");
                return false;
            }
            if (!source.Vehicle.GetData<bool>("dirtness:available"))
            {
                Console.WriteLine("Not in dirtness available veh");
                return false;
            }
            if (!source.HasData("server:washers:triggered_washer"))
            {
                Console.WriteLine("Not in washer");
                return false;
            }

            return true;
        }
        
        [RemoteEvent("Server:Washers:VehicleWashStopped")]
        private void OnClientEndedVehicleWash(Player source)
        {
            string washerUniqueKey = source.GetData<string>("server:washers:current_washer");

            var washer = ServerVehicleWashers[washerUniqueKey];

            washer.StopClientVehicleProcessing(source);
        }


        [Command("startwash")]
        private void OnClientAttemptsStartWash(Player source)
        {
            if (!this.CanWashersAcceptPlayer(source)) return;

            string washerUniqueKey = source.GetData<string>("server:washers:triggered_washer");
            var washer = ServerVehicleWashers[washerUniqueKey];

            if (!washer.CanAcceptPlayer(source))
            {
                return;
            }

            // ...

            washer.StartClientVehicleProcessing(source, WasherVehicleProcessingType.Wash);
            
        }

        [Command("startdetailing")]
        private void OnClientAttemptsStartDetailing(Player source)
        {
            if (!this.CanWashersAcceptPlayer(source)) return;

            string washerUniqueKey = source.GetData<string>("server:washers:triggered_washer");
            var washer = ServerVehicleWashers[washerUniqueKey];

            if (!washer.CanAcceptPlayer(source))
            {
                return;
            }

            // ...

            washer.StartClientVehicleProcessing(source, WasherVehicleProcessingType.Detailing);
        }
    }
}
