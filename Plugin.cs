global using BepInEx;
global using BepInEx.Logging;
global using HarmonyLib;

using FP2_Knux_Objects.Patchers;

namespace FP2_Knux_Objects
{
    [BepInPlugin("K24_FP2_CustomObjects", "Knux's Custom Objects", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Logger.
        public static ManualLogSource consoleLog;

        private void Awake()
        {
            // Set up the logger.
            consoleLog = Logger;

            // Patch all the functions that need patching.
            Harmony.CreateAndPatchAll(typeof(FPPlayerPatcher));
        }
    }
}
