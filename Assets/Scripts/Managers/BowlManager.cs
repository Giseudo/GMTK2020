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
            bowl.data = Instantiate(bowl.data);
            bowl.data.foodAmount = Instantiate(bowl.data.foodAmount);
        }
    }
    public static Bowl GetClosestBowl(Cat cat) {
        Vector3 position = cat.transform.position;
        Bowl closest = null;
        float previous = 0f;

        foreach (Bowl bowl in bowls) {
            float distance = (bowl.transform.position - position).magnitude;

            // Has no food on it
            if (bowl.FoodAmount <= 0f) continue;
            // Is a cat eating it?
            if (bowl.feedingCat != null && !cat.IsThief) continue;
            // Was I eating it just before?
            if (bowl == cat.previousBowl) continue;
            // Found one
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