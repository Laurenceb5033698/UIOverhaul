using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace UIOverhaul {
    namespace handler {
        public static class UIHandler {
            public static void Init(){
                //setup which theme we will be using
                hud.Hud.hudTheme = hud.types.Theme.Design1;

                //create the hook
                On.RoR2.UI.HUD.Awake += hud.Hud.CustomHUDAssembly;
            }

            // private void OnDestroy(){
            //    On.RoR2.UI.HUD.Awake -= hud.Hud.CustomHUDAssembly;
            // }
        }
    }
}