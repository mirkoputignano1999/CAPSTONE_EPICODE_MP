using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] protected string _interactionPrompt = "Interagisci";

    public virtual string GetInteractionPrompt()
    {
        return _interactionPrompt;
    }

    public abstract void Interact(GameObject interactor);
}