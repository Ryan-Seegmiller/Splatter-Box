using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [NonSerialized] static public GameManager instance;
    [HideInInspector] public List<GameObject> combatants = new List<GameObject>();
    [Header("Audio/Particles")]
    [HideInInspector]public AudioSourceHelper audioSourceHelper;
    [HideInInspector] public ParticleSystemHelper particleSystemHelper;
    [Search(typeof(ParticleSystemReference))]public ParticleSystemReference[] particleSystemReferences;
    [Search(typeof(ReferenceAudio))] public ReferenceAudio[] audioReferneces;

    //Colors
    [Header("Player")]
    [NonReorderable]public Sprite[] playerImages = new Sprite[4];
    public PlayerManagerScript playerManagerScript;
    public PlayerStatsUIRefernce playerStatsUI;

    //GameRules
    [HideInInspector] public GameStates currentState;
    [HideInInspector] public GameModes currentMode;
    public EditableTimers[] timers = new EditableTimers[2];
    private float mainTimer = 3;
    private float timerMax;
    [HideInInspector]public float maxLives = 3;

    //Events
    [Header("Events")]
    public UnityEvent suddenDeath;
    public delegate void PlayerStats();

    public PlayerStats resetPlayers = null;
    public GameObject disasterManager;


    //UI
    [Header("UI")]
    [SerializeField] public GameObject MenuUI;
    [SerializeField] public PlayUI playUI;
    [HideInInspector] public GameObject player;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject suddenDeathWarningText;

    //Music
    [SerializeField, Audio] private string playMusic;
    [SerializeField, Audio] private string suddenDeathMusic;
    [SerializeField, Audio] private string menuMusic;
    [SerializeField, Audio] private string tenSecondWarning;
    [SerializeField, Audio] private string suddenDeathWarning;

    private bool closeCountDown;

    #region AwakeCalls
    private void Awake()
    {
        audioSourceHelper = GetComponentInChildren<AudioSourceHelper>();
        audioSourceHelper.refernceAudioArray = audioReferneces;
        instance = GetInstance();
        SetUpPSHelper();

        //GameRules
        maxLives = 3;

        timers[0].timeInMinutes = mainTimer;
        ChangeTimers();
        //Sets state
        SetNewState(GameStates.Menu);
        SetGameMode(GameModes.Survival);
    }
    public static GameManager GetInstance()
    {
        //Singleton pattern
        if (instance == null)
        {
            return new GameManager();
        }
        else
        {
            return instance;
        }
    }
    GameManager()
    {
        instance = this;
    }
    #endregion

    private void Update()
    {
        PlayTimer();
    }
    public void ChangeTimers()
    {
        //Timer setup
        mainTimer = 60f * timers[0].timeInMinutes;
        timerMax = mainTimer;

    }

    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;

        #else
        Application.Quit();
        #endif


    }

    #region Base GameManager
    /// <summary>
    /// Generates the audio helper
    /// </summary>
    public void GenerateAudioHelper()
    {
        audioSourceHelper = GetComponentInChildren<AudioSourceHelper>();
        if (audioSourceHelper == null)
        {
            GameObject go = new GameObject("Audio Source Helper", typeof(AudioSourceHelper));
            go.transform.parent = transform;
            audioSourceHelper = go.GetComponent<AudioSourceHelper>();
        }
        audioSourceHelper.refernceAudioArray = audioReferneces;
        audioSourceHelper.CreateAudioScources();
    }
    //Creates the particle system helper
    private void SetUpPSHelper()
    {
        if(ParticleSystemHelper.instance == null) 
        {
            Type[] psHelperComponents = new Type[] { typeof(ParticleSystemHelper) };
            GameObject psHelperGO = new GameObject("Particle System Helper", psHelperComponents);

            psHelperGO.transform.SetParent(transform, false);
            psHelperGO.GetComponent<ParticleSystemHelper>().GetInstance();

        }
        particleSystemHelper = ParticleSystemHelper.instance;

    }
    #endregion

    #region StateMachine
    public void SetNewState(GameStates state)
    {
        if (currentState == state) { return; } 
        currentState = state;
        switch (state)
        {
            //Initalize play state
            case GameStates.Play:
                //Start the game
                mainTimer = timerMax;
                playUI.gameObject.SetActive(true);
                MenuUI.SetActive(false);
                panel.SetActive(false);
                resetPlayers.Invoke();
                audioSourceHelper.PlaySoundLooping(playMusic, transform);
                disasterManager.SetActive( true );
                break;
            //Initailize menu state
            case GameStates.Menu:
                //Stop all play items
                playUI.gameObject.SetActive(false);
                playUI.timer.color = Color.black;
                audioSourceHelper.StopMusic();
                audioSourceHelper.PlaySoundLooping(menuMusic, transform);
                MenuUI.SetActive(true);
                break;
            case GameStates.SuddenDeath: 
                suddenDeath.Invoke();
                audioSourceHelper.PlaySoundLooping(suddenDeathMusic, transform);
                audioSourceHelper.PlaySound(suddenDeathWarning, transform);

                break;

            case GameStates.Win:
                //Stop all play items and show the win UI

                break;
            default:
                throw new NotImplementedException();
        } 
       
    }
    #endregion

    #region PlayModes
    public void SetGameMode(GameModes mode)
    {
        if(currentMode == mode) { return; }
        currentMode = mode;
        switch (mode)
        {
            case GameModes.SuddenDeath :  
                maxLives = 1;
                break;
            case GameModes.Survival :  
                break;
            case GameModes.DeathMatch :
                maxLives = 0;
                break;
        }
    }
    private void PlayTimer()
    {
        if(currentState != GameStates.Play && currentState != GameStates.SuddenDeath && currentMode != GameModes.SuddenDeath) { return; }
        
        mainTimer -= Time.deltaTime * Time.timeScale;
        if(mainTimer > 0)
        {
            mainTimer -= Time.deltaTime * .1f;
        }
        else if(mainTimer < 0)
        {
            mainTimer = 0;
            SetNewState(GameStates.Menu);
            closeCountDown = false;
        }
        if(currentState == GameStates.Menu || mainTimer < 0) { return; }
        int minutes = Mathf.FloorToInt(mainTimer / 60);
        int seconds = Mathf.FloorToInt(mainTimer % 60);
        string timer = string.Format("{0:00} : {1:00}", minutes, seconds);
        if(minutes <= 0 && seconds <= 31)
        {
            playUI.timer.color = Color.red;
            SetNewState(GameStates.SuddenDeath);
        }
        if(minutes <= 0 && seconds <= 10 && !closeCountDown)
        {
            audioSourceHelper.PlaySound(tenSecondWarning, transform);
            closeCountDown = true;
        }
        SuddenDeath(seconds);
        playUI.timer.text = timer;
    }
    #endregion
    private void SuddenDeath(int seconds)
    {
        if (currentState != GameStates.SuddenDeath || seconds <= 20) { panel.SetActive(false); suddenDeathWarningText.SetActive(false); return; }
        if (seconds >= 30) { return; }
        panel.SetActive(true);
        if(seconds % 2 == 0)
        {
            suddenDeathWarningText.SetActive(true);
            panel.gameObject.GetComponent<Image>().color = new Color(1,0,0,.5f);
            
        }
        else
        {
            suddenDeathWarningText.SetActive(false);
            panel.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, .5f);

        }

    }
}
