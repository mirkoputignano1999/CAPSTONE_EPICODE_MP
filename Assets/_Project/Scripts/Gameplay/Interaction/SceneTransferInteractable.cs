using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransferInteractable : InteractableBase
{
    [SerializeField] private string _targetSceneName;

    public override void Interact(GameObject interactor)
    {
        if (string.IsNullOrWhiteSpace(_targetSceneName))
        {
            Debug.LogError($"{name}: Target scene name is missing.");
            return;
        }

        SceneManager.LoadScene(_targetSceneName);
    }
}