using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour
{
    public delegate void OnScoreUpdateHandler(int points);
    public event OnScoreUpdateHandler UpdateScore;

    private int score;

    public void Start()
    {
        OnUpdateScore();
    }

    public void AddPoints(int pointsToAdd)
    {
        score += pointsToAdd;
        OnUpdateScore();
    }

    public void OnUpdateScore()
    {
        if (UpdateScore != null)
        {
            UpdateScore(score);
        }
    }
}
