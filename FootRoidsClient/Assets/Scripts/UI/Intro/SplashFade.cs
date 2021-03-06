﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFade : MonoBehaviour
{
    public Image splashImage;
    public Text splashText;
    public string loadLevel;

    IEnumerator Start()
    {
        // Fades image and text
        splashImage.canvasRenderer.SetAlpha(0.0f);
        splashText.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(4.5f);
        FadeOut();
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(loadLevel);
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, 3.0f, false);
        splashText.CrossFadeAlpha(1.0f, 3.0f, false);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, 3.0f, false);
        splashText.CrossFadeAlpha(0.0f, 3.0f, false);
    }


}
