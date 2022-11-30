using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> Prefabs;

    public List<Vector2> Blocked;

    public GameObject EdgeFog;

    public Transform EdgeFogHolder;

    public int EmptynessFactor;

    public void CreateMap()
    {
        //delete old objects
        DeleteMap();

        //delete old fog
        DeleteFog();


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
                    GameObject obj = Instantiate(Prefabs[objType]);
                    obj.transform.parent = gameObject.transform;
                    
                    obj.transform.localPosition = new Vector3(
                        x + Random.Range(-25, 25), 
                        obj.transform.localPosition.y, 
                        z + Random.Range(-25, 25));

                    obj.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 359), 0f);

                    float scale = Random.Range(1f, 1.5f);
                    obj.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }

        Debug.Log(transform.childCount + " map objects created."); 

        //create and position edge fog
        for (int x = 0; x <= 6000; x += 100){
            GameObject fogB = Instantiate(EdgeFog);
            GameObject fogT = Instantiate(EdgeFog);

            fogB.transform.parent = EdgeFogHolder;
            fogT.transform.parent = EdgeFogHolder;

            fogB.transform.localPosition = new Vector3(x, 0f, 0f);
            fogT.transform.localPosition = new Vector3(x, 0f, 6000f);

            fogB.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            fogT.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);;
        }

        for (int z = 0; z <= 6000; z += 100){
            GameObject fogL = Instantiate(EdgeFog);
            GameObject fogR = Instantiate(EdgeFog);

            fogL.transform.parent = EdgeFogHolder;
            fogR.transform.parent = EdgeFogHolder;

            fogL.transform.localPosition = new Vector3(0f, 0f, z);
            fogR.transform.localPosition = new Vector3(6000f, 0f, z);
        }
    }

    public void DeleteMap()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void DeleteFog(){
        int childCount = EdgeFogHolder.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(EdgeFogHolder.GetChild(0).gameObject);
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
