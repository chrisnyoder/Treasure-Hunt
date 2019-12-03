using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    Animator joinGameAnimation;
    // Start is called before the first frame update
    
    void Start()
    {
        joinGameAnimation = GetComponent<Animator>();
        StartCoroutine(playJoinGameAnimaton());
    }

    IEnumerator playJoinGameAnimaton()
    {
        yield return new WaitForSeconds(0.3f);
        joinGameAnimation.Play("JoinGameAnimation");       
    }
}
