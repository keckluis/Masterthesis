using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPath : MonoBehaviour
{
    public void OnDrawGizmosSelected()
    {
        transform.parent.GetComponent<EnemyFollowPath>().OnDrawGizmosSelected();
    }
}
