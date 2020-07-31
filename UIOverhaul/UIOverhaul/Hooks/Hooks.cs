using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;


namespace UIOverhaul {
    public static class Hooks {
        private static Transform OriginalHUDroot = null;

        //~Hooks()
        //{
        //    On.RoR2.UI.HUD.Awake -= CustomHUDAssembly;
        //
        //}

        public static void Init() {
            On.RoR2.UI.HUD.Awake += CustomHUDAssembly;

        }

        //This approach is aimed at altering the OldHud's hierarchy and inserting our own.
        //The General Idea is: a precision cut, instantiate custom prefabs, copy values, delete old, re-parent new (to exisiting Hud root).
        //I want to make use of as much existing code, so we don't have to re-write the whole UI.
        private static void CustomHUDAssembly(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self) {
            orig(self);
            OriginalHUDroot = self.transform;

            SetupBottomLeftCluster();   //set up HpBar, ExpBar, BuffBar, ChatBox


            //We want to chop and change the HUD layout before we call the default function.
            //that way when we remove items and add our own, we shouldnt be missing out on default setup.
            throw new NotImplementedException();
        }

        private static void SetupBottomLeftCluster() {
            Transform oblc = OriginalHUDroot.Find("MainContainer/MainUIArea/BottomLeftCluster");  //old BottomLeftCluster transform
            Transform oblcparent = oblc.parent; //get existing parent
            oblc.parent = null; //detatch this one object.
            UIOverhaul.Logger.LogMessage("Original Root found and parent disassociated.");

            //instantiate new UI cluster and set parent.
            Transform _BLC = UnityEngine.Object.Instantiate<GameObject>(Assets.UIBottomLeftCluster, oblcparent).transform;
            UIOverhaul.Logger.LogMessage("Custom HUD instantiated.");

            //get references to some of the object parents
            //This is mostly to reduce path length when searching for Gameobjects (reduce risk of spelling errors)
            Transform _HealthbarRoot = _BLC.Find("BarRoots/HealthbarRoot");
            Transform _LevelDisplayRoot = _BLC.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot");
            Transform _BuffDisplayRoot = _BLC.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot");
            Transform _ExpBarRoot = _BLC.Find("BarRoots/LevelDisplayCluster/ExpBarRoot");
            UIOverhaul.Logger.LogMessage("All Transform Parents found within Custom Cluster.");

            //now go through each Hud item and make sure game scripts are setup accordingly.
            //normally this is done via editor, but we don't have access to RoR2 scripts in editor ¯\_(ツ)_/¯
            GameObject item = null;
            {   //chatBox
                UIOverhaul.Logger.LogMessage("Processing ChatBoxRoot");

                item = _BLC.Find("ChatBoxRoot").gameObject;
                InstantiatePrefabOnStart temp = item.AddComponent<InstantiatePrefabOnStart>();
                temp.prefab = Resources.Load<GameObject>("Prefabs/ChatBox, In Run");//this should be okay
                temp.targetTransform = item.transform;
                temp.copyTargetRotation = true;
                temp.parentToTarget = true;

            }
            {   //HealthBar root
                {   //Slash
                    UIOverhaul.Logger.LogMessage("Processing Slash");
                    item = _HealthbarRoot.Find("Slash").gameObject;
                    util.Component.Add(item, _BLC.Find("BarRoots/HealthbarRoot/Slash").GetComponent<HGTextMeshProUGUI>());
                    //add any changes to text here e.g. font, size etc
                }
                {   //CurrentHealthText
                    UIOverhaul.Logger.LogMessage("Processing CurrentHealthText");
                    item = _HealthbarRoot.Find("Slash/CurrentHealthText").gameObject;
                    util.Component.Add(item, oblc.Find("BarRoots/HealthbarRoot/Slash/CurrentHealthText").GetComponent<HGTextMeshProUGUI>());

                }
                {   //FullHealthText
                    UIOverhaul.Logger.LogMessage("Processing FullHealthText");
                    item = _HealthbarRoot.Find("Slash/FullHealthText").gameObject;
                    util.Component.Add(item, oblc.Find("BarRoots/HealthbarRoot/Slash/FullHealthText").GetComponent<HGTextMeshProUGUI>());

                }
                //  back to HealthBarRoot
                UIOverhaul.Logger.LogMessage("Processing HealthBarRoot");
                item = _HealthbarRoot.gameObject;
                HealthBar temp = util.Component.Add(item, oblc.Find("BarRoots/HealthbarRoot").GetComponent<HealthBar>());

                //need to use references to our objects here, rather than the old references
                temp.barContainer = _HealthbarRoot.Find("ShrunkenRoot").GetComponent<RectTransform>();
                temp.currentHealthText = _HealthbarRoot.Find("Slash/CurrentHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added
                temp.fullHealthText = _HealthbarRoot.Find("Slash/FullHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added

                //HealthBar temp = item.gameObject.AddComponent<HealthBar>();
                //temp.style = Resources.Load<HealthBarStyle>("ScriptableObject/HUDHealthBar");//PLS WORK
                //temp.style = oblc.transform.Find("BarRoots/HealthbarRoot").GetComponent<HealthBar>().style;
            }
            {   //LevelDisplayCluster
                {   //LevelDisplayRoot
                    {   //ValueText
                        UIOverhaul.Logger.LogMessage("Processing ValueText");
                        item = _LevelDisplayRoot.Find("ValueText").gameObject;
                        util.Component.Add(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot/ValueText").GetComponent<HGTextMeshProUGUI>());

                    }
                    {   //PrefixText
                        UIOverhaul.Logger.LogMessage("Processing PrefixText");
                        item = _LevelDisplayRoot.Find("PrefixText").gameObject;
                        util.Component.Add(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot/PrefixText").GetComponent<HGTextMeshProUGUI>());

                    }
                    UIOverhaul.Logger.LogMessage("Processing LevelDisplayRoot");
                    item = _LevelDisplayRoot.gameObject;
                    LevelText temp = util.Component.Add(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot").GetComponent<LevelText>());
                    temp.targetText = _LevelDisplayRoot.Find("ValueText").GetComponent<HGTextMeshProUGUI>();
                }
                {   //BuffDisplayRoot
                    UIOverhaul.Logger.LogMessage("Processing BuffDisplayRoot");
                    item = _BuffDisplayRoot.gameObject;
                    util.Component.Add(item, oblc.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot").GetComponent<BuffDisplay>());
                }
                {   //ExpBarRoot
                    UIOverhaul.Logger.LogMessage("Processing ExpBarRoot");
                    item = _ExpBarRoot.gameObject;
                    ExpBar temp = util.Component.Add(item, oblc.Find("BarRoots/LevelDisplayCluster/ExpBarRoot").GetComponent<ExpBar>());
                    temp.fillRectTransform = _ExpBarRoot.Find("ShrunkenRoot/FillPanel").GetComponent<RectTransform>();
                }
            }
            UIOverhaul.Logger.LogMessage("Finished Processing!!");
            //finally Properly Destroy old cluster
            UnityEngine.Object.DestroyImmediate(oblc.gameObject);
            UIOverhaul.Logger.LogMessage("Old Cluster Deleted Successfully.");
        }

        //private void OnDestroy()
        //{
        //    On.RoR2.UI.HUD.Awake -= CustomHUDAssembly;
        //}
    }
}
