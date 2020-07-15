using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    List<GameObject> objects;

    void Start() {
        objects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++) {
            AddObject();
        }
    }

    public GameObject GetObject() {
        for (int i = 0; i < objects.Count; i++) {
            if (!objects[i].activeInHierarchy)
            return objects[i];
        }

        return AddObject();
    }

    public GameObject AddObject() {
        GameObject obj = (GameObject)Instantiate(prefab, transform);
        obj.SetActive(false);
        objects.Add(obj);

        return obj;
    }
}
