using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates managed getters and setters for the UI elements.
/// </summary>
public class PlayerUi : MonoBehaviour
{
    public Text playerScore;
    public Text planetHp;
    public Image playerPowerUpIcon;

    public Sprite emptyPowerUpSprite;
    public Hp hp;
    public PlayerScore score;

	/// <summary>
    /// Initializes the callback functions required for this script.
    /// </summary>
	protected void Awake()
    {
        hp.UpdateHp += UpdateHpText;
        score.UpdateScore += UpdateScoreText;
    }

    /// <summary>
    /// Initializes the ui.
    /// </summary>
    protected void Start()
    {
        hp.OnUpdateHp();
        score.OnUpdateScore();
    }

    /// <summary>
    /// Callback for updating the planet HP text.
    /// </summary>
    /// <param name="currentHp">Current planet HP.</param>
    private void UpdateHpText(float currentHp)
    {
        PlanetHp = currentHp;
    }

    /// <summary>
    /// Callback for updating the players score text.
    /// </summary>
    /// <param name="currentScore">Current player score.</param>
    private void UpdateScoreText(int currentScore)
    {
        PlayerScore = currentScore;
    }

    /// <summary>
    /// Setter for the planets HP.
    /// </summary>
    private float PlanetHp
    {
        set { planetHp.text = value.ToString("##0\\%"); }
    }

    /// <summary>
    /// Setter and getter for player score.
    /// </summary>
    public int PlayerScore
    {
        get { return int.Parse(playerScore.text); }
        set { playerScore.text = value.ToString("000000"); }
    }

    /// <summary>
    /// Setter for powerup icon.
    /// </summary>
    public Sprite PlayerPowerUpIcon
    {
        set { playerPowerUpIcon.sprite = (value != null ? value : emptyPowerUpSprite); }
    }
}
