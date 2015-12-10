using UnityEngine;
using System.Collections;
using System;

public class Hp : MonoBehaviour
{
    public delegate void OnHpUpdateHandler(float currentHp);
    public event OnHpUpdateHandler UpdateHp;

    public float initialHp = 100.0f;
    public float CurrentHp { get; private set; }

    private DeathTrigger deathTrigger;

	// Use this for initialization
	void Awake()
    {
        ResetHp();

        deathTrigger = GetComponent<DeathTrigger>();
        deathTrigger.Initialize += ResetHp;
	}

    private void ResetHp()
    {
        CurrentHp = initialHp; ;
    }

    public void ReduceHp(Transform agressor, float value)
    {
        if (CurrentHp > 0)
        {
            CurrentHp -= value;
            
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                OnUpdateHp();
                deathTrigger.OnDeath(agressor, false);
            }
            else
            {
                OnUpdateHp();
            }
        }
    }

    public void OnUpdateHp()
    {
        if (UpdateHp != null)
        {
            UpdateHp(CurrentHp);
        }
    }
}
