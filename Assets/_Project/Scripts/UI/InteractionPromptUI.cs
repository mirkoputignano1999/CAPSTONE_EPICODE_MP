using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _promptText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string prompt)
    {
        if (_root != null)
        {
            _root.SetActive(true);
        }

        if (_promptText != null)
        {
            _promptText.text = $"E - {prompt}";
        }
    }

    public void Hide()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }

        if (_promptText != null)
        {
            _promptText.text = string.Empty;
        }
    }
}