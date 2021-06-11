using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum NeighborState {
    HAS_NOT_PET_DOG,
    PETTING_DOG,
    HAS_PET_DOG
}

public class Neighbor : MonoBehaviour
{
    private Vector2 origin;
    private NeighborState state;
    private float speed = 3;
    private int petDuration = 1000;
    private Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        state = NeighborState.HAS_NOT_PET_DOG;
        timer = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == NeighborState.HAS_NOT_PET_DOG) {
            //Move towards the dog-- Check to see if the dog is wet?
            Vector2 dogLoc = GameObject.FindGameObjectWithTag("Dog").transform.position;
            Vector2 newLoc = Vector2.MoveTowards(transform.position, dogLoc, Time.deltaTime * speed);
            transform.position = new Vector3(newLoc.x, newLoc.y, 0);
        }
        else if(state == NeighborState.HAS_PET_DOG){
            //Return to origin?
            Vector2 newLoc = Vector2.MoveTowards(transform.position, origin, Time.deltaTime * speed * 0.5f);
            transform.position = new Vector3(newLoc.x, newLoc.y, 0);
        }
        else {
            //Neighbor is currently petting dog... Need to track to ensure neighbor remains next to dog?
            if(timer.ElapsedMilliseconds >= petDuration) {
                //Done petting dog.
                state = NeighborState.HAS_PET_DOG;
                timer.Stop();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.CompareTag("Dog")) {
            state = NeighborState.PETTING_DOG;
            timer.Start();
        }
    }
}
