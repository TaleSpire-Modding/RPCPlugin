using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using RPCPlugin.RPC;
using SampleRPCConsumer.Consumer;
using SampleRPCConsumer.Consumer.Messages;
using UnityEngine;

namespace SampleRPCConsumer
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(RPCPlugin.RPCPlugin.Guid)]
    public class RPCConsumers: BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "Author' Sample RPC Consumer";
        public const string Guid = "org.author.rpc.consumer";
        public const string Version = "0.0.0.0";

        // Configuration
        private ConfigEntry<KeyboardShortcut> TriggerKey { get; set; }      // Sample configuration for triggering a plugin via keyboard
        private ConfigEntry<KeyboardShortcut> TriggerKey2 { get; set; }     // Sample configuration for triggering a plugin via keyboard

        /// <summary>
        /// Method triggered when the plugin loads
        /// </summary>
        public void Awake()
        {
            Logger.LogInfo($"In Awake for {Name}");
            var harmony = new Harmony(Guid);
            DoConfig(Config);
            harmony.PatchAll();
        }

        /// <summary>
        /// Establishes the config for the plugin,
        /// </summary>
        /// <param name="config">Configfile regarding the specific plugin</param>
        public void DoConfig(ConfigFile config)
        {
            TriggerKey = config.Bind("Hotkeys", "S", new KeyboardShortcut(KeyCode.S, KeyCode.LeftControl));
            TriggerKey2 = config.Bind("Hotkeys", "W", new KeyboardShortcut(KeyCode.W, KeyCode.LeftControl));
        }

        /// <summary>
        /// Method triggered every physics update
        /// </summary>
        void Update()
        {
            // Can be used to determine if a board is loaded
            // Beware: Board loaded does not necessarily mean all the minis have properly loaded and are accessible
            if (!Utility.Utility.isBoardLoaded()) return;
            
            // Proper way to check for keyboard combinations. This not only tests for the required keys but it also checks
            // for not required keys. For example, if the required keys are LCTRL+M then LCTRL+SHIFT+M will not result in
            // a true check. Using the regular Unity Input check or even the TriggerKey.Value.IsUp() result the check being
            // true even if other modifiers are being pressed.
            if (Utility.Utility.StrictKeyCheck(TriggerKey.Value))
            {
                var message = new MyMessage
                {
                    I = 1
                };
                RPCInstance.SendMessage(message);
            }

            if (Utility.Utility.StrictKeyCheck(TriggerKey2.Value))
            {
                var message = new MyMessage2
                {
                    I = 2
                };
                MyConsumer2.Instance.SendMessage(message);
            }
        }
    }
}
