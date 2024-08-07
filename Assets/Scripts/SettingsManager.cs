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

    public Slider soundFXVolume;
    public ButtonIconOverlay muteSoundFXOverlay;

    private JukeBox jukebox;
    private SoundFXManager soundFXManager;

    private bool isPlaying = false;
    private bool isLost = false;

    // Start is called before the first frame update
    void Start()
    {
        jukebox = GetComponentInChildren<JukeBox>();
        jukebox.AdjustVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
        jukebox.SetMute(PlayerPrefs.GetInt("MuteMusic", 0) == 1 ? true : false);
        jukebox.PlayNextMusic(PlayerPrefs.GetInt("MusicIndex", 0));

        soundFXManager = GetComponentInChildren<SoundFXManager>();
        soundFXManager.AdjustVolume(PlayerPrefs.GetFloat("SoundFXVolume", 1));
        soundFXManager.SetMute(PlayerPrefs.GetInt("MuteSoundFX", 0) == 1 ? true : false);

        UpdateSettingsMenu();
        settingsMenu.SetActive(false);
    }

    public void HideSettingsMenu()
    {
        if (isPlaying)
        {
            GameController.instance.playerInputActions.GameScene.Enable();
        }
        if (isLost)
        {
            GameController.instance.playerInputActions.GameScene.Enable();
        }
        settingsMenu.SetActive(false);
        GameController.instance.playerInputActions.SettingsMenu.Disable();
    }
    public void ShowSettingsMenu()
    {
        isPlaying = GameController.instance.playerInputActions.GameScene.enabled;
        isLost = GameController.instance.playerInputActions.GameOverScreen.enabled;
        GameController.instance.playerInputActions.GameScene.Disable();
        GameController.instance.playerInputActions.GameOverScreen.Disable();
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
        GameController.instance.gameSettings.mergeExplosionForce = value / 10f; // Hack to align on decimal digit
        if (GameController.instance.gameSettings.mergeExplosionForce == 0)
        {
            mergeExplosionRadiusModifierSlider.interactable = false;
            mergeExplosionUpwardsModifierSlider.interactable = false;
        }
        else
        {
            mergeExplosionRadiusModifierSlider.interactable = true;
            mergeExplosionUpwardsModifierSlider.interactable = true;
        }
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

    public void ToggleMuteMusic(bool value)
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

    public void ToggleMuteSoundFX(bool value)
    {
        soundFXManager.SetMute(value);
    }

    public void OnSoundFXVolumeChange(float value)
    {
        soundFXManager.AdjustVolume(value / 100);
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

        mergeExplosionForceSlider.value = GameController.instance.gameSettings.mergeExplosionForce * 10f; // Part of hack
        mergeExplosionRadiusModifierSlider.value = GameController.instance.gameSettings.mergeExplosionRadiusModifier;
        mergeExplosionUpwardsModifierSlider.value = GameController.instance.gameSettings.mergeExplosionUpwardsModifier;
        if (GameController.instance.gameSettings.mergeExplosionForce == 0)
        {
            mergeExplosionRadiusModifierSlider.interactable = false;
            mergeExplosionUpwardsModifierSlider.interactable = false;
        }
        else
        {
            mergeExplosionRadiusModifierSlider.interactable = true;
            mergeExplosionUpwardsModifierSlider.interactable = true;
        }

        dynamicDropperEdgeToggle.isOn = GameController.instance.gameSettings.useDynamicDropperEdgeOffset;

        musicVolume.value = jukebox.source.volume * 100;
        musicList.value = jukebox.musicIndex;
        muteMusicOverlay.SetMuteIcon(jukebox.source.mute);

        soundFXVolume.value = soundFXManager.source.volume * 100;
        muteSoundFXOverlay.SetMuteIcon(soundFXManager.source.mute);
    }
}
