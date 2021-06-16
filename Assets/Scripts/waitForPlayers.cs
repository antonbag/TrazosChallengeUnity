using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.TRAZOS.Game;


public class waitForPlayers : MonoBehaviour
{

    Sprite playerSprite;

    Texture2D playerTexture;

    public string texturaFromServer = "";

    public trazosGM trazosGM;

    string dataToQR= "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=";

    string url;

    // Start is called before the first frame update
    void Start()
    {
 
        dataToQR = dataToQR+dataToQR+trazosGM.url+":"+trazosGM.port+"&"+gameObject.tag;
       
        Debug.Log(dataToQR);

        /// * COROUTINE TO GET TEXTURE
        StartCoroutine(GetQR(dataToQR));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator GetQR(string url) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        Debug.Log("Llamando a QR");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            playerTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.Log("ok"); 
           
            playerSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);          
            
            //aplico el sprite
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprite;

        }

    }



}
