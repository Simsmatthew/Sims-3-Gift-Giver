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
            base.EnterStateMachine("giftpile", "EnterDrop", "x");  
            this.mCurrentStateMachine.SetPropActor("gift", ObjectGuid.InvalidObjectGuid); //works perfedtly. The solution was to set the actor to an invalid GUID.  Because sure.  why not.
            base.AnimateSim("ExitDrop");
            base.StandardExit();
        
            spawnGifts(base.Actor.LotCurrent.GetSimsCount());

            return true;
        }

        public void spawnGifts(int numberOfSims)  //maybe in the future make it based on age, tunable variable for whether all age groups are okay or not.
        {
            IGameObject gift;
            
            float xOffset = 1f;
            for (int n = 0; n < numberOfSims; n++)
            {
                gift = GlobalFunctions.CreateObjectOutOfWorld("giftgiverbox"); //make random later.
                Vector3 objPosition = base.Target.Position;
                Vector3 giftPosition = new Vector3(objPosition.x + xOffset, objPosition.y, objPosition.z);
                gift.AddToWorld();
                gift.SetPosition(giftPosition);
                GiftGiverSituation.debugMessage("Gift Position" + gift.Position + "santa object position" + this.Target.Position);
                xOffset += 0.7f;
            }
        }
    }
}
