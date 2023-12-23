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

    // Start is called before the first frame update
    void Start()
    {
        UpdateSlider();
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
        UpdateSlider();
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

    public void ResetFruitPhysicsSettings()
    {
        GameController.instance.gameSettings.physicsMaterial.friction = GameController.instance.gameSettings.defaultPhysicsMaterial.friction;
        GameController.instance.gameSettings.physicsMaterial.bounciness = GameController.instance.gameSettings.defaultPhysicsMaterial.bounciness;
        UpdateSlider();
    } 

    private void UpdateSlider()
    {
        frictionSlider.value = GameController.instance.gameSettings.physicsMaterial.friction;
        bouncinessSlider.value = GameController.instance.gameSettings.physicsMaterial.bounciness;
    }
}
