using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotaScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Start is called before the first frame update
    void Start()
    {

        gameObject.transform.Translate(0.0f, 5.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
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
}
