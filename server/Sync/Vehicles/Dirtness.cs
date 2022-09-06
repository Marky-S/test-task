using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace car_dirty.Sync.Vehicles
{
    public class Dirtness : Script
    {
        [RemoteEvent("Server:Sync:Vehicles:Dirtness:SetVehicleDirtnesLevel")]
        private void SyncVehicleNewDirtnessLevel(Player source, int vehNetID, float newDirtnessLevel)
        {
            Vehicle veh = new Vehicle(new NetHandle((ushort)vehNetID, EntityType.Vehicle));

            veh.SetSharedData("dirtness:value", newDirtnessLevel);
        
            if(newDirtnessLevel == 100f && veh.GetData<bool>("dirtness:ceramic_installed"))
            {
                float newModificator = veh.GetData<float>("dirtness:modificator");

                newModificator *= 2;

                veh.SetData("dirtness:ceramic_installed", false);
                veh.SetData("dirtness:modificator", newModificator);

                veh.SetSharedData("dirtness:modificator", newModificator);
            }
        }
    }
}
