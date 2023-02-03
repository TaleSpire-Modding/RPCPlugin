using System;
using System.Collections.Generic;
using System.Linq;
using RPCPlugin.Interfaces;
using UnityEngine;

namespace SampleRPCConsumer.Consumer
{
    public class MyMessage : RpcMessage
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

    [InitOnLoad]
    public class MyConsumer : RpcConsumer<MyMessage>
    {
        private static readonly MyConsumer instance = new MyConsumer();
        static MyConsumer() { } // Make sure it's truly lazy

        public static MyConsumer Instance { get { return instance; } }

        // My constructor (If I wanted something to happen)
        private MyConsumer(): base ()
        {
        }

        public override void Handle(MyMessage message)
        {
            Debug.Log(message.I);
        }
    }
}
