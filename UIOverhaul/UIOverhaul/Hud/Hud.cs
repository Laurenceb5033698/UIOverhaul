using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace UIOverhaul {
    namespace hud {
        public static class Hud {
            public static Transform OriginalHUDroot = null;
            public static types.Theme hudTheme;

            public static void CustomHUDAssembly(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self) {
                orig(self);
                OriginalHUDroot = self.transform;

                Clusters.Init(OriginalHUDroot, hudTheme);

                //We want to chop and change the HUD layout before we call the default function.
                //that way when we remove items and add our own, we shouldnt be missing out on default setup.
                throw new NotImplementedException();
            }
        }
    }
}