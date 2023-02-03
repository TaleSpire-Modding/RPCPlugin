using System;
using System.Collections.Generic;
using System.Linq;
using RPCPlugin.Interfaces;

namespace SampleRPCConsumer.Consumer.Messages
{
    internal sealed class MyMessage : RpcMessage
    {
        // Example Data
        internal int I { get; set; }

        public MyMessage()
        {

        }

        // Serialize to Binary
        public override byte[] Value()
        {
            IEnumerable<byte> bytes = new List<byte>();

            bytes = bytes.Concat(BitConverter.GetBytes(I));

            return bytes.ToArray();
        }

        // Construct from Binary
        public MyMessage(byte[] data)
        {
            I = BitConverter.ToInt32(data, 0);
        }
    }
}
