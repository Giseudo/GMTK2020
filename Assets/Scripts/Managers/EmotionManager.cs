using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    public ObjectPooling attentionPool;
    public ObjectPooling lovePool;
	public static EmotionManager Instance = null;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
	}

    public GameObject GetLoveEmotion () {
        GameObject emotion = lovePool.GetObject();

        return emotion;
    }

    public GameObject GetAttentionEmotion () {
        GameObject attention = attentionPool.GetObject();

        return attention;
    }
}
