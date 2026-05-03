using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineInteractable : InteractableBase
{
    [TextArea]
    [SerializeField] private string _message = "Non c'è nulla di interessante.";

    public override void Interact(GameObject interactor)
    {
        if (GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage(_message, 3f);
        }
        else
        {
            Debug.Log(_message);
        }
    }
}
