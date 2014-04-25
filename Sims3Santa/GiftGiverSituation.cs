using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Actors;
using Sims3.SimIFace;
using Sims3.Gameplay.Autonomy;

namespace Sims3.Gameplay.Services.Recursor94
{
    public class GiftGiverSituation : ServiceSituation
    {

        public GiftGiverSituation(Service service, Lot lot, Sim worker, int cost) : base (service, lot, worker, cost)
        {
            
            worker.AssignRole(this);
            this.Worker.Autonomy.Motives.MaxEverything();
            this.Worker.Autonomy.Motives.FreezeDecayEverythingExcept(new CommodityKind[0]);
            this.Worker.Autonomy.IncrementAutonomyDisabled(); //what the heck does that do? VOV
            
            


        }

        public override void EndService()
        {
            this.Worker.Service = null;
            this.mDestroyWorkerOnExit = false; //pretty standard.  But I'm not sure if I care whether he's destroyed since santa shouldn't be a regular sim
        }

        public GiftGiver giftGiver
        {
            get
            {
                return this.Service as GiftGiver;
            }
        }
    }




}
