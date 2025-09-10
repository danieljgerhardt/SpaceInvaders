using UnityEngine;
using TMPro;
public class GameManagerScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text winStatusText;

    public float scorePerAlien = 10f;
    private float score = 0f;
    public void IncreaseScore()
    {
        score += scorePerAlien;
        scoreText.text = "Score: " + score;
    }

    public void LoseLife(int lives)
    {
        if (lives <= 0)
        {
            SetWinStatusToLose();
        }
        livesText.text = "Lives: " + lives;
    }

    public void SetWinStatusToLose()
    {
        SetWinStatus("You Lose!");
    }
    public void SetWinStatusToWin()
    {
        SetWinStatus("You Win!");
    }

    private void SetWinStatus(string status)
    {
        winStatusText.text = status;
    }
}
