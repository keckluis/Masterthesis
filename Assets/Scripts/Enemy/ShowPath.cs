using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPath : MonoBehaviour
{
#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        transform.GetChild(0).GetComponent<EnemyAI>().OnDrawGizmosSelected();
    }
#endif
}
