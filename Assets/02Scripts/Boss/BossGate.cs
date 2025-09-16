using System;
using UnityEngine;

public class BossGate : MonoBehaviour
{
    [SerializeField] private ColliderTrigger trigger;

    private void Awake()
    {
        trigger.OnPlayerEnterTrigger += CloseGate;
    }

    private void CloseGate(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        trigger.OnPlayerEnterTrigger -= CloseGate;
    }
}
