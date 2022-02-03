using System;
using Constants;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the managing of when the game music plays and how
/// </summary>
public class GameManager : MonoBehaviour
{
    public AudioSource music;
    public bool hasMusicStarted;
    public GameState gameState;
    public BeatScroller beatScroller;
    public uint gameLevel;
    public TextMeshProUGUI levelDescriptionText;
    public TextMeshProUGUI startInstructionsText;
    public static GameManager s_gameInstance;
    public Text scoreText;
    public Text multiplierText;

    public int m_currentScore;
    public int m_currentMultiplier;
    public int m_multiplierTracker;
    public int[] m_multiplierThresholds;

    public float m_totalNotes;
    public float m_normalHits;
    public float m_goodHits;
    public float m_perfectHits;
    public float m_missedHits;
    
    #region Scoreboard Values
    public GameObject resultsScreen;
    public Text percentHitsText, normalHitsText, goodHitsText, perfectHitsText, missedHitsText, rankText, finalScoreText;
    #endregion
    
    public GameObject pauseMenu;

    void Awake()
    {
        // Subscribe to the change state event
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        // Handle any memory leaks
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject pauseMenuInstance = Instantiate(pauseMenu);
        DontDestroyOnLoad(pauseMenuInstance);
        
        s_gameInstance = this;
        m_totalNotes = FindObjectsOfType<MusicNote>().Length;
        scoreText.text = "Chill Meter: 0";
        levelDescriptionText.text = $"{levelDescriptionText.text} {gameLevel}";
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasMusicStarted)
        {
            if (Input.anyKeyDown)
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
                startInstructionsText.enabled = false;
                hasMusicStarted = true;
                beatScroller.hasStarted = true;
                music.Play();
            }
        }
        else if(!music.isPlaying && !resultsScreen.activeInHierarchy && gameState != GameState.Paused) 
        {
            DisplayScoreBoard();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Capture game state
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;
                
            // Set listener awareness of the instance to the new game state
            GameStateManager.Instance.SetState(newGameState);
            
            // Track state for the unity editor
            gameState = newGameState;
        }
    }

    public void NoteHit(HitType hitType)
    {
        Debug.Log("Hit On Time!!");

        // Ensures we dont go out of bounds
        if (m_currentMultiplier - 1 < m_multiplierThresholds.Length)
        {
            m_multiplierTracker++;
            
            // If our currently tracked multiplier has reached the next threshold
            // Reset tracker, add to the current multiplier
            if (m_multiplierThresholds[m_currentMultiplier - 1] <= m_multiplierTracker)
            {
                m_multiplierTracker = 0;
                m_currentMultiplier++;
            }
        }
        
        UpdateScoreBoard(hitType);
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note!!");

        ResetScoreMultiplier();
        UpdateScoreBoard(HitType.Miss);
    }

    public void ResetScoreMultiplier()
    {
        m_currentMultiplier = 1;
        m_multiplierTracker = 0;
        multiplierText.text = "Multiplier: x" + m_currentMultiplier;
    }

    public void UpdateScoreBoard(HitType hitType)
    {
        var scoreValue = (int) hitType;

        UpdateScore(scoreValue);
        RecordHitTypes(hitType);
    }
    
    private void UpdateScore(int score)
    {
        m_currentScore += score * m_currentMultiplier;
        scoreText.text = "Chill Meter: " + m_currentScore;
        multiplierText.text = "Multiplier: x" + m_currentMultiplier;
    }

    private void RecordHitTypes(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Normal:
                m_normalHits++;
                break;
            case HitType.Good:
                m_goodHits++;
                break;
            case HitType.Perfect:
                m_perfectHits++;
                break;
            case HitType.Miss:
                m_missedHits++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hitType), hitType, "Invalid hit type");
        }
    }

    private void DisplayScoreBoard()
    {
        resultsScreen.SetActive(true);
        normalHitsText.text = m_normalHits.ToString();
        goodHitsText.text = m_goodHits.ToString();
        perfectHitsText.text = m_perfectHits.ToString();
        missedHitsText.text = m_missedHits.ToString();

        var totalHitNotes = m_normalHits + m_goodHits + m_perfectHits;
        var percentHit = (totalHitNotes / m_totalNotes) * 100f;

        percentHitsText.text = percentHit.ToString("F1") + "%";
        finalScoreText.text = m_currentScore.ToString();
        
        rankText.text = CalculateRank(percentHit);
    }

    private static string CalculateRank(float percentHit)
    {
        if (percentHit >= Rating.RankS)
        {
            return "S";
        }
        else if (percentHit >= Rating.RankA)
        {
            return "A";
        }
        else if (percentHit >= Rating.RankB)
        {
            return "B";
        }
        else if (percentHit >= Rating.RankC)
        {
            return "C";
        }
        else if (percentHit >= Rating.RankD)
        {
            return "D";
        }
        else
        {
            return "F";
        }
    }

    private void SetIsPauseMenuDisplayed(bool value)
    {
        // Instantiate(pauseMenu);
        pauseMenu.SetActive(value);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        // enabled = newGameState == GameState.Gameplay;
        
        // Control game behaviors based on state
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            music.Pause();
            beatScroller.hasStarted = false;
            SetIsPauseMenuDisplayed(true);
        }
        else
        {
            music.UnPause();
            beatScroller.hasStarted = true;
            SetIsPauseMenuDisplayed(false);
        }
    }
}
