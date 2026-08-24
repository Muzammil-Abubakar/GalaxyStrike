using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        score = 0;
        UpdateScoreText();
    }

    public void IncreaseScore()
    {
        score += 100;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}