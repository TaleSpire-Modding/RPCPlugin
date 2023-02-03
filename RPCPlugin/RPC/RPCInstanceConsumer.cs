using Photon;
using RPCPlugin.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPCPlugin.RPC
{
    /// <summary>
    /// RpcInstance, Consumer implementation 
    /// </summary>
    public sealed partial class RPCInstance : PunBehaviour
    {
        /// <summary>
        /// Tracking consumers to unique id
        /// </summary>
        internal static Dictionary<Guid, IBinaryFactory> Binaries = new Dictionary<Guid, IBinaryFactory>();

        /// <summary>
        /// Track assembly to byte[] mapping for hash caching
        /// </summary>
        internal static Dictionary<string, byte[]> Ids = new Dictionary<string, byte[]>();

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
