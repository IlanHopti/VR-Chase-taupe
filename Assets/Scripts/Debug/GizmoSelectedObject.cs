using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSelectedObject : MonoBehaviour
{
    public Transform attachTransform;

    void OnDrawGizmosSelected()
    {
        if (attachTransform != null)
        {
                //Z
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(attachTransform.position, attachTransform.position + attachTransform.forward * 0.2f);
                 //Y
            Gizmos.color = Color.green;
            Gizmos.DrawLine(attachTransform.position, attachTransform.position + attachTransform.up * 0.2f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(attachTransform.position, attachTransform.position + attachTransform.right * 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(attachTransform.position, 0.01f);
        }
    }
}
