using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour
{
    public delegate void OnDeathHandler(Transform victim, Transform killer, bool timedOut);
    public event OnDeathHandler Death;

    public delegate void OnIntializeHandler();
    public event OnIntializeHandler Initialize;

    public SimplePool Pool { get; set; }
    public bool Dieing { get; private set; }

    public void Start()
    {
        OnInitialize();
    }

    public void OnInitialize()
    {
        if (Initialize != null)
        {
            Dieing = false;
            Initialize();
        }
    }

    public void OnDeath(Transform killer, bool timedOut)
    {
        if (Death != null && !Dieing)
        {
            Dieing = true;
            Death(transform, killer, timedOut);
        }
    }
}
