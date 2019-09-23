using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shatalmic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class Network : MonoBehaviour
{
    
    public GameState initialGameState;

    private bool dataTransmissionFinished = false; 
    private int previousByteArraySize;
    private Action<Dictionary<CardType, List<string>>> dictionaryCallback;

    private Byte[] receivedByteArray; 
    private Networking networking = null;
    private Text textStatus;
    private bool serverOn;
    private List<Networking.NetworkDevice> connectedDeviceList = null;
    private static Network instance; 
    public static Network Instance
    {
        get 
        {
            return instance;
        }
    }

    private void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            instance = this; 
        }
    }
    
    public void networkInitialGameState(GameState gameState)
    {
        this.initialGameState = gameState;
    }

    void Start()
    {
        if (networking == null)
        {
            networking = GetComponent<Networking>();
            networking.Initialize((error) =>
            {
                BluetoothLEHardwareInterface.Log("Error: " + error);
            }, (message) =>
            {
                if (textStatus != null)
                    textStatus.text = message;

                BluetoothLEHardwareInterface.Log("Message: " + message);
            });
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void StartServer()
    {
        print("starting server");
        serverOn = true;
        connectedDeviceList = null;     
        networking.StartServer("treasure_hunt", DeviceReadyCallBack, DeviceOnDisconnectCallBack, onDeviceDataCallBack);
    }

    void DeviceReadyCallBack(Networking.NetworkDevice connectedDevice)
    {
        print("the device is ready callback");
        
        if (connectedDeviceList == null)
        {
            connectedDeviceList = new List<Networking.NetworkDevice>();
        }

        if (!connectedDeviceList.Contains(connectedDevice))
        {
            connectedDeviceList.Add(connectedDevice);
        }

        foreach(Networking.NetworkDevice device in connectedDeviceList)
        {
            print(device);
        }

        sendWordsAsBytes();
    }

    void DeviceOnDisconnectCallBack(Networking.NetworkDevice disconnectedDevice)
    {
        print("Device disconnected callback: " + disconnectedDevice.Name);

    }

    void onDeviceDataCallBack(Networking.NetworkDevice dataDevice, string deviceCharacteristic, Byte[] bytes)
    {
        print("on data callback, here is the data: " + bytes);

    }

    public void StartClient(Action<Dictionary<CardType, List<string>>> dictionaryCallback)
    {
        print("client started");
        receivedByteArray = new Byte[]{};
        networking.StartClient("treasure_hunt", SystemInfo.deviceUniqueIdentifier, StartedAdvertisingCallBack, ReceivedDataCallBack);
        this.dictionaryCallback = dictionaryCallback;
    }

    void StartedAdvertisingCallBack()
    {
        print("started advertising callback");
    }

    public void ReceivedDataCallBack(string someString, string deviceCharacteristic, Byte[] bytes)
    {
        var byteArrayLength = bytes.Length;

        if(byteArrayLength < previousByteArraySize)
        {
            dataTransmissionFinished = true;
        } 
        else
        {
            dataTransmissionFinished = false;
            previousByteArraySize = byteArrayLength;
        }

        receivedByteArray = receivedByteArray.Concat(bytes).ToArray();

        if(dataTransmissionFinished)
        {
            var dict = returnByteArrayAsDictionary(receivedByteArray);
            dictionaryCallback(dict);
        }     
    }

    public void StopServer()
    {
        networking.StopServer(onStopServerCallback);
    }

    void onStopServerCallback()
    {
        print("Server stopped call back received");
        serverOn = false;
    }

    public void StopClient()
    {
        networking.StopClient(onStopClientCallback); 
        print("client stopped");       
    }

    void onStopClientCallback()
    {
        print("Client stopped call back received");
    }
 
    void onWrittenCallBack()
    {
        print("data written");
    }

    void sendWordsAsBytes()
    {
        dataTransmissionFinished = false;
        var seriablizableDict = returnCardObjectsAsSerializableDictionary(initialGameState.hiddenBoardList);

        print("original dictionary: ");
        foreach(KeyValuePair<CardType, List<string>> entry in seriablizableDict)
        {
            print("key: " + entry.Key);
            foreach(string st in entry.Value)
            {
                print(st);
            }
        }

        var byteArray = returnSerializableDictionaryAsByteArray(seriablizableDict);

        var whatsLeftInTheArray = byteArray.Length;
        for(int i = 0; i <= byteArray.Length; i += 300)
        { 
            var smallArrayLength = 300;

            if (whatsLeftInTheArray < 300)
            {
                smallArrayLength = whatsLeftInTheArray;
            }

            var smallByteArray = new ArraySegment<Byte>(byteArray, i, smallArrayLength).ToArray();

            networking.WriteDevice(connectedDeviceList[0], smallByteArray, onWrittenCallBack);

            whatsLeftInTheArray -= smallArrayLength;
        }            
    }

    Dictionary<CardType, List<string>> returnCardObjectsAsSerializableDictionary(List<CardObject> cards){
        Dictionary<CardType, List<string>> wordDict = new Dictionary<CardType, List<string>>();

        var redWordList = new List<string>();
        var blueWordList = new List<string>();
        var neutralWordList = new List<string>();
        var shipWreckWordList = new List<string>();
        foreach (CardObject card in initialGameState.hiddenBoardList)
        {
            if (card.cardType == CardType.redCard)
            {
                redWordList.Add(card.labelText);
            }
            else if (card.cardType == CardType.blueCard)
            {
                blueWordList.Add(card.labelText);
            }
            else if (card.cardType == CardType.neutralCard)
            {
                neutralWordList.Add(card.labelText);
            }
            else if (card.cardType == CardType.shipwreckCard)
            {
                shipWreckWordList.Add(card.labelText);
            }
        }

        wordDict.Add(CardType.redCard, redWordList);
        wordDict.Add(CardType.blueCard, blueWordList);
        wordDict.Add(CardType.neutralCard, neutralWordList);
        wordDict.Add(CardType.shipwreckCard, shipWreckWordList);
        return wordDict;
    }

    Byte[] returnSerializableDictionaryAsByteArray(Dictionary<CardType, List<string>> dict){
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();

        binFormatter.Serialize(mStream, dict);
        var byteArray = mStream.ToArray();
        return byteArray;
    }

    Dictionary<CardType, List<string>> returnByteArrayAsDictionary(Byte[] byteArray)
    {
        var returnStream = new MemoryStream();
        var returnBinFormatter = new BinaryFormatter();

        returnStream.Write(byteArray, 0, byteArray.Length);
        returnStream.Position = 0;

        Dictionary<CardType, List<string>> newDict = new Dictionary<CardType, List<string>>();
        newDict = returnBinFormatter.Deserialize(returnStream) as Dictionary<CardType, List<string>>;
        return newDict;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
