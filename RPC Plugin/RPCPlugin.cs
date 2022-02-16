using BepInEx;
using HarmonyLib;
using RPCPlugin.RPC;

namespace RPCPlugin
{
    [BepInPlugin(Guid, Name, Version)]
    public class RPCPlugin: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "HolloFoxes' RPC Plug-In";
        public const string Guid = "org.hollofox.plugins.rpc";
        public const string Version = "1.0.1.0";

        /// <summary>
        /// Method triggered when the plugin loads
        /// </summary>
        public void Awake()
        {
            Logger.LogInfo($"In Awake for {Name}");
            var harmony = new Harmony(Guid);
            harmony.PatchAll();
        }

        /// <summary>
        /// Triggered every frame
        /// </summary>
        public void Update()
        {
            if (RPCInstance.Instance == null && PhotonNetwork.inRoom)
                RPCInstance.Init();
        }
    }
}
