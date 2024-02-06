using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{

    public new List<TKey> Keys = new List<TKey>();
    public new List<TValue> Values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        Keys.Clear();
        Values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            Keys.Add(pair.Key);
            Values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (Keys.Count != Values.Count)
        {
            Debug.LogError($"Tried to deserialize a SerializableDictionary, but the amount of keys ({Keys.Count}) does not match the number of values ({Values.Count}) which indicates that something went wrong");
        }

        for (int i = 0; i < Keys.Count; i++)
        {
            this.Add(Keys[i], Values[i]);
        }
    }
}
