using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private Player _target;

    [Inject]
    private void Construct(Player player)
    {
        _target = player;
    }
    
    private void Update()
    {
        Vector3 playerPosition = _target.transform.position;

        Vector3 newPosition = new Vector3(transform.position.x, playerPosition.y + offset.y, playerPosition.z + offset.z);

        transform.position = newPosition;
    }
}

