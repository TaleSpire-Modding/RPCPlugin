using SampleRPCConsumer.Consumer.Messages;
using RPCPlugin.Interfaces;
using UnityEngine;

namespace SampleRPCConsumer.Consumer
{
    [InitOnLoad]
    internal sealed class MyConsumer2 : RpcConsumer<MyMessage2>
    {
        private static readonly MyConsumer2 instance = new MyConsumer2();
        static MyConsumer2() { } // Make sure it's truly lazy

        public static MyConsumer2 Instance { get { return instance; } }

        // My constructor (If I wanted something to happen)
        private MyConsumer2() : base()
        {
        }

        /// <summary>
        /// Event that's triggered once receiving the message that was sent
        /// </summary>
        public override void Handle(MyMessage2 message)
        {
            Debug.Log(message.I);
        }
    }
}
