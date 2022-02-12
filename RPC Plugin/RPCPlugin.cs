using BepInEx;
using BepInEx.Logging;
using RPCPlugin.RPC;

namespace RPCPlugin
{
    [BepInPlugin(Guid, Name, Version)]
    public class RPCPlugin: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "HolloFoxes' RPC Plug-In";
        public const string Guid = "org.hollofox.plugins.rpc";
        public const string Version = "0.9.2.0";
        public static RPCPlugin _instance;
        internal static ManualLogSource BPLogger;

        /// <summary>
        /// Method triggered when the plugin loads
        /// </summary>
        public void Awake()
        {
            BPLogger = Logger;
            Logger.LogInfo($"In Awake for {Name}");
            _instance = this;
        }

        /// <summary>
        /// Triggered every frame
        /// </summary>
        public void Update()
        {
            if (PhotonNetwork.inRoom && RPCManager.Instance == null)
            {
                RPCManager.Init();
            }
        }
    }
}
