using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffectManager : DescriptionMonoBehavior
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration;

    private Coroutine currentTransitionCoroutine;
    
    public Coroutine TransitionIn()
    {
        fadeImage.SetAlpha(1f);
        StopCurrentTransition();
        currentTransitionCoroutine = StartCoroutine(CoroutTransitionIn());
        return currentTransitionCoroutine;
    }

    private IEnumerator CoroutTransitionIn()
    {
        float timer = 0f;
        while (timer < duration)
        {
            fadeImage.SetAlpha(1f - timer/duration);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        fadeImage.SetAlpha(0f);
    }
    
    public Coroutine TransitionOut()
    {
        fadeImage.SetAlpha(0f);
        StopCurrentTransition();
        currentTransitionCoroutine = StartCoroutine(CoroutTransitionOut());
        return currentTransitionCoroutine;
    }

    private IEnumerator CoroutTransitionOut()
    {
        float timer = 0f;
        while (timer < duration)
        {
            fadeImage.SetAlpha(timer/duration);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        fadeImage.SetAlpha(1f);
    }

    private void StopCurrentTransition()
    {
        if (currentTransitionCoroutine != null)
        {
            StopCoroutine(currentTransitionCoroutine);
        }
    }
}
