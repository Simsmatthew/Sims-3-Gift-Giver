using System;
using System.Collections.Generic;
using System.Text;
using Sims3.SimIFace;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace.CAS;
namespace Sims3.Gameplay.Services.Recursor94
{
    public class GiftGiver : Service {
        [Tunable]  
        protected static bool kInstantiator;//necessary to load script
        [Tunable]
        private static Service.ServiceTuning kServiceTuning = new Service.ServiceTuning();
        private static int kEarliestTimeSimCanArrive;
        public static GiftGiver sGiftGiver; //reference to self

        static GiftGiver() {
            GiftGiver.kServiceTuning = new Service.ServiceTuning(4, 0, false, false, true);  //Look at constructor for meaning of values
            GiftGiver.sGiftGiver = null;
            GiftGiver.kEarliestTimeSimCanArrive = 23;
            GiftGiver.kMaxDistanceToRouteFromServiceCarToLotWithoutTeleporting = 0f;
           // World.OnWorldLoadFinishedEventHandler += new EventHandler(GiftGiver.OnWorldLoadFinished);

           
           


        }



        public override void MakeServiceRequest(Lot lot, bool active, ObjectGuid simRequestingService)
        {
            base.MakeServiceRequest(lot, active, simRequestingService, true, 1);  //should be immediate I guess
        }
        
        
        public GiftGiver()
        {
            GiftGiver.sGiftGiver = this;
            
        }
        
        
        public static GiftGiver Instance
        {
            get
            {


                return GiftGiver.sGiftGiver;
            }
        }

        protected override bool NeedsAssignment(Core.Lot lot) //not sure?
        {
            return base.IsServiceRequested(lot) && !base.IsAnySimAssignedToLot(lot);
        }

        public override string GetServiceTopic(Sim serviceSim) //probably refers to the conversation topic related to the acting sim
        {
            return "Easter Bunny";
        }


        protected override void SetTraits(SimDescription simDescription)  //probably refers to the sim traits that service members spawned will posses.
        {
            List<TraitNames> traits = new List<TraitNames>(new TraitNames[]
            {
                TraitNames.AboveReproach,
                TraitNames.Attractive,
                TraitNames.BornSalesman,  //I don't know, what else should He be?  Maybe I can create a custom trait in the future
                TraitNames.Brave
            });

            simDescription.TraitManager.AddHiddenElement(TraitNames.Burglar);
            
        }

        protected override void SetServiceNPCProperties(SimDescription simDescription)
        {
            simDescription.MotivesDontDecay = true;
            simDescription.CanBeKilledOnJob = false;  //assuming refering to service duties and not career.
            simDescription.Marryable = false; //sorry kids, can't marry santa or the easter bunny
            simDescription.Contactable = false;

        }

        protected override ServiceSituation InternalCreateSituation(Lot assignedLot, Sim createdSim, int cost, ObjectGuid requestingSim)
        {
            return new GiftGiverSituation(this, assignedLot, createdSim, cost);
            
        }

        protected override Service.ServiceTuning Tuning
        {
            get {
                return GiftGiver.kServiceTuning;
            }
        }
        public override ServiceType ServiceType
        {
            get {

                return ServiceType.Burglar; //Apparently determines car, and clothing.  and maybe other things.  setting to repair man yields repair truck.
            }
        }

        public static void SpawnGiftGiver()
        {



            //Sims3.UI.TwoButtonDialog.Show("Sir.  The AlarmManager is armed and we are spawning the service.", "Hiya", "booya", false);
            if (GiftGiver.Instance == null)


            {

                new GiftGiver(); //create a new instance of this service
            }


            LotManager.ActiveLot.AlarmManager.AddAlarm(1f, TimeUnit.Hours, GiftGiver.Instance.requestGiver, "Gift Giver Alarm", AlarmType.AlwaysPersisted, GiftGiver.sGiftGiver);
           
           
        }
        /*
        public void checkForSpawn(Lot lot, Sim sim)
        { //make this really simple at first--check burglar method for reference
            if (lot.AreAllSimsAsleepOrGone(sim))
            {
                lot.AlarmManager.AddAlarm(kEarliestTimeSimCanArrive, TimeUnit.Hours, new AlarmTimerCallback(SpawnGiftGiver), "Gift Giver Spawner", AlarmType.AlwaysPersisted, this);
            }
        }
        */

        private static void OnWorldLoadFinished(object sender, EventArgs args)  //add this delegate so that the event handler is loaded after the game loads
        {

            AlarmManager.Global.AddAlarm(7, TimeUnit.Hours, GiftGiver.SpawnGiftGiver, "Gift Giver Alarm", AlarmType.AlwaysPersisted, GiftGiver.sGiftGiver);
       
            
        }

        public void requestGiver()
        {
            GiftGiver.Instance.MakeServiceRequest(LotManager.ActiveLot, true, PlumbBob.SelectedActor.ObjectId); //must be here for alarm manager since directly putting this in alarm manager call is broken.

            Sims3.UI.TwoButtonDialog.Show("Sir.  The service has been requested.", "Hiya", "booya", false);
        }

        protected override CASAgeGenderFlags GetGenderForNewNpc(Lot lot)
        {
            return CASAgeGenderFlags.Male;
        }

        public override void UpdateCreatedSim(Sim sim)
        {
            sim.SimDescription.Age = CASAgeGenderFlags.Elder;
            sim.SimDescription.Fitness = 0.3f;
            sim.SimDescription.Weight = 1.4f;
            
        }
        
        
        }
}
