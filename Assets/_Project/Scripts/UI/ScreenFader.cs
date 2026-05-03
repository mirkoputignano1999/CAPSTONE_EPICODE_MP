using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image _fadeImage;

    [Header("Settings")]
    [SerializeField] private float _defaultFadeOutDuration = 0.25f;
    [SerializeField] private float _defaultBlackHoldDuration = 0.05f;
    [SerializeField] private float _defaultFadeInDuration = 0.25f;

    private Coroutine _fadeCoroutine;
    private bool _isTransitioning;

    public bool IsTransitioning => _isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_fadeImage != null)
        {
            SetAlpha(0f);
        }
    }

    public void FadeOutActionFadeIn(Action midAction)
    {
        FadeOutActionFadeIn(
            midAction,
            _defaultFadeOutDuration,
            _defaultBlackHoldDuration,
            _defaultFadeInDuration);
    }

    public void FadeOutActionFadeIn(
        Action midAction,
        float fadeOutDuration,
        float blackHoldDuration,
        float fadeInDuration)
    {
        if (_fadeImage == null || _isTransitioning)
        {
            midAction?.Invoke();
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(
            FadeOutActionFadeInRoutine(
                midAction,
                fadeOutDuration,
                blackHoldDuration,
                fadeInDuration));
    }

    private IEnumerator FadeOutActionFadeInRoutine(
        Action midAction,
        float fadeOutDuration,
        float blackHoldDuration,
        float fadeInDuration)
    {
        _isTransitioning = true;

        yield return Fade(0f, 1f, fadeOutDuration);

        midAction?.Invoke();

        yield return null;
        yield return new WaitForEndOfFrame();

        if (blackHoldDuration > 0f)
        {
            yield return new WaitForSeconds(blackHoldDuration);
        }

        yield return Fade(1f, 0f, fadeInDuration);

        _isTransitioning = false;
        _fadeCoroutine = null;
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        SetAlpha(fromAlpha);

        if (duration <= 0f)
        {
            SetAlpha(toAlpha);
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(toAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (_fadeImage == null)
        {
            return;
        }

        Color color = _fadeImage.color;
        color.a = alpha;
        _fadeImage.color = color;
    }
}