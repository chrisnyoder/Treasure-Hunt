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
    private string clientId;

    public static Networking networking;

    private int whatsLeftInTheArray = 0;
    private int byteArrayIndex = 0;
    private bool dictionaryReadyToBeSentToDevice = false; 
    private bool arrayPartFinishedSending = true; 

    private List<Networking.NetworkDevice> connectedDeviceList = null;
    private List<Networking.NetworkDevice> devicesDataSentTo = null; 
    
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
            print("networking class is null");
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
            dictionaryReadyToBeSentToDevice = true;
        } else 
        {
            print("device already in the connected device list, so it won't bother sending the dictionary");
        }
    }

    void writeTestArray(Networking.NetworkDevice device)
    {
        Byte[] testByteArray = new Byte[]{1, 2, 3, 4, 5};
        networking.WriteDevice(device, testByteArray, () =>
        {
            print("small array written");
        });
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

        dictionaryReadyToBeSentToDevice = false;
    }

    void onDeviceDataCallBack(Networking.NetworkDevice dataDevice, string deviceCharacteristic, Byte[] bytes)
    {
        print("on data callback");
        string clientDataReceived = Encoding.Unicode.GetString(bytes);
        if(clientDataReceived == "stopped")
        {
            if(connectedDeviceList.Contains(dataDevice))
            {
                dataDevice.DoDisconnect = true;
                networking.SetState(Networking.States.Disconnect, 0.1f);
                connectedDeviceList.Remove(dataDevice);
            }
        }
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

        var rd = new System.Random();
        clientId = rd.Next(1000).ToString();
        networking.StartClient("treasure_hunt", clientId, StartedAdvertisingCallBack, receivedDataCallBack);
    }

    void StartedAdvertisingCallBack()
    {
        print("started advertising callback");
    }

    public void receivedDataCallBack(string someString, string deviceCharacteristic, Byte[] bytes)
    {
        receivedByteArray = receivedByteArray.Concat(bytes).ToArray();
        print("data successfully received: " + bytes);

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
        print("client stopped");
        networking.StopClient(onStopClientCallback); 
    }

    void onStopClientCallback()
    {
        print("on client stopped callback received, sending message to the server that client is no longer advertsiing");
        byte[] stoppedAdvertising = Encoding.Unicode.GetBytes("stopped");
        networking.SendFromClient(stoppedAdvertising);
    }

    void convertDataBeforeSending()
    {
        var seriablizableDict = returnCardObjectsAsSerializableDictionary(initialGameState.hiddenBoardList);
        byteArrayToSend = returnSerializableDictionaryAsByteArray(seriablizableDict);
    }

    void sendPartOfArray(Networking.NetworkDevice device)
    {
        var smallArrayLength = 300;

        if(whatsLeftInTheArray == 0)
        {
            whatsLeftInTheArray = byteArrayToSend.Length;
        }

        if (whatsLeftInTheArray < 300)
        {
            smallArrayLength = whatsLeftInTheArray;
        }

        whatsLeftInTheArray -= smallArrayLength;
        var smallByteArray = new ArraySegment<Byte>(byteArrayToSend, byteArrayIndex, smallArrayLength).ToArray();

        networking.WriteDevice(device, smallByteArray, () =>
        {
            print("small array written");
            if (whatsLeftInTheArray == 0)
            {
                transmissionFinished(device);
                arrayPartFinishedSending = true;
            } else 
            {
                arrayPartFinishedSending = true;
                dictionaryReadyToBeSentToDevice = true;
            }
        }
        );

        byteArrayIndex += 300; 
    }

    void transmissionFinished(Networking.NetworkDevice device)
    {
        byte[] transmissionFinishedIndicator = Encoding.Unicode.GetBytes("dictionaryFinished");
        networking.WriteDevice(device, transmissionFinishedIndicator, dictionarySuccessfullySentToDevice);
        byteArrayIndex = 0;
        devicesDataSentTo.Add(device);
    }

    void dictionarySuccessfullySentToDevice()
    {
        print("full dictionary written");

        arrayPartFinishedSending = true;

        if(devicesDataSentTo.Count < connectedDeviceList.Count)
        {
            dictionaryReadyToBeSentToDevice = true;
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
        if(dictionaryReadyToBeSentToDevice)
        {   print("dictionary is ready to be sent");
            dictionaryReadyToBeSentToDevice = false;
            if(arrayPartFinishedSending)
            {
                print("small array finished sending");
                arrayPartFinishedSending = false; 
                if (devicesDataSentTo == null)
                {
                    devicesDataSentTo = new List<Networking.NetworkDevice>(){};
                }

                if(byteArrayToSend == null)
                {
                    convertDataBeforeSending();
                }

                if (devicesDataSentTo.Count < connectedDeviceList.Count)
                {   
                    print("update function, sending part of the array");
                    var deviceIndex = devicesDataSentTo.Count;
                    sendPartOfArray(connectedDeviceList[deviceIndex]);
                } else
                {
                    dictionaryReadyToBeSentToDevice = false;
                }
            }
        }
    }
}
