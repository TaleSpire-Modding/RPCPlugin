﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using RPCPlugin.RPC;
using UnityEngine;
using MD5 = System.Security.Cryptography.MD5;

namespace RPCPlugin.Interfaces
{
    public abstract class RpcConsumer<T> : IBinaryFactory where T : RpcMessage, new()
    {
        public abstract void Handle(T message);

        public void Handle(byte[] data)
        {
            var message = (T)Activator.CreateInstance(typeof(T), data);
            Handle(message);
        }

        private static readonly Lazy<RpcConsumer<T>> Lazy =
            new Lazy<RpcConsumer<T>>(() => (Activator.CreateInstance(typeof(RpcConsumer<T>), true) as RpcConsumer<T>)!);

        public static RpcConsumer<T> Instance => Lazy.Value;

        public virtual void SendMessage(T message, PhotonTargets targets = PhotonTargets.All)
        {
            RPCInstance.SendMessage(message, targets);
        }

        protected RpcConsumer()
        {
            var assembly = typeof(T).AssemblyQualifiedName;
            var key = new Guid(RpcMessage.CreateMD5(assembly));
            RPCInstance.Binaries.Add(key, this);
            Debug.Log($"Registered assembly: {key} {assembly}");
        }
    }

    public abstract class RpcMessage : IBinaryMessage
    {
        public abstract byte[] Value();

        public Guid Id()
        {
            var assembly = GetType().AssemblyQualifiedName;
            return new Guid(CreateMD5(assembly));
        }

        public static byte[] CreateMD5(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return hashBytes;
        }
    }

    public interface IBinaryMessage
    {
        public byte[] Value();
    }

    public interface IBinaryFactory
    {
        public void Handle(byte[] message);
    }

    public class InitOnLoad : Attribute
    {
        public static void Initialise()
        {
            // get a list of types which are marked with the InitOnLoad attribute
            var types =
                from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                where t.GetCustomAttributes(typeof(InitOnLoad), false).Any()
                select t;

            // process each type to force initialise it
            foreach (var type in types)
            {
                // try to find a static field which is of the same type as the declaring class
                var field = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(f => f.FieldType == type);
                // evaluate the static field if found
                if (field != null) field.GetValue(null);
            }
        }
    }
}
