using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EdgeFogGenerator : MonoBehaviour
{
    public GameObject EdgeFog;

    public void CreateFog()
    {
        //delete old fog
        DeleteFog();

        //create and position edge fog
        for (int x = 0; x <= 6000; x += 100)
        {
            GameObject fogB = Instantiate(EdgeFog);
            GameObject fogT = Instantiate(EdgeFog);

            fogB.transform.parent = transform;
            fogT.transform.parent = transform;

            fogB.transform.localPosition = new Vector3(x, 0f, 100f);
            fogT.transform.localPosition = new Vector3(x, 0f, 5900f);

            fogB.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            fogT.transform.localEulerAngles = new Vector3(-90f, 0f, 0f); ;
        }

        for (int z = 0; z <= 6000; z += 100)
        {
            GameObject fogL = Instantiate(EdgeFog);
            GameObject fogR = Instantiate(EdgeFog);

            fogL.transform.parent = transform;
            fogR.transform.parent = transform;

            fogL.transform.localPosition = new Vector3(100f, 0f, z);
            fogR.transform.localPosition = new Vector3(5900f, 0f, z);
        }
    }

    public void DeleteFog()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    [CustomEditor(typeof(EdgeFogGenerator))]
    public class EdgeFogInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EdgeFogGenerator efg = (EdgeFogGenerator)target;

            if (GUILayout.Button("Create Edge Fog"))
            {
                efg.CreateFog();
            }

            if (GUILayout.Button("Delete Edge Fog"))
            {
                efg.DeleteFog();
            }
        }
    }
}
