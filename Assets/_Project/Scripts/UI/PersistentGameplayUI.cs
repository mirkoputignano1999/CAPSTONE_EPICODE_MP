using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameplayUI : MonoBehaviour
{
    private static PersistentGameplayUI _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}