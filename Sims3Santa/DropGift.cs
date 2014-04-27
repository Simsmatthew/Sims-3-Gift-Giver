using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Objects.Recursor94;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Interfaces;
using Sims3.SimIFace;

namespace Sims3.Gameplay.Services.Recursor94

    //To be called in GiftGiverSituation
{
    public sealed class DropGift : Interaction<Sim, GameObject> //can't make it specific to my particular object for some reason.
    {
        public static readonly InteractionDefinition Singleton = new Definition();
        private IGameObject giftBox;  //may change this to collection later, to add multiple gifts

        private sealed class Definition : InteractionDefinition<Sim, GameObject, DropGift> 
        {
            protected override bool Test(Sim actor, GameObject target, bool isAutonomous, ref SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                if (actor.WasCreatedByAService)
                {
                    return true; //Don't want anybody else completing this action
                }
                return false;
            }

            
        }

        protected override bool Run()
        {
            base.Actor.RouteToObjectRadiusAndCheckInUse(base.Target, 0.03f);
            giftBox = GlobalFunctions.CreateObjectOutOfWorld("giftPartyBox"); //not sure if this will work.  Use Mailbox.putmail bottom of Run() method for reference.
            //CarrySystem.PickUp(base.Actor, this.giftBox);  Can't use carrymanager not ICarryable
            //use giftpile.add as reference

            base.EnterStateMachine("giftpile", "EnterDrop", "x");
            this.mCurrentStateMachine.SetPropActor("gift", giftBox.ObjectId);
            base.AnimateSim("ExitDrop");
            base.StandardExit();

            Vector3 objPosition = base.Target.Position;
            Vector3 giftPosition = new Vector3(objPosition.x + 0.06f, objPosition.y, objPosition.z);

            return true;
        }
    }
}
