using SampleRPCConsumer.Consumer.Messages;
using RPCPlugin.Interfaces;
using UnityEngine;

namespace SampleRPCConsumer.Consumer
{
    [InitOnLoad]
    internal sealed class MyConsumer : RpcConsumer<MyMessage>
    {
        private static readonly MyConsumer instance = new MyConsumer();
        static MyConsumer() { } // Make sure it's truly lazy

        public static MyConsumer Instance { get { return instance; } }

        // My constructor (If I wanted something to happen)
        private MyConsumer() : base()
        {
        }

        /// <summary>
        /// Event that's triggered once receiving the message that was sent
        /// </summary>
        public override void Handle(MyMessage message)
        {
            Debug.Log(message.I);
        }
    }
}
