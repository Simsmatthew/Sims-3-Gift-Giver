using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Actors;
using Sims3.SimIFace;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Objects.Recursor94;
using Sims3.UI;
using Sims3.Gameplay.Utilities;

namespace Sims3.Gameplay.Services.Recursor94
{
    public class GiftGiverSituation : ServiceSituation
    {
        
        public GiftGiverSituation(Service service, Lot lot, Sim worker, int cost) : base (service, lot, worker, cost)
        {
            
            worker.AssignRole(this);
            this.Worker.Autonomy.Motives.MaxEverything();
            this.Worker.Autonomy.Motives.FreezeDecayEverythingExcept(new CommodityKind[0]);
            this.Worker.Autonomy.AllowedToRunMetaAutonomy = false;
            this.Worker.Autonomy.IncrementAutonomyDisabled(); //what the heck does that do? VOV
            base.SetState(new GiftGiverSituation.RouteToLot(this));  //Should be using inherited walk to lot so that he doesn't driver there instead.
            base.ScheduleSwitchWorkerToServiceOutfit();

            
            


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

        public class LeaveGifts : ChildSituation<GiftGiverSituation>
        { //** situation that finds santa object, routes to it and leaves presents.
         

            public LeaveGifts(GiftGiverSituation parent) : base(parent)
            {
            }
            public override void Init(GiftGiverSituation parent)
            {
                
                

                    this.findGiftGiverObject();
                    this.Parent.Worker.RouteToObjectRadius(findGiftGiverObject(), 0f);
                
             
            }

            public GameObject findGiftGiverObject()
            {

                GiftGiverObject [] lotCallingObjects = this.Lot.GetObjects<GiftGiverObject>(); //get all of the santa calling objects on the lot, for now we're treating it as if there were only one--and that's the one santa cares about.
                debugMessage(lotCallingObjects[0].ToString());
                return lotCallingObjects[0];
               
               // PlumbBob.SelectedActor.ShowTNSIfSelectable(lotObjects[0] + "" , Sims3.UI.StyledNotification.NotificationStyle.kSimTalking);
                

            }

        }



        public class RouteToLot : ChildSituation<GiftGiverSituation>
        {
            public RouteToLot(GiftGiverSituation parent) : base(parent) //not even really sure if this is necessary.  probably calls the super class constructor which does something.
            {
                
            }

            public override void Init(GiftGiverSituation parent) {

                // Sims3.UI.TwoButtonDialog.Show("Sir!  The routing has begun.", "Hiya", "booya", false);
                parent.OnServiceStarting();
                parent.MakeServiceSimVisible(); //the former two lines actually spawn the service npc.  I'm not sure if this should be done here or in the constructor.  Leaving here for now.
                // Audio.StartSound("sting_burglar_arrive");
                base.RequestWalkStyle(parent.Worker, Sim.WalkStyle.Sneak);
                 this.Parent.SetState(new ServiceSituation.WalkToLot<GiftGiverSituation, GiftGiverSituation.LeaveGifts>(this.Parent));
            }
           

        }

        public static void debugMessage(string vsText)
        {
            StyledNotification.Show(new StyledNotification.Format(SimClock.CurrentTime().ToString() + " " + vsText, ObjectGuid.InvalidObjectGuid, ObjectGuid.InvalidObjectGuid, StyledNotification.NotificationStyle.kGameMessagePositive));
        }
    }




}
