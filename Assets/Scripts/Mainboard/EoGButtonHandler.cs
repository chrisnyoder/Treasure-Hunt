using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EoGButtonHandler : MonoBehaviour
{
    
    public void restartGame()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        var EoGCanvasRT = EoGCanvasObject.GetComponent<RectTransform>();

        var StoreCanvasObject = GameObject.Find("StoreCanvas");
        var StoreCanvasRT = StoreCanvasObject.GetComponent<RectTransform>();

        var MainBoardCanvasObject = GameObject.Find("MainBoardCanvas");
        var MainBoardCanvasRT = MainBoardCanvasObject.GetComponent<RectTransform>();
       
        EoGCanvasRT.localPosition = new Vector3(EoGCanvasRT.localPosition.x, -1500, 0);
        StoreCanvasRT.localPosition = new Vector3(StoreCanvasRT.localPosition.x, 0, 0);
        MainBoardCanvasRT.localPosition = new Vector3(EoGCanvasRT.localPosition.x, -3000, 0);
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
