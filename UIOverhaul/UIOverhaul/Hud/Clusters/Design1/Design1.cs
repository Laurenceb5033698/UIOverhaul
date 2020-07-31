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
                public static void SetupCluster(){
                    SetupBottomLeftCluster();
                }

                //set up HpBar, ExpBar, BuffBar, ChatBox
                private static void SetupBottomLeftCluster() {
                    //detatch this one object.
                    clusters.BLC.getOriginal().parent = null;
                    UIOverhaul.Logger.LogMessage("Parent disassociated.");

                    //instantiate new UI cluster and set parent.
                    Transform _BLC = UnityEngine.Object.Instantiate<GameObject>(Assets.UIBottomLeftCluster, clusters.BLC.getOriginalParent()).transform;
                    UIOverhaul.Logger.LogMessage("Custom HUD instantiated.");

                    //now go through each Hud item and make sure game scripts are setup accordingly.
                    //normally this is done via editor, but we don't have access to RoR2 scripts in editor ¯\_(ツ)_/¯

                    {   //chatBox
                        clusters.BLC.setItem(_BLC, "Processing ChatBoxRoot");

                        InstantiatePrefabOnStart tempIPOS = clusters.BLC.getItem().AddComponent<InstantiatePrefabOnStart>();
                        tempIPOS.prefab = Resources.Load<GameObject>("Prefabs/ChatBox, In Run");//this should be okay
                        tempIPOS.targetTransform = clusters.BLC.getItem().transform;
                        tempIPOS.copyTargetRotation = true;
                        tempIPOS.parentToTarget = true;
                    }

                    {   //HealthBar root
                        {   //Slash
                            clusters.BLC.processComponent(clusters.BLC.getHealthbarRoot(), "Slash", "BarRoots/HealthbarRoot/Slash", "Processing Slash");
                            // clusters.BLC.processComponent(_BLC, "Slash", "BarRoots/HealthbarRoot/Slash", "Processing Slash");  
                            //add any changes to text here e.g. font, size etc
                        }
                        {   //CurrentHealthText
                            clusters.BLC.processComponent(clusters.BLC.getHealthbarRoot(), "Slash/CurrentHealthText", "BarRoots/HealthbarRoot/Slash/CurrentHealthText", "Processing CurrentHealthText");
                        }
                        {   //FullHealthText
                            clusters.BLC.processComponent(clusters.BLC.getHealthbarRoot(), "Slash/FullHealthText", "BarRoots/HealthbarRoot/Slash/FullHealthText", "Processing FullHealthText");
                        }
                        //back to HealthBarRoot
                        clusters.BLC.setItem(clusters.BLC.getHealthbarRoot().gameObject, "Processing HealthBarRoot");
                        HealthBar tempHB = util.Component.Add(clusters.BLC.getItem(), clusters.BLC.OriginalFind("BarRoots/HealthbarRoot").GetComponent<HealthBar>());

                        //need to use references to our objects here, rather than the old references
                        tempHB.barContainer = clusters.BLC.HealthbarRootFind("ShrunkenRoot").GetComponent<RectTransform>();
                        tempHB.currentHealthText = clusters.BLC.HealthbarRootFind("Slash/CurrentHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added
                        tempHB.fullHealthText = clusters.BLC.HealthbarRootFind("Slash/FullHealthText").GetComponent<HGTextMeshProUGUI>();//cannot do this until all HGTextMeshProUGUI components added
                        //HealthBar temp = item.gameObject.AddComponent<HealthBar>();
                        //temp.style = Resources.Load<HealthBarStyle>("ScriptableObject/HUDHealthBar");//PLS WORK
                        //temp.style = oblc.transform.Find("BarRoots/HealthbarRoot").GetComponent<HealthBar>().style;
                    }

                    {   //LevelDisplayCluster
                        {   //LevelDisplayRoot
                            {   //ValueText
                                clusters.BLC.processComponent(clusters.BLC.getLevelDisplayRoot(), "ValueText", "BarRoots/LevelDisplayCluster/LevelDisplayRoot/ValueText", "Processing ValueText");
                            }
                            {   //PrefixText
                                clusters.BLC.processComponent(clusters.BLC.getLevelDisplayRoot(), "PrefixText", "BarRoots/LevelDisplayCluster/LevelDisplayRoot/PrefixText", "Processing PrefixText");
                            }
                            clusters.BLC.setItem(clusters.BLC.getLevelDisplayRoot().gameObject, "Processing LevelDisplayRoot");
                            LevelText tempLT = util.Component.Add(clusters.BLC.getItem(), clusters.BLC.OriginalFind("BarRoots/LevelDisplayCluster/LevelDisplayRoot").GetComponent<LevelText>());
                            tempLT.targetText = clusters.BLC.LevelDisplayRootFind("ValueText").GetComponent<HGTextMeshProUGUI>();
                        }

                        {   //BuffDisplayRoot
                            clusters.BLC.setItem(clusters.BLC.getBuffDisplayRoot(), "Processing BuffDisplayRoot");
                            util.Component.Add(clusters.BLC.getItem(), clusters.BLC.OriginalFind("BarRoots/LevelDisplayCluster/BuffDisplayRoot").GetComponent<BuffDisplay>());
                        }
                        
                        {   //ExpBarRoot
                            clusters.BLC.setItem(clusters.BLC.getExpBarRoot(), "Processing ExpBarRoot");
                            ExpBar tempExpBar = util.Component.Add(clusters.BLC.getItem(), clusters.BLC.OriginalFind("BarRoots/LevelDisplayCluster/ExpBarRoot").GetComponent<ExpBar>());
                            tempExpBar.fillRectTransform = clusters.BLC.ExpBarRootFind("ShrunkenRoot/FillPanel").GetComponent<RectTransform>();
                        }
                    }

                    UIOverhaul.Logger.LogMessage("Finished Processing!!");
                    //finally Properly Destroy old cluster
                    UnityEngine.Object.DestroyImmediate(clusters.BLC.getOriginal().gameObject);
                    UIOverhaul.Logger.LogMessage("Old Cluster Deleted Successfully.");
                }
            }
        }
    }
}