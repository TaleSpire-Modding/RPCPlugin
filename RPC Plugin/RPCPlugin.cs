using BepInEx;
using BepInEx.Logging;
using RPCPlugin.RPC;
using UnityEngine;

namespace RPCPlugin
{
    [BepInPlugin(Guid, Name, Version)]
    public class RPCPlugin: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "HolloFoxes' RPC Plug-In";
        public const string Guid = "org.hollofox.plugins.rpc";
        public const string Version = "0.9.0.0";
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
            // RPCManager.handlers.Add("/w",handler);
        }

        /*private string handler(string arg1, string arg2, RPCManager.ChatSource arg3)
        {
            Debug.Log($"This is the handler:{arg1},{arg2},{arg3}");
            return arg1;
        }*/

        /// <summary>
        /// Triggered every frame
        /// </summary>
        public void Update()
        {
            if (PhotonNetwork.inRoom && RPCManager.Instance == null)
            {
                RPCManager.Init();
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.S))
            {
                RPCManager.SendChatMessage("/w This is a test", LocalPlayer.Id.Value);
            }
        }
    }
}
