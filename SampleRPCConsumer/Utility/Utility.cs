using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

namespace LordAshes
{
    /// <summary>
    /// This utility class originated by LordAshes
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Function to check if the board is loaded
        /// </summary>
        /// <returns></returns>
        public static bool IsBoardLoaded()
        {
            return CameraController.HasInstance && BoardSessionManager.HasInstance && !BoardSessionManager.IsLoading;
        }

        /// <summary>
        /// Method to properly evaluate shortcut keys. 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool StrictKeyCheck(KeyboardShortcut check)
        {
            return check.IsUp() && new KeyCode[] { KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftShift, KeyCode.RightShift }.All(modifier => Input.GetKey(modifier) == check.Modifiers.Contains(modifier));
        }
    }
}
