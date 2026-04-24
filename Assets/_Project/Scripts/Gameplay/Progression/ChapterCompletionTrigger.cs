using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChapterCompletionTrigger : MonoBehaviour
{
    [SerializeField] private ChapterManager _chapterManager;
    [SerializeField] private bool _triggerOnlyOnce = true;

    private bool _hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggerOnlyOnce && _hasTriggered)
        {
            return;
        }

        CharacterBase character = other.GetComponentInParent<CharacterBase>();

        if (character == null)
        {
            return;
        }

        if (_chapterManager == null)
        {
            Debug.LogError($"{name}: ChapterManager reference is missing.");
            return;
        }

        _chapterManager.BeginChapterCompletion();
        _hasTriggered = true;
    }
}