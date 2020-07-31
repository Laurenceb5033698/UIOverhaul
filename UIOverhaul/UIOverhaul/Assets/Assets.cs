using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;
using Unity.Collections;
using System;
using System.Collections.Generic;

namespace UIOverhaul {
    internal static class Assets {
        internal static GameObject UIBottomLeftCluster;//prefab, static so it's available from anywhere.

        private const string ModPrefix = "@UIOverhaul:";
        private const string PrefabPath = ModPrefix + "Assets/CustomHud/BottomLeftCluster.prefab";

        internal static void Init() {
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UIOverhaul.bottomleftcluster")) {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                UIBottomLeftCluster = bundle.LoadAsset<GameObject>("Assets/CustomHud/BottomLeftCluster.prefab");
            }
            //Util.PrintGameObjectChildren(UIBottomLeftCluster); //print out hierarchy to debug console when running
        }
    }
}
