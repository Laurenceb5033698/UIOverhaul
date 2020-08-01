using System.Reflection;
using UnityEngine;

namespace UIOverhaul {
    namespace util {
        public static class Children {
            //good for testing prefabs loaded by assetbundle
            static void PrintGameObjectChildren(GameObject objToPrint) {
                var allComponents = objToPrint.GetComponentsInChildren<Transform>();
                UIOverhaul.Logger.LogMessage("prefab object count: " + allComponents.Length);

                foreach (Transform obj in allComponents) {
                    //if (obj.name == "FullHealthText")
                    UIOverhaul.Logger.LogMessage("prefab objname: " + obj.name);
                }
            }
        }

        public static class Component {
            //Builds on AddComponent and allows an existing component to have it's values copied to a new component on a new GameObject
            //Unknown if this method is problematic. I.E. could be overwriting default values, and setting pointers to objects that will be deleted.
            public static RoR2.UI.HGTextMeshProUGUI Add(this GameObject game, RoR2.UI.HGTextMeshProUGUI duplicate){
                RoR2.UI.HGTextMeshProUGUI target = game.AddComponent<RoR2.UI.HGTextMeshProUGUI>();
                foreach(PropertyInfo x in typeof(RoR2.UI.HGTextMeshProUGUI).GetProperties()){
                    if(x.CanWrite){
                        x.SetValue(target, x.GetValue(duplicate));
                    }
                }
                return target;
            }

            public static RoR2.UI.HealthBar Add(this GameObject game, RoR2.UI.HealthBar duplicate){
                RoR2.UI.HealthBar target = game.AddComponent<RoR2.UI.HealthBar>();
                foreach(PropertyInfo x in typeof(RoR2.UI.HealthBar).GetProperties()){
                    if(x.CanWrite){
                        x.SetValue(target, x.GetValue(duplicate));
                    }
                }
                return target;
            }

            public static RoR2.UI.LevelText Add(this GameObject game, RoR2.UI.LevelText duplicate){
                RoR2.UI.LevelText target = game.AddComponent<RoR2.UI.LevelText>();
                foreach(PropertyInfo x in typeof(RoR2.UI.LevelText).GetProperties()){
                    if(x.CanWrite){
                        x.SetValue(target, x.GetValue(duplicate));
                    }
                }
                return target;
            }

            public static RoR2.UI.BuffDisplay Add(this GameObject game, RoR2.UI.BuffDisplay duplicate) {
                RoR2.UI.BuffDisplay target = game.AddComponent<RoR2.UI.BuffDisplay>();
                foreach(PropertyInfo x in typeof(RoR2.UI.BuffDisplay).GetProperties()) {
                    if(x.CanWrite) {
                        x.SetValue(target, x.GetValue(duplicate));
                    }
                }
                return target;
            }

            public static RoR2.UI.ExpBar Add(this GameObject game, RoR2.UI.ExpBar duplicate){
                RoR2.UI.ExpBar target = game.AddComponent<RoR2.UI.ExpBar>();
                foreach(PropertyInfo x in typeof(RoR2.UI.ExpBar).GetProperties()){
                    if(x.CanWrite){
                        x.SetValue(target, x.GetValue(duplicate));
                    }
                }
                return target;
            }
        }

        public static class Log {
            public static void ConsoleMessage(string message){
                if(message != null) {
                    UIOverhaul.Logger.LogMessage(message);
                }
            }
        }
    }
}