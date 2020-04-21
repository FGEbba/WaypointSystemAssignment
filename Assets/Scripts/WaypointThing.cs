using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointThing : MonoBehaviour
{
    [SerializeField] private Color colour = Color.white;
    [SerializeField] private float sphereRadius = 0f;


    public void SetSphereRadius(float newSphereRadius)
    {
        sphereRadius = newSphereRadius;
    }



    public void SetColour(Color newColour)
    {
        colour = newColour;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = colour;
        Gizmos.DrawSphere(transform.position, sphereRadius);

    }


}
