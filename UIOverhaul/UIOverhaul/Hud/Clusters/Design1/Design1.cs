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
            public static class Design1 {
                private static RoR2.UI.HUD gameHud = null;
                private static RoR2.UI.HUDScaleController gameHudScaleController = null;

                //set up Design specific clusters. Beware These may not match clusters from Original HUD if we moved them.
                public static void SetupCluster(){
                    //get reference to game's HUD component
                    gameHud = Hud.OriginalHUDroot.GetComponent<HUD>();
                    gameHudScaleController = Hud.OriginalHUDroot.GetComponent<HUDScaleController>();

                    SetupBottomLeftCluster();       //hp, xp, buffs, chat
                    //SetupBottomRightCluster();    //skills, equipment
                    //SetupBottomCentreCluster();   //items, popups
                    //SetupTopRightCluster();       //Difficulty, objectives
                    //etc
                }

                //set up HpBar, ExpBar, BuffBar, ChatBox
                private static void SetupBottomLeftCluster() {
                    //detatch this one object.
                    clusters.BLC.getOriginal().parent = null;
                    UIOverhaul.Logger.LogMessage("Parent disassociated.");

                    //instantiate new UI cluster and set parent. //Design1_BottomLeftCluster
                    Transform D1_BLC = UnityEngine.Object.Instantiate<GameObject>(Assets.UIBottomLeftCluster, clusters.BLC.getOriginalParent()).transform;
                    UIOverhaul.Logger.LogMessage("Custom HUD instantiated.");

                    //find referneces for new cluster
                    Transform D1_HealthbarRoot = D1_BLC.Find("BarRoots/HealthbarRoot");
                    Transform D1_LevelDisplayRoot = D1_BLC.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot");
                    Transform D1_BuffDisplayRoot = D1_BLC.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot");
                    Transform D1_ExpBarRoot = D1_BLC.Find("BarRoots/LevelDisplayCluster/ExpBarRoot");
                    //now go through each Hud item and make sure game scripts are setup accordingly.
                    //normally this is done via editor, but we don't have access to RoR2 scripts in editor ¯\_(ツ)_/¯
                    
                    {   //chatBox
                        GameObject item = D1_BLC.Find("ChatBoxRoot").gameObject;

                        InstantiatePrefabOnStart tempIPOS = item.AddComponent<InstantiatePrefabOnStart>();
                        tempIPOS.prefab = Resources.Load<GameObject>("Prefabs/ChatBox, In Run");//this should be okay
                        tempIPOS.targetTransform = item.transform;
                        tempIPOS.copyTargetRotation = true;
                        tempIPOS.parentToTarget = true;
                    }

                    {   //HealthBar root
                        {   //Slash
                            // I'm leaving HGTMP in as we will no doubt want to change something about the font
                            HGTextMeshProUGUI HGTMP = BLC.CopyHGTMPvalues(D1_HealthbarRoot, BLC.getHealthbarRoot() ,"Slash", "Slash", "Processing Slash");
                            //add any changes to text here e.g. font, size etc
                            //HGTMP.fontSize = 42;  //e.g.
                        }
                        {   //CurrentHealthText
                            HGTextMeshProUGUI HGTMP = BLC.CopyHGTMPvalues(D1_HealthbarRoot, BLC.getHealthbarRoot(), "Slash/CurrentHealthText", "Slash/CurrentHealthText", "Processing CurrentHealthText");
                        }
                        {   //FullHealthText
                            HGTextMeshProUGUI HGTMP = BLC.CopyHGTMPvalues(D1_HealthbarRoot, BLC.getHealthbarRoot(), "Slash/FullHealthText", "Slash/FullHealthText", "Processing FullHealthText");
                        }
                        //back to HealthBarRoot
                        util.Log.ConsoleMessage("Processing HealthbarRoot.");
                        //get gameobject from custom Hud HealthbarRoot
                        GameObject item = D1_HealthbarRoot.gameObject;

                        item.SetActive(false);  //temporarily disable object while adding healthbar to prevent early awake call.
                        //use script on original HealthbarRoot to set values on new component on custom HealthbarRoot
                        HealthBar customHB = util.Component.Add(item, BLC.getHealthbarRoot().GetComponent<HealthBar>());

                        //need to use references to our objects here, rather than the old references
                        customHB.barContainer = D1_HealthbarRoot.Find("ShrunkenRoot").GetComponent<RectTransform>();
                        customHB.currentHealthText = D1_HealthbarRoot.Find("Slash/CurrentHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added
                        customHB.fullHealthText = D1_HealthbarRoot.Find("Slash/FullHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added
                        //customHB.style = Resources.Load<HealthBarStyle>("ScriptableObject/HUDHealthBar");//PLS WORK
                        
                        customHB.style = BLC.getHealthbarRoot().GetComponent<HealthBar>().style;
                        
                        item.SetActive(true);   //re-enable, Healthbar should call awake now.
                        //Tell games's HUD component about new reference
                        gameHud.healthBar = customHB;
                    }

                    {   //LevelDisplayCluster
                        {   //LevelDisplayRoot
                            {   //ValueText
                                HGTextMeshProUGUI HGTMP = BLC.CopyHGTMPvalues(D1_LevelDisplayRoot, BLC.getLevelDisplayRoot(), "ValueText", "ValueText", "Processing ValueText");
                            }
                            {   //PrefixText
                                HGTextMeshProUGUI HGTMP = BLC.CopyHGTMPvalues(D1_LevelDisplayRoot, BLC.getLevelDisplayRoot(), "PrefixText", "PrefixText", "Processing PrefixText");
                            }
                            GameObject item = D1_LevelDisplayRoot.gameObject;
                            LevelText customLT = util.Component.Add(item, BLC.getLevelDisplayRoot().GetComponent<LevelText>());
                            customLT.targetText = D1_LevelDisplayRoot.Find("ValueText").GetComponent<HGTextMeshProUGUI>();

                            //set hud ref
                            gameHud.levelText = customLT;
                        }

                        {   //BuffDisplayRoot
                            util.Log.ConsoleMessage("Processing BuffDisplayRoot.");

                            GameObject item = D1_BuffDisplayRoot.gameObject;
                            item.SetActive(false);
                            BuffDisplay customBD = util.Component.Add(item, BLC.getBuffDisplayRoot().GetComponent<BuffDisplay>());
                            customBD.buffIconPrefab = Resources.Load<GameObject>("Prefabs/BuffIcon");
                            util.Log.ConsoleMessage("Processing attempted. BuffDisplay: " + customBD.ToString());
                            if (customBD.buffIconPrefab != null)
                                util.Log.ConsoleMessage("BD.style Resource Loading: " + customBD.buffIconPrefab.ToString());
                            else
                            {
                                customBD.buffIconPrefab = BLC.getBuffDisplayRoot().GetComponent<BuffDisplay>().buffIconPrefab;
                                util.Log.ConsoleMessage("BD.style Copy from old: " + customBD.buffIconPrefab.ToString());
                            }
                            item.SetActive(true);
                            //set hud ref
                            gameHud.buffDisplay = customBD;
                        }
                        
                        {   //ExpBarRoot
                            GameObject item = D1_ExpBarRoot.gameObject;
                            ExpBar customEB = util.Component.Add(item, BLC.getExpBarRoot().GetComponent<ExpBar>());
                            customEB.fillRectTransform = D1_ExpBarRoot.Find("ShrunkenRoot/FillPanel").GetComponent<RectTransform>();

                            //set hud ref
                            gameHud.expBar = customEB;
                        }
                    }

                    UIOverhaul.Logger.LogMessage("Finished Processing!!");
                    //finally Properly Destroy old cluster
                    UnityEngine.Object.DestroyImmediate(clusters.BLC.getOriginal().gameObject);
                    UIOverhaul.Logger.LogMessage("Old Cluster Deleted Successfully.");

                    //assign to scale controller
                    gameHudScaleController.rectTransforms[0] = D1_BLC.GetComponent<RectTransform>();
                }
            }
        }
    }
}