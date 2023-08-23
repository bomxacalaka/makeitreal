using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    // Variable for zooming
    public float zoom = 10f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        newPos += offset;

        // Zooming
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
    }
}
