using System;
using UnityEngine;

public class PlayerBoosts : MonoBehaviour
{
    public event Action<BoostType> OnBoostCollectedEvent;
    
    [SerializeField] private PlayerCollision playerCollision;

    private void OnEnable()
    {
        playerCollision.OnPlayerColliderHitEvent += PlayerColliderHit;
    }

    private void OnDisable()
    {
        playerCollision.OnPlayerColliderHitEvent -= PlayerColliderHit;
    }

    private void PlayerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Boost boost))
        {
            OnBoostCollectedEvent?.Invoke(boost.type);
            Destroy(boost.gameObject);
        }
    }
}
