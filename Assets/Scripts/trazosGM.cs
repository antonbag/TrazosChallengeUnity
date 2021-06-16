using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trazosGM : MonoBehaviour
{

    public GameObject gota;

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
            //gota.GetComponent<Image>().color = colorPlayer1;
        }

        if(player == 2){
            score2 +=1;
            scoreText2.GetComponent<TMPro.TextMeshProUGUI>().text = score2.ToString();
            //gota.GetComponent<Image>().color = colorPlayer2;
        }

    }
}
