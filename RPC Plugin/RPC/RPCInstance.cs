using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Bounce.ManagedCollections;
using Bounce.Unmanaged;
using Newtonsoft.Json;
using Photon;
using Talespire;
using UnityEngine;

namespace RPCPlugin.RPC
{
    public class RPCInstance: PunBehaviour
    {
        internal static Dictionary<string, Func<string, string, SourceRole, string>> Handlers = new Dictionary<string, Func<string, string, SourceRole, string>>();
        internal static RPCInstance Instance;
        internal static PhotonView View;

        public static void Init()
        {
            if (Instance != null) return;
            var rpc = Instantiate(new GameObject());
            Instance = rpc.AddComponent<RPCInstance>();
            View = rpc.AddComponent<PhotonView>();
            View.viewID = 606;
        }

        public static void SendMessage(string message, NGuid thingThatIsTalking)
        {
            var photonView = View;
            var sourceRole = SourceRole.anonymous;
            if (LocalPlayer.Id.Value == thingThatIsTalking)
                sourceRole = CampaignSessionManager.PlayersInfo[LocalPlayer.Id].Rights.CanGm ?  
                    SourceRole.gm : SourceRole.player;
            if (sourceRole == SourceRole.anonymous)
                Parallel.ForEach(CreaturePresenter.AllCreatureAssets, asset =>
                {
                    if (asset.Creature.UniqueId.Value == thingThatIsTalking)
                        sourceRole = SourceRole.creature;
                });
            if (sourceRole == SourceRole.anonymous)
                Parallel.ForEach(
                    (BList<HideVolumeItem>) typeof(HideVolumeManager).GetProperty("_hideVolumeItems", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null),
                    volume =>
                    {
                        if (volume.HideVolume.Id == thingThatIsTalking) sourceRole = SourceRole.hideVolume;
                    });
            photonView.RPC(nameof(ReceivedMessage), PhotonTargets.All, new object[] { message, thingThatIsTalking.ToString(), JsonConvert.SerializeObject(sourceRole) });
        }

        [PunRPC]
        public void ReceivedMessage(string message, string thingThatIsTalking, string chatSource)
        {
            var source = JsonConvert.DeserializeObject<SourceRole>(chatSource);
            Parallel.ForEach(Handlers, handler =>
            {
                if (message.StartsWith(handler.Key))
                    _ = handler.Value(message, thingThatIsTalking, source);
            });
        }
    }
}