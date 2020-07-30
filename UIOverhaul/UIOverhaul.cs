using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;

namespace UIOverhaul
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(LanguageAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class UIOverhaul : BaseUnityPlugin
    {
        private const string ModVer = "0.0.1";
        private const string ModName = "UIOverhaul";
        public const string ModGuid = "com.WaterWhen.UIOverhaul";

        internal new static ManualLogSource Logger; // allow access to the logger across the plugin classes

        private UnityEngine.Transform HUDroot = null;

        public void Awake()
        {
            Logger = base.Logger;

            //Assets.Init();
            //Hooks.Init();
            On.RoR2.UI.HUD.Awake += newHUD;
        }
        
        private void newHUD(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self); // Don't forget to call this, or the vanilla / other mods' codes will not execute!
            HUDroot = self.transform.root; // This will return the canvas that the UI is displaying on!
                                           // Rest of the code is to go here

            HUDroot.DetachChildren();
            // UnityEngine.Transform child = HUDroot.GetChild(0);
            // Destroy(child.gameObject);
            // child.parent = null;
            // for(int i = 0; i < HUDroot.childCount; i--)
            // {
            //     UnityEngine.Transform child = HUDroot.GetChild(i);
            //     Destroy(child.gameObject);
            //     child.parent = null;
            // }

            // HUDroot.DetachChildren();
        }

        private void OnDestroy()
        {
            On.RoR2.UI.HUD.Awake -= newHUD;
        }
    }
}
