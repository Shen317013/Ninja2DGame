using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDiamond : MonoBehaviour
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
        //當玩家碰到物件,鑽石+1
        if (collision.name == "Player")
        {
            int diamond = PlayerPrefs.GetInt("PlayerDiamond") + 1;
            PlayerPrefs.SetInt("PlayerDiamond", diamond);
            myCanvas.DiamondUpdate();
            Destroy(this.gameObject);
        }
    }
}
