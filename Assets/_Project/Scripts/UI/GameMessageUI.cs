using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameMessageUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private float _defaultDuration = 2f;

    private Coroutine _messageCoroutine;

    private void Awake()
    {
        HideImmediate();
    }

    public void ShowMessage(string message)
    {
        ShowMessage(message, _defaultDuration);
    }

    public void ShowMessage(string message, float duration)
    {
        if (_messageCoroutine != null)
        {
            StopCoroutine(_messageCoroutine);
        }

        _messageCoroutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        if (_root != null)
        {
            _root.SetActive(true);
        }

        if (_messageText != null)
        {
            _messageText.text = message;
        }

        yield return new WaitForSecondsRealtime(duration);

        HideImmediate();
        _messageCoroutine = null;
    }

    private void HideImmediate()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }

        if (_messageText != null)
        {
            _messageText.text = string.Empty;
        }
    }
}