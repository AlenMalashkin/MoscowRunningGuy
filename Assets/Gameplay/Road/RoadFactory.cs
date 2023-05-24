using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadFactory : MonoBehaviour
{
	[SerializeField] private GameObject[] roadPrefabs;
	[SerializeField] private float spawnPos = 0;
	[SerializeField] private float roadLength = 60;

	private int startRoutes = 5;
	private List<GameObject> activeRoads = new List<GameObject>();
	private Player _player;
	
	
	[Inject]
	private void Construct(Player player)
	{
		_player = player;
	}
	
	private void Start()
	{
		for (int i = 0; i < startRoutes; i++)
		{
			if (i == 0)
				SpawnRoad(0);
			SpawnRoad(Random.Range(1, roadPrefabs.Length));
		}
	}

	private void Update()
	{
		if (_player.transform.position.z - 10 > spawnPos - (startRoutes * roadLength))
		{
			SpawnRoad(Random.Range(1, roadPrefabs.Length));
			DeleteRoad();
		}
	}

	private void SpawnRoad (int routeIndex)
	{
		GameObject newRoute = Instantiate(roadPrefabs[routeIndex], transform.forward * spawnPos, transform.rotation);
		activeRoads.Add(newRoute);
		spawnPos += roadLength;
	}

	private void DeleteRoad ()
	{
		Destroy(activeRoads[0]);
		activeRoads.RemoveAt(0);
	}
}
