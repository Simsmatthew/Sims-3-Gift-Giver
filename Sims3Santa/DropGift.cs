using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Objects.Recursor94;

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
            return true;
        }
    }
}
