using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AutoLevelTransferTrigger : MonoBehaviour
{
    [Header("Transfer")]
    [SerializeField] private Transform _targetPoint;

    [Header("Fade")]
    [SerializeField] private bool _useFadeTransition = true;

    [Header("Message")]
    [SerializeField] private bool _showMessage;
    [SerializeField] private string _transferMessage = "Ti sposti in un'altra area.";
    [SerializeField] private float _messageDuration = 1.5f;

    [Header("Behavior")]
    [SerializeField] private bool _triggerOnlyOnce;
    [SerializeField] private float _cooldownDuration = 0.5f;

    private bool _hasTriggered;
    private bool _isBusy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isBusy)
        {
            return;
        }

        if (_triggerOnlyOnce && _hasTriggered)
        {
            return;
        }

        CharacterBase character = other.GetComponentInParent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        StartCoroutine(TransferRoutine(character));
    }

    private IEnumerator TransferRoutine(CharacterBase character)
    {
        _isBusy = true;

        if (_triggerOnlyOnce)
        {
            _hasTriggered = true;
        }

        if (_targetPoint == null)
        {
            Debug.LogError($"{name}: Target point is missing.");
            _isBusy = false;
            yield break;
        }

        if (_useFadeTransition && ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOutActionFadeIn(() =>
            {
                character.transform.position = _targetPoint.position;

                CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();

                if (cameraFollow != null)
                {
                    cameraFollow.SnapToTarget();
                }
            });
        }
        else
        {
            character.transform.position = _targetPoint.position;

            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();

            if (cameraFollow != null)
            {
                cameraFollow.SnapToTarget();
            }
        }

        if (_showMessage && GameplayUIManager.Instance != null)
        {
            GameplayUIManager.Instance.ShowMessage(_transferMessage, _messageDuration);
        }

        yield return new WaitForSeconds(_cooldownDuration);
        _isBusy = false;
    }
}