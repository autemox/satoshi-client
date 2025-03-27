using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public static DevTools instance { get; private set; }

    [Header("Testing")]
    public bool vChangesCharacterAnimation = false;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

}
