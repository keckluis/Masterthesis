using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
    public GameObject Seagulls;
    public int Height = 40;
    public float Speed = 0.1f;
    private float currentDirection;

    void FixedUpdate()
    {
        if (transform.childCount == 0)
        {
            GameObject gulls = Instantiate(Seagulls);
            gulls.transform.parent = transform;

            transform.localPosition = new Vector3(0f, Height, 0f);
            transform.localEulerAngles = new Vector3(0, Random.Range(0, 359), 0);

            int direction = Random.Range(0, 1);

            if (direction == 0)
            {
                gulls.transform.localPosition = new Vector3(-4250f, 0f, 0f);
                currentDirection = 1f;
            }
            else
            {
                gulls.transform.localPosition = new Vector3(4250f, 0f, 0f);
                currentDirection = -1f;
            }

            gulls.transform.LookAt(transform.position); 
        }
        else
        {
            Transform gulls = transform.GetChild(0);
            gulls.localPosition = new Vector3(gulls.localPosition.x + (currentDirection * Speed), 0f, 0f);

            if (currentDirection < 0 && gulls.localPosition.x < -4250f)
            {
                Destroy(gulls.gameObject);
            }
            else if (currentDirection > 0 && gulls.localPosition.x > 4250f)
            {
                Destroy(gulls.gameObject);
            }
        }
    }
}
