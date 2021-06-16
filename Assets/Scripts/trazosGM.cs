using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

namespace Unity.TRAZOS.Game{
public class trazosGM : MonoBehaviour
{

    //[Space]
    //public GameObject gota;
    [Space]
    [Header("=============SERVER=============")]
    public string port = "4444";
    public string url = "localhost";
    public string cachePath = "http://localhost/online/Node_WS/cache/";


[Header("=============PLAYER 1=============")]
    public int score1;
    public GameObject scoreText1;
    public Color colorPlayer1;

    public GameObject goal1;


[Header("=============PLAYER 2=============")]
    public int score2;
    public GameObject scoreText2;
    public Color colorPlayer2;

    public GameObject goal2;

[HideInInspector] public bool ingame = true;
[HideInInspector] public bool startReboot = false;





    void Start()
    {
        score1 = 0;
        score2 = 0;
        
        //Para que el usuario elija el color
        goal1.GetComponent<Image>().color = colorPlayer1;
        goal2.GetComponent<Image>().color = colorPlayer2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore(int player){

        if(!ingame) return;

        if(player == 1){
            score1 +=1;
            scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = score1.ToString();
        }

        if(player == 2){
            score2 +=1;
            scoreText2.GetComponent<TMPro.TextMeshProUGUI>().text = score2.ToString();
        }

    }
}
}
