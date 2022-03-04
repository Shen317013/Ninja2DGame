using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            string LevelName = SceneManager.GetActiveScene().name;
            string temp = LevelName.Substring(5);
            int LevelNumber = int.Parse(temp);

            int clearedLevel = PlayerPrefs.GetInt("clearedLevel");

            if (LevelNumber > clearedLevel)
            {
                PlayerPrefs.SetInt("clearedLevel", LevelNumber);
            }

            //SceneManager.LoadScene("LevelChange");
            Time.timeScale = 0f;
            FadeInOut.instance.ScenceFadeInOut("LevelChange");
        }
    }
}
