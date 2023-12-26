using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    private Vector3 originalPos;
    public Button toggleButton;
    public Vector3 hidePos;

    public GameObject buttonIcon;
    public Vector3 originalIconRotation;
    public Vector3 hiddenIconRotation;

    public float animationTime = 1f;

    private bool isHidden = false;

    private void Start()
    {
        originalPos = transform.localPosition;
        toggleButton.onClick.AddListener(OnButtonToggle);
    }

    public void OnButtonToggle()
    {
        // TODO
        if (isHidden)
        {
            //buttonIcon.transform.localRotation = Quaternion.Euler(originalIconRotation);
            LeanTween.rotateLocal(buttonIcon, originalIconRotation, animationTime).setEaseInOutBack();
            LeanTween.moveLocal(gameObject, originalPos, animationTime).setEaseOutElastic();
        } 
        else
        {
            //buttonIcon.transform.localRotation = Quaternion.Euler(hiddenIconRotation);
            LeanTween.rotateLocal(buttonIcon, hiddenIconRotation, animationTime).setEaseInOutBack();
            LeanTween.moveLocal(gameObject, hidePos, animationTime).setEaseOutElastic();
        }
        isHidden = !isHidden;
    }
}
