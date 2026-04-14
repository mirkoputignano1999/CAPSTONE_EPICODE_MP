using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SceneFlowManager.LoadMainMenu();
    }
}