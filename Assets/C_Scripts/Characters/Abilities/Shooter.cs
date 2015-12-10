using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attach to entities that are capable of shooting bullets.
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AudioSource))]
public class Shooter : MonoBehaviour
{
    [Header("Bullet Data")]
	public AudioClip shootSound; 	
    public SimplePool bulletPool;   // TODO: place in weapon class
    public PlayerUi ui;
    public bool shoot;

    public float maxRateOfFire = 100;
    public float maxScatterAngle = 45.0f;

    public float initialRateOfFire = 2.0f;
    public float initialScatterAngle = 0.0f;

    public List<Transform> spawnPositions;

    private AudioSource audioSource; // Plays bullet shoot sound
    private float rateOfFire;
    private float scatterAngle;    
    private int spawnPositionIndex;
    private float timer = 0;

    #region Unity Life Cycle Methods

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Awake()
    {
		audioSource = GetComponent<AudioSource>();

        rateOfFire = initialRateOfFire;
        scatterAngle = initialScatterAngle;

        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions to place bullets at: " + name);
        }
    }

    /// <summary>
    /// Handle input and spawn bullets. TODO: Move movement logic to seperate script for moving any character and
    /// move input logic to seperate script.
    /// </summary>
    protected void Update()
    {
        if (!Lock)
        {
            if (timer >= 1.0f / rateOfFire)
            {
                if (shoot)
                {
                    GameObject spawnedBullet = bulletPool.GetAvailableObject();
                    spawnedBullet.GetComponent<Bullet>().Initialize(this, scatterAngle);
                    spawnedBullet.transform.position = spawnPositions[spawnPositionIndex].position;
                    spawnedBullet.layer = gameObject.layer;

                    audioSource.PlayOneShot(shootSound);

                    spawnPositionIndex = (spawnPositionIndex + 1 >= spawnPositions.Count ? 0 : spawnPositionIndex + 1);

                    timer = 0;
                }
            }
        }

        timer += Time.deltaTime;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Getter and Setter for preventing this object from shooting.
    /// </summary>
    public bool Lock { get; set; }

    /// <summary>
    /// Rate of fire for the players weapon.
    /// </summary>
    public float RateOfFire
    {
        get
        {
            return rateOfFire;
        }
        set
        {
            rateOfFire = Mathf.Clamp(value, initialRateOfFire, maxRateOfFire);
        }
    }

    /// <summary>
    /// Angle the shots scatter in.
    /// </summary>
    public float ScatterAngle
    {
        get
        {
            return scatterAngle;
        }
        set
        {
            scatterAngle = Mathf.Clamp(value, initialScatterAngle, maxScatterAngle);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds points to the players score.
    /// </summary>
    /// <param name="points">Points to add</param>
    public void AddScore(int points)
    {
        ui.PlayerScore += points;
    }

    #endregion
}
