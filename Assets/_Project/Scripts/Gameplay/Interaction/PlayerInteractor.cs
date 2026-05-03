using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;

    private readonly List<IInteractable> _nearbyInteractables = new();

    private void Awake()
    {
        if (_inputHandler == null)
        {
            Debug.LogError($"{name}: PlayerInputHandler reference is missing.");
        }
    }

    private void Update()
    {
        if (_inputHandler == null)
        {
            return;
        }

        if (_inputHandler.InteractPressedThisFrame)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        IInteractable interactable = GetClosestInteractable();

        if (interactable == null)
        {
            return;
        }

        GameObject playerRoot = transform.root.gameObject;
        interactable.Interact(playerRoot);
    }

    private IInteractable GetClosestInteractable()
    {
        _nearbyInteractables.RemoveAll(item => item == null);

        if (_nearbyInteractables.Count == 0)
        {
            return null;
        }

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (IInteractable interactable in _nearbyInteractables)
        {
            if (interactable is not MonoBehaviour interactableBehaviour)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, interactableBehaviour.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null)
        {
            return;
        }

        if (!_nearbyInteractables.Contains(interactable))
        {
            _nearbyInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null)
        {
            return;
        }

        _nearbyInteractables.Remove(interactable);
    }
}