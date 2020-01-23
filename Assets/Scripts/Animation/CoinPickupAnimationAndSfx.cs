using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class CoinPickupAnimationAndSfx : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public void OnDrag(PointerEventData pointerEventData)
    {
        rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.forward, -rotX);
        transform.Rotate(Vector3.right, rotY);

        angleX += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        angleX = Mathf.Clamp(angleX, -5.0f, 5.0f);
        transform.rotation = Quaternion.Euler(angleY, 0.0f, -angleX);

        angleY += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -10.0f, 10.0f);
        transform.rotation = Quaternion.Euler(angleY, 0.0f, -angleX);


        

    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        var rotateBack = rectTransform.DORotate(initialRotation, 0.5f, RotateMode.Fast);
        rotateBack.Play();
        
    }

    float angleX = -0.0f;
    float angleY = 0.0f;
    float rotSpeed = 10.0f;
    float rotX;
    float rotY;
    RectTransform rectTransform;
    Vector3 initialRotation;
    Vector3 pushedScale;
    Vector3 initialScale;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRotation = new Vector3(0, 0, 0);
        initialScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 1.3f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.InOutBack);
        GlobalAudioScript.Instance.playSfxSound2("pickup2");
        GlobalAudioScript.Instance.playSfxSound("metal_drag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();
        GlobalAudioScript.Instance.playSfxSound2("coin_drop");
        GlobalAudioScript.Instance.playSfxSound("metal_drag");
    }
}