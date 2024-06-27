using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCenterOfMass : MonoBehaviour
{
    public Vector3 centerOfMass;

    private void Update()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
