using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    /* TO HEAR MUSIC AND SOUND EFFECTS AND 
    TO BE ABLE TO USE GAME MANAGER AND THE CANVAS IN OTHER SCENES */
    public static GameManager Instance {get; private set;}

    // For Background / Menu Music
    private AudioSource[] musicAudioSources;
    public AudioClip[] musicAudioClips;

    private AudioSource backgroundMusic;

    // For AudioClips
    private AudioSource[] sfxAudioSources;
    public AudioClip[] sFXAudioClips;

    public Slider musicSlider;
    public Slider sFXSlider;
    
    public TextMeshProUGUI musicVolume;
    public TextMeshProUGUI sFXVolume;

    public GameObject health;
    public GameObject wave;
    public GameObject ammo;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI ammoText;

    private int healthPoints;
    private int waveNumber;
    private int ammoCount;

    public GameObject canvas;
    public GameObject titleScreen;
    public GameObject settingsScreen;
    public GameObject controlsScreen;
    public GameObject gameOverScreen;

    public static bool isGameActive;

    // ------ TO CONTINUE USING AUDIO BETWEEN SCENES ------
    // ------ TO BE ABLE TO USE GAME MANAGER AND TEXTMESHPROUGUI IN OTHER SCENES ------
    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;

            // Keep GameManager, the canvas and audio across scenes
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvas);
        }
        else
        {
            Destroy(gameObject); // Only one instance allowed
        }
    }

    // Start is called before the first frame update
    void Start(){

        // Show title screen here so it can loop when you go back to "Main Menu" from the game over screen
        titleScreen.SetActive(true);

        backgroundMusic = GetComponent<AudioSource>();
        //Initialise the AudioSources for background music and SFX
        musicAudioSources = new AudioSource[musicAudioClips.Length];
        sfxAudioSources = new AudioSource[sFXAudioClips.Length];

        // Use AddComponent to add new AudioSources 
        for (int i = 0; i < musicAudioClips.Length; i++){
            musicAudioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        for (int i = 0; i < sFXAudioClips.Length; i++){
            sfxAudioSources[i] = gameObject.AddComponent<AudioSource>();
        }
        
        //Set initial volume and add listeners for arrays 
        UpdateMusicVolume(musicSlider.value);
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);

        UpdateSFXVolume(sFXSlider.value);
        sFXSlider.onValueChanged.AddListener(UpdateSFXVolume);

        // Set initial volume for beginnning menu audio 
        backgroundMusic.volume = musicSlider.value;
        musicSlider.onValueChanged.AddListener(UpdateBackgroundMusicVolume);
    }


    // Switch scenes and Start the game
    public void StartGame(){
        PlayBackgroundMusic();
        isGameActive = true;
        waveNumber = 0;
        healthPoints = -10;
        UpdateHealth(-10);
        UpdateWave();
        UpdateAmmo(5);
        titleScreen.SetActive(false);
        health.SetActive(true);
        wave.SetActive(true);
        ammo.SetActive(true);
        SceneManager.LoadScene("Day of the Deer");
    }

    // Update is called once per frame
    void Update(){
    
    }

    // Method to Update the health
    public void UpdateHealth(int change){

        if (healthPoints == -10){
            healthPoints = 10;
        }
        else{
            healthPoints += (change);
        }

        healthText.text = "Health: " + healthPoints;
    }

    // Method to Update the wave
    public void UpdateWave(){
        waveNumber++;
        waveText.text = "Wave: " + waveNumber;
    }

    // Method to call Coroutine as GameManager.Instance.StartCoroutine(UpdateAmmo(int number)) can't be used
    public void UpdateAmmo(int number){
        StartCoroutine(RealUpdateAmmo(number));
    }

    // Method to update the ammo
    public IEnumerator RealUpdateAmmo(int number){
        if (number == 5){
            ammoCount = 5;
            ammoText.text = "Ammo: " + ammoCount;
        }
        else if (number == 1){
            ammoCount--;
            ammoText.text = "Ammo: " + ammoCount;
        }
        else if (number == 0){
            yield return new WaitForSeconds(10);
        }

    }

    // Button to open Settings
    public void Settings(){
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    // Button to open Controls
    public void Controls(){
        titleScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    // Button to open Github page
    public void GithubLink(){
        Application.OpenURL("https://github.com/Cathal-Farrell/SCK-IMM-Game-Final");
    }

    // Button for exit
    public void Exit(){
        // UnityEditor.EditorApplication.isPlaying = false;
        // UnityEditor keyword causes issues with building WebGL
        Application.Quit();
    }

    // Button to leave Settings and Controls Screen
    public void Back(){
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    // Game Over method
    public void GameOver(){
        gameOverScreen.SetActive(true);
    }


    // ----------------   FOR AUDIO   -------------------

    // Play specific sound effect by index without needing the clip set on the AudioSource
    public void PlaySFXByIndex(int index)
    {
        if (index >= 0 && index < sFXAudioClips.Length)
        {
            sfxAudioSources[index].PlayOneShot(sFXAudioClips[index]);
        }
    }

    public void PlayMusicByIndex(int index)
    {
        if (index >= 0 && index < musicAudioClips.Length)
        {
            musicAudioSources[index].PlayOneShot(musicAudioClips[index]);
        }
    }

    // Method to change Background and Game Over volume
    public void UpdateMusicVolume(float volume){

        // For every audio in the array 
        foreach (var audio in musicAudioSources){

                // Divided by 5 because it is very loud
                audio.volume = volume / 5;
        }

        // Call method to update UI
        UpdateMusicVolumeText(volume);
    }

    // Method to set Background volume before first loop
    public void UpdateBackgroundMusicVolume(float volume){
        backgroundMusic.volume = volume;
        UpdateMusicVolumeText(volume);

    }

    // Method to update the Music Slider text
    public void UpdateMusicVolumeText(float volume){
        musicVolume.text = "Music:  " + (Mathf.Round(volume * 100f));
    }

    // Method to change SFX Volume
    public void UpdateSFXVolume(float volume){

        // For every audio in the array
        foreach (var audio in sfxAudioSources){

                // Divided by 5 because it is very loud
                audio.volume = volume / 5;
        }

        // Call method to update UI
        UpdateSFXVolumeText(volume);
    }

    // Method to update the SFX Slider text
    public void UpdateSFXVolumeText(float volume){
        sFXVolume.text = "SFX:  " + (Mathf.Round(volume * 100f));
    }

    // Stop playing all music 
    public void StopAllMusic(){
        foreach (var audio in musicAudioSources){
            audio.Stop();
        }
    }

    // Start Playing the background music when you click "Main Menu" and "Play Again" from game over screen
    public void PlayBackgroundMusic(){
        if (backgroundMusic.isPlaying == false){
            backgroundMusic.Play();
        }
    }

    // Stop background music to play game over music
    public void StopBackgroundMusic(){
        backgroundMusic.Stop();
    }

}
