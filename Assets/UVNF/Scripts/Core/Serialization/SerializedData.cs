﻿using System;
using UnityEngine;

namespace UVNF.Core.Serialization
{
    [Serializable]
    public class SerializedData
    {
        public string Type;
        public string Data;

        public static SerializedData Serialize(object obj)
        {
            var t = new SerializedData()
            {
                Type = obj.GetType().AssemblyQualifiedName,
                Data = JsonUtility.ToJson(obj)
            };
            
            return t;
        }

        public static object Deserialize(SerializedData sd)
        {
            Type objectType = System.Type.GetType(sd.Type);
            return JsonUtility.FromJson(sd.Data, objectType);
        }
    }
}