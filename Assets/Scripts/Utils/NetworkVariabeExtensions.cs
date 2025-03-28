using Unity.Netcode;
using UnityEngine;

public static class NetworkVariableExtensions
{
    public static void SetString(this NetworkVariable<Unity.Collections.FixedString128Bytes> variable, string value)
    {
        if (variable != null)
        {
            Debug.Log("Setting NetworkVariable to: " + value);
            variable.Value = new Unity.Collections.FixedString128Bytes(value);
            Debug.Log("NetworkVariable now contains: " + variable.Value.ToString());
        }
        else
        {
            Debug.LogError("NetworkVariable is null!");
        }
    }

    public static string GetString(this NetworkVariable<Unity.Collections.FixedString128Bytes> variable)
    {
        if (variable != null)
        {
            return variable.Value.ToString();
        }
        else
        {
            Debug.LogError("NetworkVariable is null!");
            return "";
        }
    }
}