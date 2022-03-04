using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject selectPanel , stopButton , levelSelectButton , mainMenuButton , replayButton;

    public void ReplayButton()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        FadeInOut.instance.ScenceFadeInOut(sceneName);
    }

    public void MainMenuButton()
    {
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        FadeInOut.instance.ScenceFadeInOut("MainMenu");
    }

    public void LevelSelectButton()
    {
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        FadeInOut.instance.ScenceFadeInOut("LevelChange");
    }

    public void NoButton()
    {
        RectTransform DataDeleteImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleteImage.anchoredPosition = new Vector2(0f, 1500f);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    public void YesButton()
    {
        PlayerPrefs.DeleteAll();
        IsFirstTimePlayerCheck checkScript = GameObject.Find("IsFirstTimePlayerCheck").GetComponent<IsFirstTimePlayerCheck>();
        checkScript.FirstTimePlayState();
        RectTransform DataDeleteImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleteImage.anchoredPosition = new Vector2(0f, 1500f);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    public void DataDeleteButton()
    {
        RectTransform DataDeleteImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleteImage.anchoredPosition = new Vector2(0f , -100f);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    //開啟暫停選單
    public void setSelectPanelOn()
    {
        selectPanel.SetActive(true);

        //暫停遊戲
        Time.timeScale = 0f;
    }

    //關閉暫停選單
    public void setSelectPanelOff()
    {
        selectPanel.SetActive(false);

        //繼續遊戲
        Time.timeScale = 1.0f;
    }

    //按下暫停按鈕 原本的鈕會消失
    public void SetStopButtonOn()
    {
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        stopButton.SetActive(true);
    }

    //按下關閉按鈕 原本的鈕會回來
    public void SetStopButtonOff()
    {
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        stopButton.SetActive(false);
    }


    
    public void MainMenuPlayButton()
    {
        //首頁玩家會往右跑
        GameObject mainMenuPlayer = GameObject.Find("MainMenuPlayer");
        Animator myAnim = mainMenuPlayer.GetComponent<Animator>();
        myAnim.SetBool("Run" , true);

        //點擊完後,消失play按鈕
        GameObject playButton = GameObject.Find("Canvas/SafeAreaPanel/PlayButton");
        playButton.SetActive(false);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    
        //SceneManager.LoadScene("LevelChange");
        FadeInOut.instance.ScenceFadeInOut("LevelChange");
    
    }
}
