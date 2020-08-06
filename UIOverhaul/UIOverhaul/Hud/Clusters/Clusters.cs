using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace UIOverhaul {
    namespace hud {
        //A cluster here is a block of UI elements that usually share a common parent in object hierarchy
        public static class Clusters {
            public static void Init(Transform OriginalHUDroot, types.Theme hudTheme) {
                InitClusters(OriginalHUDroot);
                SelectThemeCluster(hudTheme);
            }

            //process original Hud clusters
            public static void InitClusters(Transform OriginalHUDroot) {
                clusters.BLC.Init(OriginalHUDroot);
            }

            //Select which custom Hud we are using
            public static void SelectThemeCluster(types.Theme hudTheme){
                switch(hudTheme){
                    case types.Theme.Design1:
                        clusters.Design1.SetupCluster();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}