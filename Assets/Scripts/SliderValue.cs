using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SliderValue : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI valuePanel;

    // Start is called before the first frame update
    void Start()
    {
        valuePanel = GetComponent<TextMeshProUGUI>();
        slider = transform.parent.GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        valuePanel.text = String.Format("{0:0.##}", slider.value);
    }
}
