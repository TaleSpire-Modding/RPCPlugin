using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModdingTales;
using PluginUtilities;
using RPCPlugin.Interfaces;
using RPCPlugin.RPC;

namespace RPCPlugin
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(SetInjectionFlag.Guid)]
    public class RPCPlugin: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "RPC Plug-In";
        public const string Guid = "org.hollofox.plugins.rpc";
        public const string Version = "0.0.0.0";

        internal static ManualLogSource InternalLogger;
        
        /// <summary>
        /// Method triggered when the plugin loads
        /// </summary>
        public void Awake()
        {
            InternalLogger = Logger;
            Logger.LogInfo($"In Awake for {Name}");
            var harmony = new Harmony(Guid);
            harmony.PatchAll();
            ModdingUtils.AddPluginToMenuList(this, "HolloFoxes'");
        }

        private bool _registerSingletons = true;

        /// <summary>
        /// Triggered every frame
        /// </summary>
        public void Update()
        {
            if (_registerSingletons)
            {
                InitOnLoad.Initialise();
                _registerSingletons = false;
            }

            if (RPCInstance.Instance == null && PhotonNetwork.inRoom)
                RPCInstance.Init();
        }
    }
}
