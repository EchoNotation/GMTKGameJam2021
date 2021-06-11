using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treat : MonoBehaviour
{
    private Vector2 destination;
    private float speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public void setTreatDestination(Vector2 destination) {
        this.destination = destination;
    }
}
