using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : InteractableBase
{
    private static readonly int OpenTriggerHash = Animator.StringToHash("Open");

    [Header("References")]
    [SerializeField] private DoorUnlockState _doorUnlockState;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _targetPoint;

    [Header("Door Settings")]
    [SerializeField] private bool _isOpen;
    [SerializeField] private float _openDuration = 0.5f;

    [Header("Messages")]
    [SerializeField] private string _closedMessage = "Sembra che sia chiusa.";

    private bool _isBusy;

    public override void Interact(GameObject interactor)
    {
        if (_isBusy)
        {
            return;
        }

        if (_doorUnlockState == null || !_doorUnlockState.IsUnlocked)
        {
            if (GameplayUIManager.Instance != null)
            {
                GameplayUIManager.Instance.ShowMessage(_closedMessage, 2f);
            }

            return;
        }

        if (_isOpen)
        {
            Transfer(interactor);
            return;
        }

        StartCoroutine(OpenAndTransferRoutine(interactor));
    }

    private IEnumerator OpenAndTransferRoutine(GameObject interactor)
    {
        _isBusy = true;

        if (_animator != null)
        {
            _animator.SetTrigger(OpenTriggerHash);
        }

        yield return new WaitForSeconds(_openDuration);

        _isOpen = true;
        _isBusy = false;

        Transfer(interactor);
    }

    private void Transfer(GameObject interactor)
    {
        if (_targetPoint == null)
        {
            Debug.LogError($"{name}: Target point is missing.");
            return;
        }

        CharacterBase character = interactor.GetComponentInParent<CharacterBase>();

        if (character != null)
        {
            character.transform.position = _targetPoint.position;
            return;
        }

        interactor.transform.position = _targetPoint.position;
    }
}