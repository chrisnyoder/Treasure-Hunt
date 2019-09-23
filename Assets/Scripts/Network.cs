using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shatalmic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Network : MonoBehaviour
{
    
    public GameState initialGameState;
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

    public void StartServer(){
        print("starting server");
        serverOn = true;
        connectedDeviceList = null;     
        networking.StartServer("treasure_hunt", DeviceReadyCallBack, DeviceOnDisconnectCallBack, onDeviceDataCallBack);
    }

    void DeviceReadyCallBack(Networking.NetworkDevice connectedDevice){
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

    public void StartClient()
    {
        print("client started");
        networking.StartClient("treasure_hunt", SystemInfo.deviceUniqueIdentifier, StartedAdvertisingCallBack, CharacteristicWrittentCallback);
    }

    void StartedAdvertisingCallBack()
    {
        print("started advertising callback");
    }

    void CharacteristicWrittentCallback(string someString, string deviceCharacteristic, Byte[] bytes)
    {
        print("characteristic written callback, here is the data: ");
        foreach(Byte bt in bytes)
        {
            print(bt);
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

        print("hidden board list size at time of networking");
        print(initialGameState.hiddenBoardList.Count);
        // var seriablizableDict = returnCardObjectsAsSerializableDictionary(initialGameState.hiddenBoardList);
        // var byteArray = returnSerializableDictionaryAsByteArray(seriablizableDict);

        var bitArray = new Byte[] {0, 1, 2, 3, 4};

        networking.WriteDevice(connectedDeviceList[0], bitArray, onWrittenCallBack);
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
