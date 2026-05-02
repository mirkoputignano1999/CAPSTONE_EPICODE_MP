using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDebugWatcher : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"{name} ENABLED");
    }

    private void OnDisable()
    {
        Debug.Log($"{name} DISABLED");
        Debug.Log(System.Environment.StackTrace);
    }
}