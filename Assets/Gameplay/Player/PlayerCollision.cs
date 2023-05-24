using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	public event Action<ControllerColliderHit> OnPlayerColliderHitEvent;
	
	private void OnControllerColliderHit(ControllerColliderHit other)
	{
		OnPlayerColliderHitEvent?.Invoke(other);
	}
}