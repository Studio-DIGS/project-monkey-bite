using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffectManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration;
    
    public Coroutine TransitionIn()
    {
        return StartCoroutine(CoroutTransitionIn());
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
        return StartCoroutine(CoroutTransitionOut());
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
}
