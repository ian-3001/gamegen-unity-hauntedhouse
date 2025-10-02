using System;
using UnityEngine;

public class IntroCoin : MonoBehaviour
{
    [SerializeField] private IntroCoinManager _coinManager;
    
    private void OnTriggerEnter(Collider other)
    {
        _coinManager.AddCoin();
        Destroy(gameObject);
    }

    private void Reset()
    {
        if (_coinManager == null)
        {
            _coinManager = FindAnyObjectByType<IntroCoinManager>();
        }
    }
}
