# RPC Plugin

This is a plugin for TaleSpire using BepInEx.

## Install

Currently you need to either follow the build guide down below or use the R2ModMan.

## Usage

Reference this dependency plugin in the parent plugin and then use the following syntax to add a chat
service:

```RPCManager.handlers.Add(serviceKey, handler)```

Where the service key is a string that must appear at the beginning of the rpc message in order to trip the corresponding handler.

Where hander is a function that takes in two string, the message content and the sender, and a source which is a ChatSource enumeration indicating if the source if a GM message, player message or creature message. The handler returns a string for Interface compatibility.

An example of adding a inline handler for "/w" function would be:

``handlers.Add("/w ", (chatMessage, sender, source)=> { Debug.Log(sender+" whispered "+chatMessage); });``

### Usage programatically

``RPCManager.SendChatMessage(message, sender);``

Where *message* is the content to be sent and should include the handling prefix.

Where *sender* is the NGuid of either a creature mini (CreatureId) or player (PlayerId).

For example, to use the above hander for "/doSomething" assuming it is a whisper message to the GM:
``RPCManager.SendChatMessage("/doSomething{\"I'm a\": \"json\"}", LocalPlayer.Id.Value)``

## Compiling
```
* BepInEx.dll  (Download from the BepInEx project.)
* Bouncyrock.TaleSpire.Runtime (found in Steam\steamapps\common\TaleSpire\TaleSpire_Data\Managed)
* UnityEngine.dll
* UnityEngine.CoreModule.dll
* UnityEngine.InputLegacyModule.dll 
* UnityEngine.UI
* Unity.TextMeshPro
* Newtonsoft.Json
* Photon
```

Build the project.

Browse to the newly created ```bin/Debug``` or ```bin/Release``` folders and copy the ```RPCPlugin.dll``` to ```Steam\steamapps\common\TaleSpire\BepInEx\plugins```

## Changelog
- 0.9.0: Alpha release

## Shoutouts
Shoutout to my Patreons on https://www.patreon.com/HolloFox recognising your
mighty contribution to my caffeine addiciton:
- John Fuller
- [Tales Tavern](https://talestavern.com/) - MadWizard