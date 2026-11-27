using UnityEngine;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public static event Action Tick; // Event triggered every second

    [SerializeField] private TextMeshProUGUI timerText;

    public float timeLimit = 600f; // Time in seconds 
    public float timeRemaining, tickTimer;
    public static bool timerIsRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        timeRemaining = timeLimit;
        timerIsRunning = false;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining -= Time.deltaTime;
            tickTimer += Time.deltaTime;

            if (tickTimer >= 1.0f)
            {
                Tick?.Invoke();
                tickTimer -= 1.0f;
            }

            if (timeRemaining <= 0)
            {
                Debug.Log("Time has run out!");
                PlayerStatus._instance.TimeUp();
                timeRemaining = 0;
                timerIsRunning = false;
            }

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
