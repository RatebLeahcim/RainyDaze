using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerCollision : MonoBehaviour
{
    private void Awake()
    {
        //Get the path of the polygon collider
        PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();
        Vector2[] path = polygonCollider2D.points;

        //Create a new edge collider and set the path to the path of the polygon collider
        EdgeCollider2D edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider2D.points = path;

        //Destroy the polygon collider
        Destroy(polygonCollider2D);
    }
}
