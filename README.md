# RPC Plugin
[![.github/workflows/release.yml](https://github.com/TaleSpire-Modding/RPCPlugin/actions/workflows/release.yml/badge.svg)](https://github.com/TaleSpire-Modding/RPCPlugin/actions/workflows/release.yml)

This is a plugin for TaleSpire using BepInEx, it creates a consumer and service for sending data over RPC. This is designed to be light weight for small messages. If you want large messages then you should host the file externally and only message a pointer to it. This is to reduce the impact on the performance and server costs from sending large messages.

## Install

Currently you need to either follow the build guide down below or use the R2ModMan.

## Usage

Reference this dependency plugin in the parent plugin and then use the following syntax to add a chat
service:

```RPCManager.AddHandler(serviceKey, handler)```

Where the service key is a string that must appear at the beginning of the rpc message in order to trip the corresponding handler.

Where hander is a function that takes in two string, the message content and the sender, and a source which is a ChatSource enumeration indicating if the source if a GM message, player message or creature message. The handler returns a string for Interface compatibility.

An example of adding a inline handler for "/w" function would be:

``RPCManager.AddHandler("/w", (chatMessage, sender, source)=> { Debug.Log(sender+" whispered "+chatMessage); });``

An example of removing a inline handler for "/w" function would be:

``RPCManager.RemoveHandler("/w");``

### Usage programatically

``RPCManager.SendMessage(message, sender);``

Where *message* is the content to be sent and should include the handling prefix.

Where *sender* is the NGuid of either a creature mini (CreatureId) or player (PlayerId).

For example, to use the above hander for "/doSomething" assuming it is a whisper message to the GM:
``RPCManager.SendMessage("/doSomething{\"I'm a\": \"json\"}", LocalPlayer.Id.Value)``


### Usage Consumer and Messenger framework
RPC Manager now supports a Consumer and messenger workflow allowing a singleton 
```CSharp
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
```

## Compiling

Build the project using nuget.

Browse to the newly created ```bin/Debug``` or ```bin/Release``` folders and copy the ```RPCPlugin.dll``` to ```Steam\steamapps\common\TaleSpire\BepInEx\plugins```

## Changelog
- 2.1.2: Sample project now uploaded on gitrepo using message and consumer
- 2.1.1: Message Id Optimization
- 2.1.0: Added Binary message and Consumer workflow for messaging.
- 2.0.1: Fix dependency in pipeline
- 2.0.0: Nuget and re-starting the project
- 1.0.0: Slight Optimization, Official Release
- 0.9.5: Updated depended dll
- 0.9.4: Hide Volume Support and Parallelization
- 0.9.3: Refactor RPCManager to Static class
- 0.9.2: Use SourceRoll.dll and implement static methods
- 0.9.1: remove logging
- 0.9.0: Alpha release

## Shoutouts
Shoutout to my Patreons on https://www.patreon.com/HolloFox recognising your
mighty contribution to my caffeine addiciton:
- John Fuller
- [Tales Tavern](https://talestavern.com/) - MadWizard
