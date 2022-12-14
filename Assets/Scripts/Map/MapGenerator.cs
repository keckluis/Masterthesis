using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> Prefabs;
    public List<GameObject> Colliders;

    public List<Vector2> Blocked;

    public int EmptynessFactor;

    public Transform MapColliders;

    public void CreateMap()
    {
        //delete old objects
        DeleteMap();

        //create and position map objects
        for (int x = 100; x < 6000; x += 100)
        {
            for (int z = 100; z < 6000; z += 100)
            {
                if (Blocked.Contains(new Vector2(x, z)))
                    continue;

                int objType = Random.Range(0, Prefabs.Count + EmptynessFactor);

                if (objType < Prefabs.Count)
                {
                    GameObject obj = PrefabUtility.InstantiatePrefab(Prefabs[objType]) as GameObject;
                    obj.name = obj.name + " ("+ x / 100 + ", " + z / 100 + ")";
                    obj.transform.parent = gameObject.transform;
                    
                    obj.transform.localPosition = new Vector3(
                        x + Random.Range(-25, 25), 
                        obj.transform.localPosition.y, 
                        z + Random.Range(-25, 25));

                    obj.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 359), 0f);

                    float scale = Random.Range(1f, 1.5f);
                    obj.transform.localScale = new Vector3(scale, scale, scale);

                    GameObject col = PrefabUtility.InstantiatePrefab(Colliders[objType]) as GameObject;
                    col.name = col.name + " (" + x / 100 + ", " + z / 100 + ")";
                    col.transform.parent = MapColliders;
                    col.transform.position = obj.transform.position;
                    col.transform.rotation = obj.transform.rotation;
                    col.transform.localScale = obj.transform.localScale;
                }
            }
        }

        Debug.Log(transform.childCount + " map objects created.");     
    }

    public void DeleteMap()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
            DestroyImmediate(MapColliders.GetChild(0).gameObject);
        }
    }

    public void OnDrawGizmosSelected()
    {
        for (int x = -2900; x < 3000; x += 100)
        {
            for (int z = -2900; z < 3000; z += 100)
            {
                if (Blocked.Contains(new Vector2(x + 3000, z + 3000)))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawSphere(new Vector3(x, 0f, z), 5);
            }
        }
    }

    [CustomEditor(typeof(MapGenerator))]
    public class MapInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapGenerator mg = (MapGenerator)target;
            if (GUILayout.Button("Create Map Objects"))
            {
                mg.CreateMap();
            }

            if (GUILayout.Button("Delete Map Objects"))
            {
                mg.DeleteMap();
            }
        }
    }
}
