using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> Prefabs;

    public List<Vector2> Blocked;

    public void CreateMap()
    {
        DeleteMap();

        for (int x = 0; x < 6000; x += 100)
        {
            for (int z = 0; z < 6000; z += 100)
            {
                if (Blocked.Contains(new Vector2(x, z)))
                    continue;

                int objType = Random.Range(0, Prefabs.Count + 100);

                if (objType < Prefabs.Count)
                {
                    GameObject obj = Instantiate(Prefabs[objType]);
                    obj.transform.parent = gameObject.transform;
                    obj.transform.localPosition = new Vector3(x, 0f, z);
                    obj.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 359), 0f);
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
        }
    }

    public void OnDrawGizmosSelected()
    {
        for (int x = -3000; x < 3000; x += 100)
        {
            for (int z = -3000; z < 3000; z += 100)
            {
                Gizmos.color = new Color(1f, 0.5f, 0f);
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
