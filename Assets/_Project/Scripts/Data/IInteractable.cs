using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt();
    void Interact(GameObject interactor);
}
