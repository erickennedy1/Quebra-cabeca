using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
public CanvasGroup fadeCanvasGroup; 
    public float fadeDuration = 1f; 

    private void Start()
    {
        StartCoroutine(FadeIn()); 
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName)); 
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.blocksRaycasts = true; 
        fadeCanvasGroup.alpha = 0; 
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration; 
            yield return null;
        }
    }

    private IEnumerator FadeOut(string sceneName)
    {
        fadeCanvasGroup.alpha = 1; 
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration; 
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;

        SceneManager.LoadScene(sceneName); 
    }
}
