using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class imageFromUser : MonoBehaviour
{

    Sprite playerSprite;

    Texture2D playerTexture;

    public string texturaFromServer = "";

    // Tiempo para destruir
    public float timeToDie = 50.0f;


    //*SCALE
    // cada cuanto se hace pequeño
    public float scaleEverySeconds = 10.0f;
    private float currentScaleEverySeconds;
    private bool mustScale = false;

    // factor de escala
    public float scaleFactor = 1f;

    // vector de escala
    private Vector3 scaleChange;
    private float currentScaleValue;
       
    void Start()
    {
       
        timeToDie = Random.Range(timeToDie-20f, timeToDie+20f);

        //SCALE
        currentScaleEverySeconds = scaleEverySeconds;
        scaleChange = new Vector3(scaleFactor/100, scaleFactor/100, scaleFactor/100);
        currentScaleValue = 0.9f;

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


        //DESTROY OBJECT AFTER random
        Destroy(gameObject, Random.Range(timeToDie-20f, timeToDie+20f));

    }


    // Update is called once per frame
    void Update()
    {   

        /*//TEST
        if(Time.fixedTime >= 5.0f && Time.fixedTime <= 6.0f){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,10.0f,0);
        }
        */

        /////////////
        //SCALE
        /////////////
        currentScaleEverySeconds -= Time.deltaTime;
        if(currentScaleEverySeconds <= 0){
            //reset contador
            //scalo object
            mustScale = true;
            currentScaleValue -= 0.1f;
            currentScaleEverySeconds = scaleEverySeconds;
        }
        scaleObject();



        //DESTROY ON EXIT SCREEN
        if(gameObject.transform.position.y <= -50){
            Destroy(gameObject);
        }



        //PARECE MÁS ESTABLE PERO MENOS ELEGANTE
        /*        
                if(timeToDie > 0){
                    timeToDie -= Time.deltaTime;
                    Debug.Log(timeToDie);
                }else{
                    Debug.Log("Destroy!!!");
                    Destroy(gameObject);
                } 

        */
        
    }


    void scaleObject(){
        
        //escala cada x
        if(mustScale && (transform.localScale.x >= currentScaleValue) ){
            transform.localScale -= scaleChange;
        }else{
            currentScaleValue = transform.localScale.x;
            mustScale=false;
        }

        //
        if(transform.localScale.x <= 0.4f){
            //go to destroy
            transform.localScale -= scaleChange;
        }

        if(transform.localScale.x <= 0.01f){
            //go to destroy
            Destroy(gameObject);
        }
        
    }

}
