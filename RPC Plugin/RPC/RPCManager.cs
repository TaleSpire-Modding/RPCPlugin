using System;
using System.Collections.Generic;
using Bounce.Unmanaged;
using Newtonsoft.Json;
using Photon;
using UnityEngine;

namespace RPCPlugin.RPC
{
    public class RPCManager : PunBehaviour
    {
        public enum ChatSource
        {
            gm = 0,
            player = 1,
            creature = 2,
            anonymous = 999
        }

        public static Dictionary<string, Func<string, string, ChatSource, string>> handlers = new Dictionary<string, Func<string, string, ChatSource, string>>();

        public static RPCManager Instance;
        public static PhotonView View;

        public static void Init()
        {
            if (Instance == null)
            {
                var o = new GameObject();
                var rpc = Instantiate(o);
                Instance = rpc.AddComponent<RPCManager>();
                View = rpc.AddComponent<PhotonView>();
                View.viewID = 606;
            }
        }

        public static void SendChatMessage(string message, NGuid thingThatIsTalking)
        {
            PhotonView photonView = View;
            ChatSource source = ChatSource.anonymous;
            if (LocalPlayer.Id.Value == thingThatIsTalking)
                source = CampaignSessionManager.PlayersInfo[LocalPlayer.Id].Rights.CanGm ?  
                    ChatSource.gm : ChatSource.player;
            foreach (CreatureBoardAsset asset in CreaturePresenter.AllCreatureAssets)
            {
                if (asset.Creature.UniqueId.Value == thingThatIsTalking)
                {
                    source = ChatSource.creature;
                }
            }
            photonView.RPC(nameof(ReceivedMessage), PhotonTargets.All, new object[] { message, thingThatIsTalking.ToString(), JsonConvert.SerializeObject(source) });
        }

        [PunRPC]
        public void ReceivedMessage(string message, string thingThatIsTalking, string chatsource)
        {
            var source = JsonConvert.DeserializeObject<ChatSource>(chatsource);
            foreach (KeyValuePair<string, Func<string, string, ChatSource, string>> handler in handlers)
            {
                if (message.StartsWith(handler.Key))
                {
                    _ = handler.Value(message,thingThatIsTalking,source);
                }
            }
        }
    }
}
