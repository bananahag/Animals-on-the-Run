using UnityEngine;
using System.Collections;

public class AudioAmbienceFiller : MonoBehaviour
{
	public float minTime;
	public float maxTime;
	public Object prefabToSpawn;
	public float borderX;
	public float borderZ;

	void Awake()
	{
		StartCoroutine (SpawnPrefabs ());
	}

	IEnumerator SpawnPrefabs()
	{
		yield return new WaitForSeconds(Random.Range(minTime, maxTime));
		Vector3 pos = new Vector3 (Random.Range (-borderX, borderX), 0f, Random.Range (-borderZ, borderZ));
		pos = pos + transform.position;
		Instantiate (prefabToSpawn, pos, transform.rotation);
		StartCoroutine (SpawnPrefabs());
	}
}
