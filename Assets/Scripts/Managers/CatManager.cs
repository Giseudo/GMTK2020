using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CatManager : MonoBehaviour {
    public static List<Cat> cats = new List<Cat>();
    public delegate void OnAddCat(Cat cat);
    public static OnAddCat onAddCat;
	public static CatManager Instance = null;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

    public void Start () {
        foreach (Cat cat in cats) {
            cat.Initialize();
            cat.onScare += OnCatScare;
        }
    }

    public void FeedCats () {
        foreach (Cat cat in cats) {
            cat.LookForFood();
        }
    }

    public void AddCat (Cat cat) {
        if (onAddCat != null) onAddCat(cat);

        cats.Add(cat);
    }

    public static Vector3 RandomPosition(Vector3 origin, float distance = 1f) {
        Vector2 random = Random.insideUnitCircle;
        Vector3 direction = new Vector3(random.x, 0f, random.y);

        if (Physics.Raycast(origin, direction, out RaycastHit raycastHit, distance))
            direction = (origin - raycastHit.point).normalized;

        Vector3 position = direction * distance;
        position += origin;

        NavMeshHit navmeshHit;
        NavMesh.SamplePosition(position, out navmeshHit, distance, 1);

        return navmeshHit.position;
    }

    public void OnCatScare(Cat cat) {
        SoundManager.Instance.Play("AngryCat");
    }

    #if UNITY_EDITOR
    void OnDrawGizmos() {
        foreach (Cat cat in cats) {
            Vector3 managerPos = transform.position;
            Vector3 catPos = cat.transform.position;
            float halfHeight = (managerPos.y - catPos.y) * .5f;
            Vector3 offset = Vector3.up * halfHeight;

            Handles.DrawBezier(
                managerPos,
                catPos,
                managerPos - offset,
                catPos + offset,
                Color.white,
                EditorGUIUtility.whiteTexture,
                1f
            );
        }
    }
    #endif
}
