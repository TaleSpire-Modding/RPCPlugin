using BepInEx;
using HarmonyLib;

namespace SampleRPCConsumer
{
    [BepInPlugin(Guid, Name, Version)]
    public class RPCPlugin: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "HolloFoxes' Sample RPC Consumer";
        public const string Guid = "org.hollofox.plugins.rpc.consumer";
        public const string Version = "0.0.0.0";

        /// <summary>
        /// Method triggered when the plugin loads
        /// </summary>
        public void Awake()
        {
            Logger.LogInfo($"In Awake for {Name}");
            var harmony = new Harmony(Guid);
            harmony.PatchAll();
        }

        private bool _registerSingletons = true;

        /// <summary>
        /// Triggered every frame
        /// </summary>
        public void Update()
        {

        }
    }
}
