using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonIconOverlay : MonoBehaviour
{
    public GameObject icon;
    private Button button;
    public bool isPressed = false;
    public UnityEvent<bool> OnToggle;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
        icon.SetActive(false);
        if (OnToggle == null)
        {
            OnToggle = new UnityEvent<bool>();
        }
    }

    private void Toggle()
    {
        isPressed = !isPressed;
        if (isPressed)
        {
            icon.SetActive(true);
        } else
        {
            icon.SetActive(false);
        }
        OnToggle?.Invoke(isPressed);
    }

    
}
