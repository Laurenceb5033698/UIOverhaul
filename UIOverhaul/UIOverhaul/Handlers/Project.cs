using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace UIOverhaul {
    namespace handler {
        public static class Project {
            public static void Init(){
                Assets.Init();

                UIHandler.Init();
            }
        }
    }
}