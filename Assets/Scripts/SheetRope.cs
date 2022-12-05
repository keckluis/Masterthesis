using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetRope : MonoBehaviour
{
    public GameObject RopePrefab;
    public int Size;

    private List<GameObject> RopeSegments = new List<GameObject>();

    private void Start()
    {
        CreateRope();
    }

    void CreateRope()
    {
        if (transform.childCount > 0)
            DeleteRope();

        for (int i = 0; i < Size; i++)
        {
            GameObject rs = Instantiate(RopePrefab);

            if (i == 0)
                rs.transform.parent = transform;
            else
            {
                rs.transform.localPosition = new Vector3(0f, 0.2f, 0f);
                //rs.transform.parent = RopeSegments[i - 1].transform;
                //rs.GetComponent<HingeJoint>().connectedBody = RopeSegments[i - 1].GetComponent<Rigidbody>();
            }
             
            RopeSegments.Add(rs);
        }
    }

    void DeleteRope()
    {
        Destroy(transform.GetChild(0));
        RopeSegments.Clear();
    }
}
