using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public Slider frictionSlider;
    public Slider bouncinessSlider;
    public Toggle dynamicDropperEdgeToggle;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSettingsMenu();
        settingsMenu.SetActive(false);
    }

    public void HideSettingsMenu()
    {
        GameController.instance.playerInputActions.GameScene.Enable();
        settingsMenu.SetActive(false);
        GameController.instance.playerInputActions.SettingsMenu.Disable();
    }
    public void ShowSettingsMenu()
    {
        GameController.instance.playerInputActions.GameScene.Disable();
        settingsMenu.SetActive(true);
        UpdateSettingsMenu();
        GameController.instance.playerInputActions.SettingsMenu.Enable();
    }

    public void OnFrictionChange(float value)
    {
        GameController.instance.gameSettings.physicsMaterial.friction = value;
    }

    public void OnBouncinessChange(float value)
    {
        GameController.instance.gameSettings.physicsMaterial.bounciness = value;
    }

    public void OnDynamicDropperEdge(bool value)
    {
        GameController.instance.gameSettings.useDynamicDropperEdgeOffset = dynamicDropperEdgeToggle.isOn;
    }

    public void ResetSettings()
    {
        GameController.instance.gameSettings.physicsMaterial.friction = GameController.instance.defaultGameSettings.physicsMaterial.friction;
        GameController.instance.gameSettings.physicsMaterial.bounciness = GameController.instance.defaultGameSettings.physicsMaterial.bounciness;
        GameController.instance.gameSettings.useDynamicDropperEdgeOffset = GameController.instance.defaultGameSettings.useDynamicDropperEdgeOffset;
        UpdateSettingsMenu();
    } 

    private void UpdateSettingsMenu()
    {
        frictionSlider.value = GameController.instance.gameSettings.physicsMaterial.friction;
        bouncinessSlider.value = GameController.instance.gameSettings.physicsMaterial.bounciness;
        dynamicDropperEdgeToggle.isOn = GameController.instance.gameSettings.useDynamicDropperEdgeOffset;
    }
}
