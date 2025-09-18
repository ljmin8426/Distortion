using System;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public event EventHandler OnPlayerEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }
}
