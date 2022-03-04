using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemKunai : MonoBehaviour
{
    Player myPlayer;
    Canvas myCanvas;

    private void Awake()
    {
        myPlayer = GameObject.Find("Player").GetComponent<Player>();

        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //當玩家碰到物件,飛鏢+1
        if (collision.name == "Player")
        {
            int kunai = PlayerPrefs.GetInt("PlayerKunai")+ 1;
            PlayerPrefs.SetInt("PlayerKunai", kunai);
            myPlayer.playerKunai = kunai;
            myCanvas.KunaiUpdate();
            Destroy(this.gameObject);
        }
    }
}
