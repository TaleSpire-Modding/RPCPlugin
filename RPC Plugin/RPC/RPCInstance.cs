using System;
using System.Collections.Generic;
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
            if (Instance == null)
            {
                var o = new GameObject();
                var rpc = Instantiate(o);
                Instance = rpc.AddComponent<RPCInstance>();
                View = rpc.AddComponent<PhotonView>();
                View.viewID = 606;
            }
        }

        public static void SendMessage(string message, NGuid thingThatIsTalking)
        {
            PhotonView photonView = View;
            SourceRole sourceRole = SourceRole.anonymous;
            if (LocalPlayer.Id.Value == thingThatIsTalking)
                sourceRole = CampaignSessionManager.PlayersInfo[LocalPlayer.Id].Rights.CanGm ?  
                    SourceRole.gm : SourceRole.player;
            foreach (CreatureBoardAsset asset in CreaturePresenter.AllCreatureAssets)
            {
                if (asset.Creature.UniqueId.Value == thingThatIsTalking)
                {
                    sourceRole = SourceRole.creature;
                }
            }
            photonView.RPC(nameof(ReceivedMessage), PhotonTargets.All, new object[] { message, thingThatIsTalking.ToString(), JsonConvert.SerializeObject(sourceRole) });
        }

        [PunRPC]
        public void ReceivedMessage(string message, string thingThatIsTalking, string chatsource)
        {
            var source = JsonConvert.DeserializeObject<SourceRole>(chatsource);
            foreach (KeyValuePair<string, Func<string, string, SourceRole, string>> handler in Handlers)
            {
                if (message.StartsWith(handler.Key))
                {
                    _ = handler.Value(message,thingThatIsTalking,source);
                }
            }
        }
    }
}
