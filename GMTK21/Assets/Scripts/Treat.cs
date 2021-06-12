using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treat : MonoBehaviour
{
    private Vector2 destination;
    private float speed = 10;
    private bool treatSafety = true;

    private void Start() {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), destination) <= 0.1 && treatSafety) {
            //Debug.Log("Treat stopped moving");
            GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().addNewObject(gameObject);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            treatSafety = false;
        }
    }

    public void setTreatDestination(Vector2 destination) {
        this.destination = destination;
    }
}
