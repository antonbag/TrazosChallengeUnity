using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using UnityEngine;
using System.Globalization;
using Unity.TRAZOS.Game;

public class socketController : MonoBehaviour
{
    trazosGM trazosGM;

    string port,url,cachePath;


    string address;
     Uri u;
    ClientWebSocket cws = null;
    ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);


    [Serializable]
    public class TrazosData
    {
        public string from;
        public int player;
        public string component;
        public string task;
        public string data;
        public string extra;
    }

    TrazosData _dataToSend;
    TrazosData _dataReceive;

    GameObject goal1;
    GameObject goal2;
    public GameObject trazos;

    GameObject lastTrazoPlayer1;
    GameObject lastTrazoPlayer2;

    //****************************
    //* GYRO VARIABLES
    //****************************
    bool gyro2 = false;
    float gyroForce2 = 1.0f;


    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 1.0f;


    private void Awake()
    {
        
        //Uri u = new Uri("ws://"+url+":"+":"+port);

    }


    void Start()
    {

        trazosGM = FindObjectOfType<trazosGM>();

        //data from GMs
        port = trazosGM.port;
        url = trazosGM.url;
        cachePath = trazosGM.cachePath;


        address = "ws://"+url+":"+port;
        u = new Uri(address);

        _dataToSend = new TrazosData();

        Connect();

        goal1 = GameObject.Find("goal1");
        goal2 = GameObject.Find("goal2");

    }



    async void Connect() {

        cws = new ClientWebSocket();

        try {
            await cws.ConnectAsync(u, CancellationToken.None);
            if (cws.State == WebSocketState.Open) Debug.Log("...connected!");

            //HELLO
            _dataToSend.from = "unity";
            _dataToSend.component = "unityHello";
            _dataToSend.task = "justSayHelloTo";
            _dataToSend.data = "";
            _dataToSend.player = 0;

            SendMessage(_dataToSend);


            //inicializo la recepcion de mensajes
            GetServerMessages();
        }
        catch (Exception e) { 
            Debug.Log("HEOUW " + e.Message); 
            
        }
    }






    async void SendMessage(TrazosData _dataToSend) {

        //dejo toda esta basura por si os interesa ver las aproximaciones que he hecho

        //quien
        //mensaje
        //data 
/*         List<string> msg = new List<string>() {
            "unity",
            "holi, soy",
            "UNITY",
        }; */

         //byte[] result = msg.SelectMany(x => u8.GetBytes(x+';')).ToArray();
       // byte[] result = JsonUtility.ToJson(_dataToSend);

        //string s = u8.GetString(JsonUtility.ToJson(_dataToSend).ToCharArray());

        //string s = u8.GetString(bytes).ToArray();



        Encoding  u8 = Encoding.UTF8;
        var bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(_dataToSend));
        
        Debug.Log(JsonUtility.ToJson(_dataToSend)); 
       
        
        //CODIFICO EN SEGMENTOS Y ENVÍO
        ArraySegment<byte> envio = new ArraySegment<byte>(bytes);
        await cws.SendAsync(envio, WebSocketMessageType.Text, true, CancellationToken.None);

    }







    async void GetServerMessages() {

        WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);

        Debug.Log("UNITY log: " + Encoding.UTF8.GetString(buf.Array, 0, r.Count));

        var dataFromServer = JsonUtility.FromJson<TrazosData>(Encoding.UTF8.GetString(buf.Array, 0, r.Count));

            
            
        //startQR
        if(dataFromServer.component == "server"){

            if(dataFromServer.task == "reboot"){
                if(dataFromServer.player == 1){
                    GameObject.Find("QR1").GetComponent<SpriteRenderer>().enabled = true;
                }

                if(dataFromServer.player == 2){
                GameObject.Find("QR2").GetComponent<SpriteRenderer>().enabled = true;
                }
            }

        }




        //QR
        if(dataFromServer.component == "qr"){

            Debug.Log(dataFromServer.ToString());

            if(dataFromServer.player == 1){
                GameObject.Find("QR1").GetComponent<SpriteRenderer>().enabled = false;
            }

            if(dataFromServer.player == 2){
               GameObject.Find("QR2").GetComponent<SpriteRenderer>().enabled = false;

            }

        }




        //CANVAS
        if(dataFromServer.component == "canvas"){

            Debug.Log(cachePath+dataFromServer.data+".jpg");



            if(dataFromServer.player == 1){
                trazos.GetComponent<imageFromUser>().texturaFromServer = cachePath+dataFromServer.data+".jpg";
                lastTrazoPlayer1 = Instantiate(trazos, transform.position, transform.rotation);
            }

            if(dataFromServer.player == 2){

                Debug.Log("transform.position:"+transform.position.ToString());
                trazos.GetComponent<imageFromUser>().texturaFromServer = cachePath+dataFromServer.data+".jpg";
                lastTrazoPlayer2 = Instantiate(trazos, new Vector3(6f,5.5f,0), transform.rotation);
            }

        }

        //GYRO
        if(dataFromServer.component == "gyro"){

            /*
            Debug.Log("GYRO");
            Debug.Log(float.Parse(dataFromServer.data));
            Debug.Log(float.Parse(dataFromServer.data));
            */

            if(dataFromServer.player == 1 && lastTrazoPlayer1 != null){

                //elimino inconsistencias
                if(float.Parse(dataFromServer.data) <= 2){
                    float gyroRotate = float.Parse(dataFromServer.data);
                    Debug.Log(float.Parse(dataFromServer.data));
                    lastTrazoPlayer1.GetComponent<Transform>().Rotate(0,0,gyroRotate,Space.Self);
                }
            }

            if(dataFromServer.player == 2 && lastTrazoPlayer2 != null){
                gyro2 = true;
                if(float.Parse(dataFromServer.data) <= 10){
                    gyroForce2 = float.Parse(dataFromServer.data, CultureInfo.InvariantCulture.NumberFormat);
                    //gyroForce2 = (float)double.Parse(dataFromServer.data,System.Globalization.NumberStyles.AllowDecimalPoint);
                    Debug.Log("dataFromServer.data:"+float.Parse(dataFromServer.data).ToString());
       
                    //lastTrazoPlayer2.GetComponent<Transform>().Rotate(0,0,gyroRotate,Space.Self);
                }

            }
               
        }


        Debug.Log("UNITY log: " + dataFromServer.data);
        Debug.Log(gyroForce2);
        

        GetServerMessages();
    }




  



    void Update() {
        
        /*
        float singleStep = speed * Time.deltaTime;

        if(lastTrazoPlayer2){
            Vector3 gyro2v3 = new Vector3(gyroRotate2,0,0);
            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(lastTrazoPlayer2.GetComponent<Transform>().forward, gyro2v3, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            lastTrazoPlayer2.GetComponent<Transform>().rotation = Quaternion.LookRotation(newDirection);
        }
        */
/*      
        float singleStep = 1f * Time.deltaTime;
        Vector3 currentRot = goal2.transform.eulerAngles;
        Vector3 gyro2v3 = new Vector3(0,0,gyroForce2);
        Vector3 newDirection = Vector3.RotateTowards(currentRot, gyro2v3, singleStep, 0.0f);
        goal2.transform.rotation = Quaternion.LookRotation(gyro2v3);
 */
        Vector3 gyro2v3 = new Vector3(0,0,gyroForce2);

  
        float singleStep = 1f * Time.deltaTime;
        //goal2.transform.rotation = Quaternion.Euler(0, 0, gyroForce2*180.0f / Mathf.PI + singleStep);
        goal2.transform.localRotation = Quaternion.Euler(0, 0, gyroForce2);
       
        //Debug.Log(gyroForce2);
    }

        void FixedUpdate()
    {
        if(lastTrazoPlayer2){

            
            // Alternatively, specify the force mode, which is ForceMode2D.Force by default
            //lastTrazoPlayer2.GetComponent<Rigidbody2D>().AddForce(transform.up * gyroForce2, ForceMode2D.Impulse);
        }
    }


}

