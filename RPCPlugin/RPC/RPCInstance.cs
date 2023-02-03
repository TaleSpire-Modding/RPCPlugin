using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bounce.ManagedCollections;
using Bounce.Unmanaged;
using Newtonsoft.Json;
using Photon;
using RPCPlugin.Interfaces;
using Talespire;
using UnityEngine;

namespace RPCPlugin.RPC
{
    public sealed class RPCInstance: PunBehaviour
    {
        internal static Dictionary<string, Func<string, string, SourceRole, string>> Handlers = new Dictionary<string, Func<string, string, SourceRole, string>>();
        internal static RPCInstance Instance;
        internal static PhotonView View;
        internal static BList<HideVolumeItem> hideVolumes;

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
                    if (asset.CreatureId.Value == thingThatIsTalking)
                        sourceRole = SourceRole.creature;
                });
            if (sourceRole == SourceRole.anonymous)
                Parallel.ForEach(
                    hideVolumes,
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


        /// <summary>
        /// Tracking consumers to unique id
        /// </summary>
        internal static Dictionary<Guid, IBinaryFactory> Binaries = new Dictionary<Guid, IBinaryFactory>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rpcMessage"></param>
        /// <param name="targets"></param>
        public static void SendMessage(RpcMessage rpcMessage, PhotonTargets targets = PhotonTargets.All)
        {
            if (View != null)
            {
                View.RPC(nameof(ReceivedBinaryMessage), targets, rpcMessage.Id(), rpcMessage.Value());
            }
        }

        /// <summary>
        /// Received binary arrays which will be used to re-construct the message and handle the response
        /// </summary>
        /// <param name="bid">unique id of the consumer to use</param>
        /// <param name="message">the binarialized data</param>
        [PunRPC]
        public void ReceivedBinaryMessage(byte[] bid, byte[] message)
        {
            var id = new Guid(bid);

            if (!Binaries.ContainsKey(id))
            {
                Debug.Log($"RPC Key not found: {id}");
                return;
            }
            Binaries[id].Handle(message);
        }
    }
}