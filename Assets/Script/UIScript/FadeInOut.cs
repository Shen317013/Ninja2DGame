using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;
    public GameObject fadeInOutImage;
    public Animator myAnim;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ScenceFadeInOut(string LevelName)
    {
        StartCoroutine(SceneFadeInOut2(LevelName));
    }

    IEnumerator SceneFadeInOut2(string LevelName)
    {
        fadeInOutImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(LevelName);
        myAnim.Play("FadeOut");

        yield return new WaitForSecondsRealtime(1.0f);
        fadeInOutImage.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
