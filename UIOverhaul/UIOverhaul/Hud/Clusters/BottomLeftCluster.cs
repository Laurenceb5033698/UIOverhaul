using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;


namespace UIOverhaul {
    namespace hud {
        namespace clusters {
            public static class BLC {
                private static Transform oblc = null;
                private static Transform oblcparent = null;

                private static Transform Old_HealthbarRoot = null;
                private static Transform Old_LevelDisplayRoot = null;
                private static Transform Old_BuffDisplayRoot = null;
                private static Transform Old_ExpBarRoot = null;

                private static GameObject item = null;

                public static void Init(Transform OriginalHUDroot) {
                    oblc = OriginalHUDroot.Find("MainContainer/MainUIArea/BottomLeftCluster");  //old BottomLeftCluster transform
                    oblcparent = oblc.parent; //get existing parent
                    UIOverhaul.Logger.LogMessage("Original Root found.");

                    //get references to some of the object parents
                    //This is mostly to reduce path length when searching for Gameobjects (reduce risk of spelling errors)
                    Old_HealthbarRoot = oblcparent.Find("BarRoots/HealthbarRoot");
                    Old_LevelDisplayRoot = oblcparent.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot");
                    Old_BuffDisplayRoot = oblcparent.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot");
                    Old_ExpBarRoot = oblcparent.Find("BarRoots/LevelDisplayCluster/ExpBarRoot");
                    UIOverhaul.Logger.LogMessage("All Transform Parents found within Custom Cluster.");
                }

                public static void processComponent(Transform hudComponent, string gameObjectPath, string logMessage = null) {
                    util.Log.ConsoleMessage(logMessage);
                    item = hudComponent.gameObject;
                    util.Component.Add(item, oblc.Find(gameObjectPath).GetComponent<HGTextMeshProUGUI>());
                }

                public static void processComponent(Transform hudComponent, string gameObjectName, string gameObjectPath, string logMessage = null){
                    util.Log.ConsoleMessage(logMessage);
                    item = hudComponent.Find(gameObjectName).gameObject;
                    util.Component.Add(item, oblc.Find(gameObjectPath).GetComponent<HGTextMeshProUGUI>());
                }

                public static void processComponent(Transform customHud, Transform hudComponent, string gameObjectName, string gameObjectPath, string logMessage = null) {
                    util.Log.ConsoleMessage(logMessage);
                    item = hudComponent.Find(gameObjectName).gameObject;
                    util.Component.Add(item, customHud.Find(gameObjectPath).GetComponent<HGTextMeshProUGUI>());
                }

                public static void setItem(Transform customHud, string logMessage = null){
                    util.Log.ConsoleMessage(logMessage);
                    item = customHud.Find("ChatBoxRoot").gameObject;
                }

                public static void setItem(GameObject _item, string logMessage = null){
                    util.Log.ConsoleMessage(logMessage);
                    item = _item;
                }

                public static Transform OriginalFind(string path){ return oblc.Find(path); }
                public static Transform HealthbarRootFind(string path){ return Old_HealthbarRoot.Find(path); }
                public static Transform LevelDisplayRootFind(string path){ return Old_LevelDisplayRoot.Find(path); }
                public static Transform BuffDisplayRootFind(string path){ return Old_BuffDisplayRoot.Find(path); }
                public static Transform ExpBarRootFind(string path) { return Old_ExpBarRoot.Find(path); }

                public static Transform getOriginal(){ return oblc; }
                public static Transform getOriginalParent(){ return oblcparent; }

                public static Transform getHealthbarRoot(){ return Old_HealthbarRoot; }
                public static Transform getLevelDisplayRoot(){ return Old_LevelDisplayRoot; }
                public static Transform getBuffDisplayRoot(){ return Old_BuffDisplayRoot; }
                public static Transform getExpBarRoot(){ return Old_ExpBarRoot; }
                
                public static GameObject getItem(){ return item; }
            }
        }
    }
}