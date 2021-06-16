using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class imageFromUser : MonoBehaviour
{

    Sprite playerSprite;

    Texture2D playerTexture;

    public string texturaFromServer = "http://localhost/online/linux.png";


    // Start is called before the first frame update
    void Awake() {
    }
       
    void Start()
    {
       
        /// * COROUTINE TO GET TEXTURE
        StartCoroutine(GetTexture(texturaFromServer));
     
    }


 
    IEnumerator GetTexture(string url) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            playerTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.Log("ok"); 
           
            playerSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);          
            
            cambiaSprite();
            //cambiaCanvas();
        }
    }

    
    /// * cambio el sprite y genero las colisiones
    void cambiaSprite(){

        //aplico el sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = playerSprite;

        //agrego el collider
        gameObject.AddComponent<PolygonCollider2D>();

        //Posiciono el sprite fuera de campo (seguridad)
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,10.0f,0);

        //Roto la imagen (test)
        gameObject.transform.rotation = Quaternion.Euler(0,0, Random.Range(0,90));

        //Modifico el rigibody para que sea dinámico
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

    }


    // Update is called once per frame
    void Update()
    {   

        //TEST
        if(Time.fixedTime >= 5.0f && Time.fixedTime <= 6.0f){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,10.0f,0);
        } 
        
    }
}
