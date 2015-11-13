using UnityEngine;
using System.Collections;

public class PlayerPowerUpManager : MonoBehaviour
{
    private GameObject storedPowerUp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
    {

	}

    public void ActivatePowerUp(Transform player)
    {
        GameObject powerUpObject = GameObject.Instantiate(storedPowerUp);
        IPowerUp powerUp = powerUpObject.GetComponent("IPowerUp") as IPowerUp;
        powerUp.Activate(player);
    }

    public GameObject StoredPowerUp
    {
        set
        {
            storedPowerUp = value;

            if (storedPowerUp != null)
            {
                // TODO: set icon
            }
            else
            {
                // TODO: clear icon
            }
        }
    }
}
