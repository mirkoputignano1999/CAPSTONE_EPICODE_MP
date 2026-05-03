using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : InteractableBase
{
    [Header("Lever")]
    [SerializeField] private bool _canBeUsedOnlyOnce = true;
    [SerializeField] private bool _isActivated;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private DoorUnlockState _doorUnlockState;

    [Header("Messages")]
    [SerializeField] private string _activatedMessage = "Si è sentito un rumore metallico, qualcosa si è aperto.";

    public override void Interact(GameObject interactor)
    {
        if (_canBeUsedOnlyOnce && _isActivated)
        {
            return;
        }

        _isActivated = true;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        if (_doorUnlockState != null)
        {
            _doorUnlockState.Unlock();
        }

        if (GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage(_activatedMessage, 2.5f);
        }
    }

    public override string GetInteractionPrompt()
    {
        return _isActivated ? "Leva attivata" : _interactionPrompt;
    }
}