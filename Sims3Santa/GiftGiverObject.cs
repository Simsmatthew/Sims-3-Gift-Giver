using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ActorSystems;
using Sims3.SimIFace;
using Sims3.SimIFace.CustomContent;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.Services.Recursor94;

namespace Sims3.Gameplay.Objects.Recursor94
{
    class GiftGiverObject : GameObject, IGameObject, IScriptObject, IScriptLogic, IHasScriptProxy, IObjectUI, IExportableContent
    {

        public override void OnStartup()
        {
            base.OnStartup();
            //Sims3.UI.TwoButtonDialog.Show("Sir.  The AlarmManager is armed and we are spawning the service.", "Hiya", "booya", false);
            GiftGiver.SpawnGiftGiver();
        }
    }
}
