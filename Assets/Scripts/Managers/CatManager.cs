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
    public delegate void OnRemoveCat(Cat cat);
    public static OnRemoveCat onRemoveCat;

    public static void EnableCat(int amount = 1) {
        int count = 0;
        Transform parent = GameObject.Find("Cats").transform;

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

    public static void DisableCat() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Cat")) {
            if (obj.activeInHierarchy) {
                obj.SetActive(false);
                return;
            }
        }
    }

    public static void AddCat (Cat cat) {
        cats.Add(cat);

        cat.data.hunger.RuntimeValue = cat.data.hunger.InitialValue;

        Material material = cat.GetComponentInChildren<Renderer>().sharedMaterial;
        material.SetFloat("_Hunger", Mathf.InverseLerp(0f, cat.data.hunger.InitialValue * 2, cat.data.hunger.InitialValue));

        if (onAddCat != null) onAddCat(cat);

        cat.Initialize();
    }

    public static void RemoveCat (Cat cat) {
        if (onRemoveCat != null) onRemoveCat(cat);

        cats.Remove(cat);
    }

    public static List<Cat> GetDeadCats () {
        List<Cat> deadCats = new List<Cat>();

        foreach (Cat cat in cats) {
            if (cat.Hunger >= 200f)
                deadCats.Add(cat);
        }

        return deadCats;
    }

    public static void FeedTheCats () {
        foreach (Cat cat in cats) {
            // cat.LookForFood();
        }
    }

    public static void Reset () {
        foreach (Cat cat in cats) {
            cat.data.hunger.RuntimeValue = cat.data.hunger.InitialValue;
        }
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

    /*#if UNITY_EDITOR
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
    */
}
