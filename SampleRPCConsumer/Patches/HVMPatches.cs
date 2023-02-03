namespace SampleRPCConsumer.Patches
{
    // [HarmonyPatch(typeof(HideVolumeManager), "OnHideVolumeAdded")]
    // internal class HVMAddPatch
    // {
    //     public static void Postfix(ref HideVolume volume, ref BList<HideVolumeItem> ____hideVolumeItems)
    //     {
    //         RPCInstance.hideVolumes = ____hideVolumeItems;
    //     }
    // }
    //
    // [HarmonyPatch(typeof(HideVolumeManager), "OnHideVolumeRemoved")]
    // internal class HVMRemovePatch
    // {
    //     public static void Postfix(ref HideVolume volume, ref BList<HideVolumeItem> ____hideVolumeItems)
    //     {
    //         RPCInstance.hideVolumes = ____hideVolumeItems;
    //     }
    // }
}
