using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPath : MonoBehaviour
{
    public void OnDrawGizmosSelected()
    {
        transform.GetChild(0).GetComponent<EnemyAI>().OnDrawGizmosSelected();
    }
}
