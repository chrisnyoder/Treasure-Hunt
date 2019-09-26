using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shatalmic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;

public class Network : MonoBehaviour
{
    public GameState initialGameState;

    private Action<Dictionary<CardType, List<string>>> DictionaryReceivedCallback;
    private Action<Dictionary<string, bool>> WordsSelectedCallback;
    private Action<CurrentGameState> GameStateChangedCallback;
    private Action<string> languageChangedCallBack;

    private Byte[] byteArrayToSend;
    private Byte[] receivedByteArray; 

    private Text textStatus;
    private bool isServer = true;

    public static Networking networking;
   
    private bool dictionaryReadyToBeSent = false; 

    private List<Networking.NetworkDevice> connectedDeviceList = null;
    private List<Networking.NetworkDevice> devicesDataSentTo = null; 
    
    // private void Awake()  
    // {
    //     if(NetworkManagerObjectInstance == null) 
    //     {
    //         NetworkManagerObjectInstance = new List<GameObject>();
    //         NetworkManagerObjectInstance.Add(this.gameObject);
    //     } 
    //     else
    //     {
    //         NetworkManagerObjectInstance.Add(this.gameObject);
    //         networking = NetworkManagerObjectInstance[0].GetComponent<Networking>();
    //     }
    // }
    
    public void networkInitialGameState(GameState gameState)
    {
        this.initialGameState = gameState;
    }

    public void setNetworkAsServer()
    {
        isServer = true;
        startNetworkingClass();
    }

    public void setNetworkAsClient()
    {
        isServer = false;
        startNetworkingClass();
    }

    void startNetworkingClass()
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
            }, () => 
            {
                if(isServer)
                {
                    startServer();
                }
                else
                {
                    startClient();
                }
            }
            );
        }
    }

    public void startServer()
    {
        print("server started");
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

        dictionaryReadyToBeSent = true;
    }

    void DeviceOnDisconnectCallBack(Networking.NetworkDevice disconnectedDevice)
    {

        print("disconnected callback happening");
        if(connectedDeviceList != null)
        {
            if (connectedDeviceList.Contains(disconnectedDevice))
            {
                connectedDeviceList.Remove(disconnectedDevice);
            }
        }

        if(devicesDataSentTo != null)
        {
            if (devicesDataSentTo.Contains(disconnectedDevice))
            {
                devicesDataSentTo.Remove(disconnectedDevice);
            }
        }

        dictionaryReadyToBeSent = false;
    }

    void onDeviceDataCallBack(Networking.NetworkDevice dataDevice, string deviceCharacteristic, Byte[] bytes)
    {
        print("on data callback");
    }

    public void ProvideHiddenBoardNetworkCallbacks(Action<Dictionary<CardType, List<string>>> DictionaryReceivedCallback, Action<Dictionary<string, bool>> WordsSelectedCallback, Action<CurrentGameState> GameStateChangedCallback, Action<string> languageChangedCallBack)
    {
        this.DictionaryReceivedCallback = DictionaryReceivedCallback;
        this.WordsSelectedCallback = WordsSelectedCallback; 
        this.GameStateChangedCallback = GameStateChangedCallback; 
        this.languageChangedCallBack = languageChangedCallBack;
    }

    void startClient()
    {
        print("starting client...");
        startNetworkingClass();
        receivedByteArray = new Byte[] { };
        networking.StartClient("treasure_hunt", SystemInfo.deviceUniqueIdentifier, StartedAdvertisingCallBack, receivedDataCallBack);
    }

    void StartedAdvertisingCallBack()
    {
        print("started advertising callback");
    }

    public void receivedDataCallBack(string someString, string deviceCharacteristic, Byte[] bytes)
    {
        receivedByteArray = receivedByteArray.Concat(bytes).ToArray();

        if(Encoding.Unicode.GetString(bytes) == "dictionaryFinished")
        {
            var dict = returnByteArrayAsDictionary(receivedByteArray);
            DictionaryReceivedCallback(dict);
            receivedByteArray = new Byte[]{};
        }     

        if(Encoding.Unicode.GetString(bytes) == "cardSelected")
        {
            receivedByteArray = new Byte[]{};
        }

        if(Encoding.Unicode.GetString(bytes) == "languageChanged")
        {
            receivedByteArray = new Byte[]{};
        }

        if(Encoding.Unicode.GetString(bytes) == "gameStateChanged")
        {
            receivedByteArray = new Byte[]{};
        }
    }

    public void StopServer()
    {
        networking.StopServer(onStopServerCallback);
    }

    void onStopServerCallback()
    {
        print("server stopped callbacked");
    }

    public void StopClient()
    {
        networking.StopClient(onStopClientCallback); 
    }

    void onStopClientCallback()
    {
        print("client stopped callback");
    }

    void convertDataBeforeSending()
    {
        var seriablizableDict = returnCardObjectsAsSerializableDictionary(initialGameState.hiddenBoardList);
        byteArrayToSend = returnSerializableDictionaryAsByteArray(seriablizableDict);
        sendToNextDevice();
    }

    void sendToNextDevice()
    {
        var deviceIndex = devicesDataSentTo.Count;
        var whatsLeftInTheArray = byteArrayToSend.Length;

        for (int i = 0; i <= byteArrayToSend.Length; i += 300)
        {
            var smallArrayLength = 300;

            if (whatsLeftInTheArray < 300)
            {
                smallArrayLength = whatsLeftInTheArray;
            }

            var smallByteArray = new ArraySegment<Byte>(byteArrayToSend, i, smallArrayLength).ToArray();

            networking.WriteDevice(connectedDeviceList[deviceIndex], smallByteArray, () =>
            {
                print("small array written");
            }
            );

            whatsLeftInTheArray -= smallArrayLength;
        }
        byte[] transmissionFinishedIndicator = Encoding.Unicode.GetBytes("dictionaryFinished");
        networking.WriteDevice(connectedDeviceList[deviceIndex], transmissionFinishedIndicator, dictionarySuccessfullySentToDevice);
        devicesDataSentTo.Add(connectedDeviceList[deviceIndex]);
    }

    void dictionarySuccessfullySentToDevice()
    {
        print("data written");
        if(devicesDataSentTo.Count < connectedDeviceList.Count)
        {
            dictionaryReadyToBeSent = true;
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
        if(dictionaryReadyToBeSent)
        {
            dictionaryReadyToBeSent = false;
            if (devicesDataSentTo == null)
            {
                devicesDataSentTo = new List<Networking.NetworkDevice>(){};
            }

            if (devicesDataSentTo.Count < connectedDeviceList.Count)
            {
                print("The number of devices that data has been sent to is less than the number of connected devices, so data will be sent");
                convertDataBeforeSending();
            }  
        }
    }
}
