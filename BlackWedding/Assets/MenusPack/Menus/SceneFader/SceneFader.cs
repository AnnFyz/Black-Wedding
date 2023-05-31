using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }
    public Image img;
    public AnimationCurve curve;
    [SerializeField] float fadeInTime =  1f;
    [SerializeField] float fadeOutTime = 0;
    public bool isCoroutineExecuting = false;
    public float time = 1f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FadeInAgain();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCor());
    }

    public void FadeInAgain()
    {
        StopCoroutine(FadeInCor());
        isCoroutineExecuting = false;
        if (!isCoroutineExecuting)
        {
            img.color = new Color(0, 0, 0, 255);
            fadeInTime = 1f;
            time = 1f;
        }
              StartCoroutine(FadeInCor());
      
    }


    IEnumerator FadeInCor()
    {
        if (!isCoroutineExecuting)
        {
            while (fadeInTime > 0)
            {
                fadeInTime -= Time.unscaledDeltaTime;
                float a = curve.Evaluate(fadeInTime);
                img.color = new Color(0, 0, 0, a);
                yield return 0;
            }
            Debug.Log("FadeInCor");
        }
     
        isCoroutineExecuting = true;
    }



    public void FadeTo(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
        Debug.Log("FadeTo");
    }

  
    public void FadeTo()
    {
        StartCoroutine(FadeOut());
        Debug.Log("FadeTo");
    }

    IEnumerator FadeOut(int sceneIndex)
    {
        while (fadeOutTime < 1)
        {
            fadeOutTime += Time.deltaTime * 2f;
            float a = curve.Evaluate(fadeOutTime);
            img.color = new Color(0, 0, 0, a);
            yield return 0;
        }
        Debug.Log("FadeOut");
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator FadeOut()
    {
        if (!isCoroutineExecuting)
        {
            while (fadeOutTime < 1)
            {
                fadeOutTime += Time.deltaTime * 2f;
                float a = curve.Evaluate(fadeOutTime);
                img.color = new Color(191, 136, 110, a);
                yield return 0;
            }
        }
        Debug.Log("FadeOut");
        isCoroutineExecuting = true;
    }
}
