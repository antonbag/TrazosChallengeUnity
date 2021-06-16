using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Unity.TRAZOS.Game;
public class gotaScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public trazosGM trazosGM;



    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Start is called before the first frame update
    void Start()
    {

        gameObject.transform.Translate(0.0f, 5.0f, 0.0f);
//rb.bodyType = RigidbodyType2D.Static;

       
    }

    // Update is called once per frame
    void Update()
    {

        //REINICIO
        if(trazosGM.ingame == false && trazosGM.startReboot){
            rb.bodyType = RigidbodyType2D.Static;
            float step =  10.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new UnityEngine.Vector3(0,5.5f,0), step);
        }


        //BOTTOM
        if(transform.position.y <= -10.0){
            transform.position = new UnityEngine.Vector3(0,5.5f,0);
            rb.velocity = new Vector2(0.0f, 2.0f);
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = -0f;
        }
        
/*         if(gameObject.GetComponent<Rigidbody2D>().gravityScale <=1.0f){
            gameObject.GetComponent<Rigidbody2D>().gravityScale += 0.001f;
        } */

        //y si se sale de la pantalla?
    }

    void OnCollisionEnter2D(Collision2D other) {
        
        Debug.Log(other.gameObject.tag);



        if(other.gameObject.tag == "goal1" || other.gameObject.tag == "goal2"){

            if(other.gameObject.tag == "goal1"){
                trazosGM.updateScore(1);
                gameObject.GetComponent<SpriteRenderer>().color  = Color.green;
            }

            if(other.gameObject.tag == "goal2"){
                trazosGM.updateScore(2);
                Debug.Log(trazosGM.colorPlayer2);
                gameObject.GetComponent<SpriteRenderer>().color = trazosGM.colorPlayer2;
            }

            trazosGM.ingame = false;
            
            //inicio y 
            StartCoroutine(waitGame(3));
        }

     
    }

    //paro el juego
    IEnumerator waitGame(int segundos)
    {

        //cambio de color la esfera según el jugador
        trazosGM.ingame = false;

        //espero 3 segundos y reinicio
        yield return new WaitForSeconds(segundos);


/*         
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<SpriteRenderer>().color  = Color.white; */
        StartCoroutine(waitAndStart(3));
        //StopCoroutine(waitAndStart(segundos));
    }



    IEnumerator waitAndStart(int segundos)
    {

        trazosGM.startReboot = true;


        //espero 3 segundos y reinicio
        yield return new WaitForSeconds(segundos);


        trazosGM.ingame = true;
        trazosGM.startReboot = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<SpriteRenderer>().color  = Color.white;
        StopCoroutine(waitGame(segundos));
        StopCoroutine(waitAndStart(segundos));
    }

}
