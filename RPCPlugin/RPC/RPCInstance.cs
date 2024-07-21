using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bounce.ManagedCollections;
using Bounce.Unmanaged;
using Photon;
using Talespire;
using UnityEngine;
using static Mono.Security.X509.X520;

namespace RPCPlugin.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class RPCInstance : PunBehaviour
    {
        internal static Dictionary<string, Func<string, string, SourceRole, string>> Handlers =
            new Dictionary<string, Func<string, string, SourceRole, string>>();

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

        // public void SendMessage(string message, NGuid thingThatIsTalking) => SendMessage(message, thingThatIsTalking);

        public static void SendMessage(string message, NGuid thingThatIsTalking)
        {
            Debug.Log($"message: {message}, thingThatIsTalking: {thingThatIsTalking}");
            var photonView = View;
            SourceRole sourceRole = SourceRole.other;
            
            if (message.Contains("ANONYMOUS", StringComparison.OrdinalIgnoreCase))
            {
                sourceRole = SourceRole.anonymous;
            }
            else if (CampaignSessionManager.PlayersInfo.TryGetValue(new PlayerGuid(thingThatIsTalking), out var playerInfo))
            {
                sourceRole = playerInfo.Rights.CanGm ? SourceRole.gm : SourceRole.player;
            }
            else if (CreaturePresenter.TryGetAsset(new CreatureGuid(thingThatIsTalking), out var c))
            {
                sourceRole = SourceRole.creature;
            }
            else if (hideVolumes.ToArray().Any(hv => hv.HideVolume.Id == thingThatIsTalking))
            {
                sourceRole = SourceRole.hideVolume;
            }

            photonView.RPC(nameof(ReceivedMessage), 
                PhotonTargets.All, 
                new object[] { message, thingThatIsTalking.ToString(), sourceRole }
                );
        }


        [PunRPC]
        public void ReceivedMessage(string message, string thingThatIsTalking, SourceRole chatSource)
        {
            foreach (KeyValuePair<string, Func<string, string, SourceRole, string>> handler in Handlers)
            {
                Debug.Log("RPC Plugin: ParseMessage: Found Handler '" + handler.Key + "'");
                if (message.StartsWith(handler.Key))
                {
                    Debug.Log("RPC: ParseMessage: Applying Handler '" + handler.Key + "'");
                    try
                    {
                        message = handler.Value(message, thingThatIsTalking, chatSource);
                    }
                    catch (Exception x)
                    {
                        Debug.LogWarning("RPC Plugin: ParseMessage: Exception In Handler: " + x.Message);
                        Debug.LogException(x);
                        message = "";
                    }
                    Debug.Log("RPC Plugin: ParseMessage: Post Handler: Title = '" + Convert.ToString(thingThatIsTalking) + "' Message = '" + Convert.ToString(message) + "'");
                    if (message == null) { return; }
                    if (message.Trim() == "") { return; }
                    break;
                }
            }

            /*
            Parallel.ForEach(Handlers, handler =>
            {
                if (message.StartsWith(handler.Key))
                    _ = handler.Value(message, thingThatIsTalking, chatSource);
            });*/
        }

    }
    
}