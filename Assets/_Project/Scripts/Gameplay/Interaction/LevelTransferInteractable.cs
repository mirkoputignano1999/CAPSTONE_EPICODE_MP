using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransferInteractable : InteractableBase
{
    [Header("Transfer")]
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private bool _showMessage = true;
    [SerializeField] private string _transferMessage = "Ti sposti in un'altra area.";

    public override void Interact(GameObject interactor)
    {
        if (_targetPoint == null)
        {
            Debug.LogError($"{name}: Target point is missing.");
            return;
        }

        interactor.transform.position = _targetPoint.position;

        if (_showMessage && GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage(_transferMessage, 1.5f);
        }
    }
}