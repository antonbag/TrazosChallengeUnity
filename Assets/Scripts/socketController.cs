using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using UnityEngine;


public class socketController : MonoBehaviour
{
    public string port = "4444";
    public string url = "localhost";
    public string cachePath = "http://localhost/online/Node_WS/cache/";
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


    public GameObject trazos;



    private void Awake()
    {
        
        //Uri u = new Uri("ws://"+url+":"+":"+port);

    }


    void Start()
    {
        address = "ws://"+url+":"+port;
        u = new Uri(address);

        _dataToSend = new TrazosData();

        Connect();







 
       //INSTANTIATE TESTS



















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

        //CANVAS
        if(dataFromServer.component == "canvas"){

            Debug.Log(cachePath+dataFromServer.data+".jpg");
            trazos.GetComponent<imageFromUser>().texturaFromServer = cachePath+dataFromServer.data+".jpg";
            Instantiate(trazos, transform.position, transform.rotation);

        }


        Debug.Log("UNITY log: " + dataFromServer.data);
        
        




        GetServerMessages();
    }




  






}

