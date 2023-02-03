using System;
using System.Collections.Generic;
using System.Linq;
using RPCPlugin.Interfaces;
using UnityEngine;

namespace SampleRPCConsumer.Consumer
{
    public class MyMessage2 : RpcMessage
    {
        // Example Data
        internal int I { get; set; }

        public MyMessage2()
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
        public MyMessage2(byte[] data)
        {
            I = BitConverter.ToInt32(data, 0);
        }
    }

    [InitOnLoad]
    public class MyConsumer2 : RpcConsumer<MyMessage2>
    {
        private static readonly MyConsumer2 instance = new MyConsumer2();
        static MyConsumer2() { } // Make sure it's truly lazy

        public static MyConsumer2 Instance { get { return instance; } }

        // My constructor (If I wanted something to happen)
        private MyConsumer2(): base ()
        {
        }

        public override void Handle(MyMessage2 message)
        {
            Debug.Log(message.I);
        }
    }
}
