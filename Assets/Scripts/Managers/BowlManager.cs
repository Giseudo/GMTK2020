using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BowlManager : MonoBehaviour {
    public static List<Bowl> bowls = new List<Bowl>();

    public void Start () {
        foreach (Bowl bowl in bowls) {
            bowl.Initialize();
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
                Color.white,
                EditorGUIUtility.whiteTexture,
                1f
            );
        }
    }
    #endif
}