using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomSpawn : MonoBehaviour
{
	public AudioClip[] clips;
	private AudioSource source;
    [Range(0.1f, 3.0f)]
    public float minPitch;
    [Range(0.1f, 3.0f)]
    public float maxPitch;

	void Awake ()
	{
		source = gameObject.GetComponent<AudioSource> ();

		if (clips.Length == 0)
		{
			return;
		}

		else
		{
			PlayRandom ();
		}
	}

	void PlayRandom()
	{
		int clipToPlay = Random.Range (0, clips.Length);
		source.clip = clips [clipToPlay];
		source.pitch = Random.Range (minPitch, maxPitch);
		source.Play ();
		Destroy (gameObject, source.clip.length);
	}
}
