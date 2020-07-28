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

        public void Awake()
        {
            Logger = base.Logger;

            //Assets.Init();
            //Hooks.Init();
        }
    }
}
