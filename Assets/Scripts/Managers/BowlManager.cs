using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BowlManager : MonoBehaviour {
    public static List<Bowl> bowls = new List<Bowl>();
    public delegate void OnAddBowl(Bowl bowl);
    public static OnAddBowl onAddBowl;
    public delegate void OnRemoveBowl(Bowl bowl);
    public static OnRemoveBowl onRemoveBowl;

    public void Start () {
        GameManager.Instance.onRoundStart += PlaceFood;
    }

    public static void EnableBowl(int amount) {
        int count = 0;
        Transform parent = GameObject.Find("Bowls").transform;

        for (int i = 0; i < parent.transform.childCount; i++) {
            if (count >= amount) return;

            Transform child = parent.transform.GetChild(i);

            if (child == null) return;

            if (!child.gameObject.activeInHierarchy) {
                child.gameObject.SetActive(true);
                count++;
            }
        }
    }

    public static void DisableBowl() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bowl")) {
            if (obj.activeInHierarchy) {
                obj.SetActive(false);
                return;
            }
        }
    }

    public static void AddBowl (Bowl bowl) {
        bowls.Add(bowl);

        if (onAddBowl != null) onAddBowl(bowl);

        if (!Application.IsPlaying(bowl.gameObject)) return;

        bowl.Initialize();
    }

    public static void RemoveBowl (Bowl bowl) {
        if (onRemoveBowl != null) onRemoveBowl(bowl);

        bowls.Remove(bowl);
    }

    public static void PlaceFood (int round) {
        foreach (Bowl bowl in bowls) {
            bowl.PlaceFood();
        }
    }

    public static float InitialMeal () {
        float amount = 0f;

        foreach (Bowl bowl in bowls) {
            amount += bowl.InitialMeal;
        }

        return Mathf.Round(amount);
    }

    public static float RemainingMeal () {
        float amount = 0f;

        foreach (Bowl bowl in bowls) {
            amount += bowl.RemainingMeal;
        }

        return Mathf.Round(amount);
    }

    public static Bowl GetClosestBowl(Cat cat) {
        Vector3 position = cat.transform.position;
        Bowl closest = null;
        float previous = 0f;

        foreach (Bowl bowl in bowls) {
            float distance = (bowl.transform.position - position).magnitude;

            if (!cat.CanEat(bowl)) continue;

            if (previous == 0f || previous > distance) {
                previous = distance;
                closest = bowl;
            }
        }

        return closest;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos() {
        foreach (Bowl bowl in bowls) {
            Vector3 managerPos = transform.position;
            Vector3 bowlPos = bowl.transform.position;
            float halfHeight = (managerPos.y - bowlPos.y) * .5f;
            Vector3 offset = Vector3.up * halfHeight;

            Handles.DrawBezier(
                managerPos,
                bowlPos,
                managerPos - offset,
                bowlPos + offset,
                Color.green,
                EditorGUIUtility.whiteTexture,
                1f
            );
        }
    }
    #endif
}