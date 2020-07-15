using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [Serializable]
    public class SoundClip {
        public string name;
        public AudioClip clip;
        [NonSerialized] public float lastPlayedTime;
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
        SoundClip sound = soundClips.Find(clip => clip.name == name);

        if (sound == null) {
            Debug.LogWarning(string.Format("Could not found clip with name \"{0}\"", name));
            return;
        }

        if (sound.lastPlayedTime + 2f > Time.unscaledTime)
            return;

        sound.lastPlayedTime = Time.unscaledTime;

        GetComponent<AudioSource>().PlayOneShot(sound.clip, volume);
    }
}
