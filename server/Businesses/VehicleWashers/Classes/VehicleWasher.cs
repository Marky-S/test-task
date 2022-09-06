using System;
using System.Collections.Generic;
using System.Text;
using car_dirty.Businesses.VehicleWashers.Enums;
using GTANetworkAPI;
using RAGE;

namespace car_dirty.Businesses.Washers.Class
{
    public class VehicleWasher
    {
        public float TriggerRadius { get; set; } = 10f;

        public bool IsBusy { get; set; }

        public string UniqueKey { get; set; }

        public WasherVehicleProcessingType CurrentProcessingType { get; set; }

        public Vector3 TriggerPosition { get; set; } = new Vector3();
        public Vector3 WashInitialPosition { get; set; } = new Vector3();
        public Vector3 WashInitialRotation { get; set; } = new Vector3();
        public Vector3 WashFinalPosition { get; set; } = new Vector3();
        public ColShape TriggerColShape { get; set; }
        public Vehicle CurrentProcessingVehicle { get; set; }

        public void Initialize()
        {
            this.UniqueKey = Utils.RandomString(6);

            this.TriggerColShape = NAPI.ColShape.CreateSphereColShape(this.TriggerPosition, this.TriggerRadius);
              
            this.TriggerColShape.OnEntityEnterColShape += OnEntityEnterColShape;
            this.TriggerColShape.OnEntityExitColShape += OnEntityExitColShape;
        }

        private void OnEntityEnterColShape(ColShape colShape, Player client)
        {
            client.SetData("server:washers:triggered_washer", this.UniqueKey);
        }

        private void OnEntityExitColShape(ColShape colShape, Player client)
        {
            client.ResetData("server:washers:triggered_washer");
        }

        public void StartClientVehicleProcessing(Player player, WasherVehicleProcessingType processingType)
        {
            Console.WriteLine($"Processing started {processingType}");

            this.CurrentProcessingVehicle = player.Vehicle;
            this.CurrentProcessingType = processingType;
            this.IsBusy = true;

            player.SetData("server:washers:current_washer", this.UniqueKey);
            player.TriggerEvent("Client:Vehicles:StartWash", WashInitialPosition, WashInitialRotation, WashFinalPosition);
        }

        public void StopClientVehicleProcessing(Player player)
        {
            Console.WriteLine($"Processing ended {this.CurrentProcessingType}");

            if (this.CurrentProcessingType == WasherVehicleProcessingType.Detailing)
            {
                this.CurrentProcessingVehicle.SetData("dirtness:ceramic_installed", true);

                float newModificator = this.CurrentProcessingVehicle.GetData<float>("dirtness:modificator");

                newModificator /= 2;

                this.CurrentProcessingVehicle.SetData("dirtness:modificator", newModificator);
                this.CurrentProcessingVehicle.SetSharedData("dirtness:modificator", newModificator);

                this.CurrentProcessingVehicle.Controller?.TriggerEvent("Client:Sync:Vehicles:SetDirtnessBaseModificator", this.CurrentProcessingVehicle, newModificator);
            }

            this.CurrentProcessingVehicle.SetSharedData("dirtness:value", 0);
            this.CurrentProcessingVehicle.SetData("dirtness:value", 0);

            this.CurrentProcessingVehicle = null;
            this.CurrentProcessingType = WasherVehicleProcessingType.None;
            this.IsBusy = false;
        }

        public bool CanAcceptPlayer(Player player)
        {
            if (this.IsBusy)
            {
                return false;
            }

            var rotationTemp = player.Vehicle.Rotation - this.WashInitialRotation;
            float rotationDifference = rotationTemp.Length();

            if (rotationDifference > 1)
            {
                Console.WriteLine($"Too big rotation difference {rotationDifference}");
                return false;
            }

            Console.WriteLine($"Rotation difference {rotationDifference}");

            return true;
        }
    }
}
