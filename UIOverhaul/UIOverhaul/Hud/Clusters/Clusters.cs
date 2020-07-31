using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace UIOverhaul {
    namespace hud {
        public static class Clusters {
            public static void Init(Transform OriginalHUDroot, types.Theme hudTheme) {
                InitClusters(OriginalHUDroot);
                SelectThemeCluster(hudTheme);
            }

            public static void InitClusters(Transform OriginalHUDroot) {
                clusters.BLC.Init(OriginalHUDroot);
            }

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