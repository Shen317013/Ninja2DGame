using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text lifeText, kunaiText, diamondText;

    private void Awake()
    {
        LifeUpdate();
        KunaiUpdate();
        DiamondUpdate();
    }

    public void LifeUpdate()
    {
        lifeText.text = "X" + PlayerPrefs.GetInt("PlayerLife").ToString();
    }

    public void KunaiUpdate()
    {
        kunaiText.text = "X" + PlayerPrefs.GetInt("PlayerKunai").ToString();
    }

    public void DiamondUpdate()
    {
        diamondText.text = "X" + PlayerPrefs.GetInt("PlayerDiamond").ToString();
    }
}
