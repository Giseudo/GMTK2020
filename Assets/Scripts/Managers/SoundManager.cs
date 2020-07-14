using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [Serializable]
    public struct SoundClip {
        public string name;
        public AudioClip clip;
    }
    public List<SoundClip> soundClips;

	public static SoundManager Instance = null;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
	}

    public void Play (string name, float volume = 1f) {
        AudioClip audioClip = soundClips.Find(clip => clip.name == name).clip;

        if (audioClip == null) {
            Debug.LogWarning(string.Format("Could not found clip with name \"{0}\"", name));
            return;
        }

        GetComponent<AudioSource>().PlayOneShot(audioClip, volume);
    }
}
