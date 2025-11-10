using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static event Action<LevelSO> OnFadeFromBlackComplete;
    public static event Action OnFadeToBlackComplete;
    private Image objectToFade;
    private bool fadeToBlack, fadeFromBlack, beginFading;
    private float fadeSpeed = 1f; 
    public void InitiateFade(bool _isFadingIn)
    {
        objectToFade = gameObject.GetComponent<Image>();
        if(_isFadingIn)
        {
            fadeFromBlack = true;
            fadeToBlack = false;
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 1f);
        }
        else
        {
            fadeToBlack = true;
            fadeFromBlack = false;
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 1f);
        }
        StartCoroutine("InitiateFadeDelay");
    }

    IEnumerator InitiateFadeDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        beginFading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!beginFading) return;

        if (fadeFromBlack)
        {
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b,
                Mathf.MoveTowards(objectToFade.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (objectToFade.color.a <= .1f)
            {
                objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 0f);
                OnFadeFromBlackComplete?.Invoke(null);
            }
        }

        if (fadeToBlack)
        {
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b,
                Mathf.MoveTowards(objectToFade.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (objectToFade.color.a == 1f)
            {
                objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 1f);
                OnFadeToBlackComplete?.Invoke();
            }
        }
    }
}
