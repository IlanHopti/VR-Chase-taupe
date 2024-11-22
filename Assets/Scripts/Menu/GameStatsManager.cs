using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI  ScoreText;
    public TextMeshProUGUI  ComboText;
    public TextMeshProUGUI  LivesText;
    public int score = 0;
    public int combo = 0;
    public int lives = 3;

    public bool isGameOver = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update(){
        ScoreText.text = "Score :" + score;
        ComboText.text = "Combo :" + combo;
        LivesText.text = "Live :" + lives;
    }

    public void OnMoleHit(int points)
    {
        score += points * (combo + 1); 
        combo++;
        UpdateDifficulty();
        Debug.Log($"Score: {score}, Combo: {combo}");
    }

    public void OnMoleMissed()
    {

        if (isGameOver) return;

        lives--;
        combo = 0;

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateDifficulty()
    {
        float difficultyFactor = 1f + (score / 50f); 
        MoleTracker.Instance.UpdateSpeedMultiplier(difficultyFactor);

        if (score % 100 == 0) 
        {
            MoleTracker.Instance.IncreaseMaxActiveMoles();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        MoleTracker.Instance.StopAllMoles();
    }

    public void RestartGame()
    {
        Debug.Log("Redémarrage du niveau !");
        
        score = 0;
        combo = 0;
        lives = 3;
        isGameOver = false;

        MoleTracker.Instance.ResetTracker();

        MoleTracker.Instance.StartSpawningMoles();
    }
}
