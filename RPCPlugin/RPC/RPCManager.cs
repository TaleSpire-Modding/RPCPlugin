using System;
using Bounce.Unmanaged;
using Talespire;

namespace RPCPlugin.RPC
{
    public static class RPCManager
    {
        public static void AddHandler(string key, Func<string, string, SourceRole, string> callback)
            => RPCInstance.Handlers.Add(key,callback);

        public static void RemoveHandler(string key)
            => RPCInstance.Handlers.Remove(key);

        public static void SendMessage(string message, NGuid source)
        {
            RPCPlugin.InternalLogger.LogDebug("RPCManager: Sending " + message + " (signature " + source.ToString() + ")");
            
            RPCInstance.SendMessage(message, source);
        }
        //=> RPCInstance.SendMessage(message, thingThatIsTalking);
    }
}
