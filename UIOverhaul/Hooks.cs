using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

namespace UIOverhaul
{
    public class Hooks
    {
        private Transform OldHUDroot = null;

        internal void Init()
        {
            On.RoR2.UI.HUD.Awake += CustomHUDAssembly;

        }

        //This approach is aimed at altering the OldHud's hierarchy and inserting our own.
        //The General Idea is: a precision cut, instantiate custom prefabs, copy values, delete old, re-parent new (to exisiting Hud root).
        //I want to make use of as much existing code, so we don't have to re-write the whole UI.
        private void CustomHUDAssembly(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            OldHUDroot = self.transform;

            SetupBottomLeftCluster();   //set up HpBar, ExpBar, BuffBar, ChatBox
            

            //We want to chop and change the HUD layout before we call the default function.
            //that way when we remove items and add our own, we shouldnt be missing out on default setup.
            orig(self);
            throw new NotImplementedException();
        }

        private void SetupBottomLeftCluster()
        {
            Transform oblc = OldHUDroot.Find("BottomLeftCluster");  //old BottomLeftCluster transform
            Transform oblcparent = oblc.parent; //get existing parent
            oblc.parent = null; //detatch this one object.

            //instantiate new UI cluster and set parent.
            Transform _BLC = UnityEngine.Object.Instantiate<GameObject>(Assets.UIBottomLeftCluster, oblcparent).transform;

            //get references to some of the object parents
            //This is mostly to reduce path length when searching for Gameobjects (reduce risk of spelling errors)
            Transform _HealthbarRoot = _BLC.Find("BarRoots/HealthbarRoot");
            Transform _LevelDisplayRoot = _BLC.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot");
            Transform _BuffDisplayRoot = _BLC.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot");
            Transform _ExpBarRoot = _BLC.Find("BarRoots/LevelDisplayCluster/ExpBarRoot");

            //now go through each Hud item and make sure game scripts are setup accordingly.
            //normally this is done via editor, but we don't have access to RoR2 scripts in editor ¯\_(ツ)_/¯
            GameObject item = null;
            {   //chatBox
                item = _BLC.Find("ChatBoxRoot").gameObject;
                InstantiatePrefabOnStart temp = item.AddComponent<InstantiatePrefabOnStart>();
                temp.prefab = Resources.Load<GameObject>("Prefabs/ChatBox, In Run");//this should be okay
                temp.targetTransform = item.transform;
                temp.copyTargetRotation = true;
                temp.parentToTarget = true;
            }
            {   //HealthBar root
                {   //Slash
                    item = _HealthbarRoot.Find("Slash").gameObject;
                    Util.AddComponent(item, _BLC.Find("BarRoots/HealthbarRoot/Slash").GetComponent<HGTextMeshProUGUI>());
                    //add any changes to text here e.g. font, size etc
                }
                {   //CurrentHealthText
                    item = _HealthbarRoot.Find("Slash/CurrentHealthText").gameObject;
                    Util.AddComponent(item, oblc.Find("BarRoots/HealthbarRoot/Slash/CurrentHealthText").GetComponent<HGTextMeshProUGUI>());

                }
                {   //FullHealthText
                    item = _HealthbarRoot.Find("Slash/FullHealthText").gameObject;
                    Util.AddComponent(item, oblc.Find("BarRoots/HealthbarRoot/Slash/FullHealthText").GetComponent<HGTextMeshProUGUI>());

                }
                //  back to HealthBarRoot
                item = _HealthbarRoot.gameObject;
                HealthBar temp = Util.AddComponent(item, oblc.Find("BarRoots/HealthbarRoot").GetComponent<HealthBar>());

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
                        item = _LevelDisplayRoot.Find("ValueText").gameObject;
                        Util.AddComponent(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot/ValueText").GetComponent<HGTextMeshProUGUI>());

                    }
                    {   //PrefixText
                        item = _LevelDisplayRoot.Find("PrefixText").gameObject;
                        Util.AddComponent(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot/PrefixText").GetComponent<HGTextMeshProUGUI>());

                    }
                    item = _LevelDisplayRoot.gameObject;
                    LevelText temp = Util.AddComponent(item, oblc.Find("BarRoots/LevelDisplayCluster/LevelDisplayRoot").GetComponent<LevelText>());
                    temp.targetText = _LevelDisplayRoot.Find("ValueText").GetComponent< HGTextMeshProUGUI>();
                }
                {   //BuffDisplayRoot
                    item = _BuffDisplayRoot.gameObject;
                    Util.AddComponent(item, oblc.Find("BarRoots/LevelDisplayCluster/BuffDisplayRoot").GetComponent<BuffDisplay>());
                }
                {   //ExpBarRoot
                    item = _ExpBarRoot.gameObject;
                    ExpBar temp = Util.AddComponent(item, oblc.Find("BarRoots/LevelDisplayCluster/ExpBarRoot").GetComponent<ExpBar>());
                    temp.fillRectTransform = _ExpBarRoot.Find("ShrunkenRoot/FillPanel").GetComponent<RectTransform>();
                }
            }
        }

              

        private void OnDestroy()
        {
            On.RoR2.UI.HUD.Awake -= CustomHUDAssembly;
        }

    }


}
