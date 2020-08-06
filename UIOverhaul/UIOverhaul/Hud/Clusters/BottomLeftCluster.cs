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
            //BottomLeftCluster(BLC) dissects objects from Exisiting HUD and allows easy access to scripts for copying.
            public static class BLC {
                private static Transform oblc = null;
                private static Transform oblcparent = null;

                private static Transform Old_HealthbarRoot = null;
                private static Transform Old_LevelDisplayRoot = null;
                private static Transform Old_BuffDisplayRoot = null;
                private static Transform Old_ExpBarRoot = null;


                public static void Init(Transform OriginalHUDroot) {
                    oblc = OriginalHUDroot.Find("MainContainer/MainUIArea/BottomLeftCluster");  //old BottomLeftCluster transform
                    oblcparent = oblc.parent; //get existing parent
                    UIOverhaul.Logger.LogMessage("Original Root found.");

                    //get references to some of the object parents
                    //This is mostly to reduce path length when searching for Gameobjects (reduce risk of spelling errors)
                    Old_HealthbarRoot = oblc.Find("BarRoots/HealthbarRoot");
                    Old_LevelDisplayRoot = oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot");
                    Old_BuffDisplayRoot = oblc.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot");
                    Old_ExpBarRoot = oblc.Find("BarRoots/LevelDisplayCluster/ExpBarRoot");
                    UIOverhaul.Logger.LogMessage("All Transform Parents found within Old Cluster.");
                }

                //takes a new object then finds a specific path object within BLC, then adds the HGMTP component to new obj and copies values over from old.
                public static HGTextMeshProUGUI CopyHGTMPvalues(Transform hudComponent, string gameObjectPath, string logMessage = null) {
                    util.Log.ConsoleMessage(logMessage);
                    GameObject item = hudComponent.gameObject;
                    return util.Component.Add(item, oblc.Find(gameObjectPath).GetComponent<HGTextMeshProUGUI>());
                }

                //takes a new object and finds child, then finds specific path to object within BLC. adds new HGTMP & copies values.
                public static HGTextMeshProUGUI CopyHGTMPvalues(Transform hudComponent, string gameObjectName, string gameObjectPath, string logMessage = null){
                    util.Log.ConsoleMessage(logMessage);
                    GameObject item = hudComponent.Find(gameObjectName).gameObject;
                    return util.Component.Add(item, oblc.Find(gameObjectPath).GetComponent<HGTextMeshProUGUI>());
                }

                //takes new item and finds child, then takes old item and finds child, then adds new HGTMP & copies values.
                public static HGTextMeshProUGUI CopyHGTMPvalues(Transform customHudItem, Transform oldHudItem, string customObjectPath, string oldObjectPath, string logMessage = null) {
                    util.Log.ConsoleMessage(logMessage);
                    GameObject item = customHudItem.Find(customObjectPath).gameObject;
                    return util.Component.Add(item, oldHudItem.Find(oldObjectPath).GetComponent<HGTextMeshProUGUI>());

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
                
            }
        }
    }
}