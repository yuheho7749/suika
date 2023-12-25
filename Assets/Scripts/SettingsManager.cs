using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public Slider frictionSlider;
    public Slider bouncinessSlider;
    public Slider mergeExplosionForceSlider;
    public Slider mergeExplosionRadiusModifierSlider;
    public Slider mergeExplosionUpwardsModifierSlider;

    public Toggle dynamicDropperEdgeToggle;
    public TMP_Dropdown musicList;
    public Toggle randomizeMusicToggle;
    public Slider musicVolume;
    public ButtonIconOverlay muteMusicOverlay;

    private JukeBox jukebox;

    // Start is called before the first frame update
    void Start()
    {
        jukebox = GetComponent<JukeBox>();
        jukebox.AdjustVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
        jukebox.SetMute(PlayerPrefs.GetInt("MuteMusic", 0) == 1 ? true : false);
        jukebox.PlayNextMusic(PlayerPrefs.GetInt("MusicIndex", 0));
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

    public void OnMergeExplosionForceChange(float value)
    {
        GameController.instance.gameSettings.mergeExplosionForce = value;
    }

    public void OnMergeExplosionRadiusModifierChange(float value)
    {
        GameController.instance.gameSettings.mergeExplosionRadiusModifier = value;
    }

    public void OnMergeExplosionUpwardsModifierChange(float value)
    {
        GameController.instance.gameSettings.mergeExplosionUpwardsModifier = value;
    }

    public void OnDynamicDropperEdge(bool value)
    {
        GameController.instance.gameSettings.useDynamicDropperEdgeOffset = dynamicDropperEdgeToggle.isOn;
    }

    public void ToggleMute(bool value)
    {
        jukebox.SetMute(value);
    }

    public void ChangeMusic(int trackNumber)
    {
        jukebox.PlayNextMusic(trackNumber);
    }

    public void OnRandomizeMusic(bool value)
    {
        jukebox.SetRandomize(randomizeMusicToggle.isOn);
    }

    public void OnMusicVolumeChange(float value)
    {
        jukebox.AdjustVolume(value/100);
    }

    public void ResetSettings()
    {
        GameController.instance.gameSettings.physicsMaterial.friction = GameController.instance.defaultGameSettings.physicsMaterial.friction;
        GameController.instance.gameSettings.physicsMaterial.bounciness = GameController.instance.defaultGameSettings.physicsMaterial.bounciness;
        GameController.instance.gameSettings.mergeExplosionForce = GameController.instance.defaultGameSettings.mergeExplosionForce;
        GameController.instance.gameSettings.mergeExplosionRadiusModifier = GameController.instance.defaultGameSettings.mergeExplosionRadiusModifier;
        GameController.instance.gameSettings.mergeExplosionUpwardsModifier = GameController.instance.defaultGameSettings.mergeExplosionUpwardsModifier;
        
        GameController.instance.gameSettings.useDynamicDropperEdgeOffset = GameController.instance.defaultGameSettings.useDynamicDropperEdgeOffset;
        UpdateSettingsMenu();
    } 

    private void UpdateSettingsMenu()
    {
        frictionSlider.value = GameController.instance.gameSettings.physicsMaterial.friction;
        bouncinessSlider.value = GameController.instance.gameSettings.physicsMaterial.bounciness;
        mergeExplosionForceSlider.value = GameController.instance.gameSettings.mergeExplosionForce;
        mergeExplosionRadiusModifierSlider.value = GameController.instance.gameSettings.mergeExplosionRadiusModifier;
        mergeExplosionUpwardsModifierSlider.value = GameController.instance.gameSettings.mergeExplosionUpwardsModifier;

        dynamicDropperEdgeToggle.isOn = GameController.instance.gameSettings.useDynamicDropperEdgeOffset;

        musicVolume.value = jukebox.source.volume * 100;
        musicList.value = jukebox.musicIndex;
        muteMusicOverlay.SetMuteIcon(jukebox.source.mute);
    }
}
