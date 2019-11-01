using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EoGButtonHandler : MonoBehaviour
{
    
    Network network; 

    public void restartGame()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        var EoGCanvasAninmator = EoGCanvasObject.GetComponent<Animator>();

        var StoreCanvasObject = GameObject.Find("StoreCanvas");
        var StoreCanvasAnimator = StoreCanvasObject.GetComponent<Animator>();

        var MainBoardCanvasObject = GameObject.Find("MainBoardCanvas");
        var MainBoardCanvasAnimator = MainBoardCanvasObject.GetComponent<Animator>();

        var cards = EoGCanvasObject.GetComponent<EoGScript>().cards;

        foreach(GameObject card in cards)
        {
            Destroy(card);
        }
       
        EoGCanvasAninmator.Play("ResultsAnimationReverse");
        StoreCanvasAnimator.Play("StoreAnimationReverse");
        MainBoardCanvasAnimator.Play("MainboardCanvasReverseAnimation");

        network = GameObject.Find("NetworkManager").GetComponent<Network>();
        network.StopServer();
    }
    
    public void showBoard()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        var EoGCanvasAninmator = EoGCanvasObject.GetComponent<Animator>();
        EoGCanvasAninmator.Play("ResultsAnimationReverse");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


