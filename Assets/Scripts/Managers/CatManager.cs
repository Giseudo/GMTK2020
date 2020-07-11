using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CatManager : MonoBehaviour {
    public static List<Cat> cats = new List<Cat>();

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
