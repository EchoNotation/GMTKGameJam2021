using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum NeighborState {
    HAS_NOT_PET_DOG,
    PETTING_DOG,
    HAS_PET_DOG
}

public class Neighbor : MonoBehaviour {
    private Vector2 origin;
    private NeighborState state;
    private float speed = 3;
    private int petDuration = 1000;
    private float petRange = 8;
    private Stopwatch timer;
    private Vector2 wanderDirection;
    private int wanderCounter;
    private bool wandering;

    // Start is called before the first frame update
    void Start() {
        origin = transform.position;
        state = NeighborState.HAS_NOT_PET_DOG;
        timer = new Stopwatch();
        wandering = false;
        wanderCounter = 60;

        GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().addNewObject(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(!GameObject.Find("CameraCart").GetComponent<CameraController>().currentlyInTransition()) {
            Vector2 dogLoc = GameObject.FindGameObjectWithTag("Dog").transform.position;

            if(state == NeighborState.HAS_NOT_PET_DOG && Vector2.Distance(transform.position, dogLoc) <= petRange) {
                //Move towards the dog-- Check to see if the dog is wet?
                Vector2 newLoc = Vector2.MoveTowards(transform.position, dogLoc, Time.deltaTime * speed);
                transform.position = new Vector3(newLoc.x, newLoc.y, 0);
                transform.up = newLoc - (Vector2) transform.position;
            }
            else if(state == NeighborState.HAS_NOT_PET_DOG) {
                wander();
            }
            else if(state == NeighborState.HAS_PET_DOG) {
                //Return to origin? Wander?
                /*Vector2 newLoc = Vector2.MoveTowards(transform.position, origin, Time.deltaTime * speed * 0.5f);
                transform.position = new Vector3(newLoc.x, newLoc.y, 0);*/
                wander();
            }
            else {
                //Neighbor is currently petting dog... Need to track to ensure neighbor remains next to dog?
                //Notify dog that it is being pet so it stops moving?
                if(timer.ElapsedMilliseconds >= petDuration) {
                    //Done petting dog.
                    state = NeighborState.HAS_PET_DOG;
                    timer.Stop();
                    GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().setIsBeingPet(false);
                    GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().removeObject(gameObject);
                }
            }
        }
    }

    void wander() {
        if(wandering) {
            transform.position = new Vector3(transform.position.x + (wanderDirection.x * speed * 0.5f * Time.deltaTime), transform.position.y + (wanderDirection.y * speed * 0.5f * Time.deltaTime), 0);
            wanderCounter--;
            transform.up = wanderDirection;

            if (wanderCounter <= 0) {
                wandering = false;
            }
        }
        else {
            float angle = UnityEngine.Random.Range(0, 359);
            wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            wanderCounter = UnityEngine.Random.Range(45, 125);
            wandering = true;
        }
    }

    public void beginPettingDog() {
        state = NeighborState.PETTING_DOG;
        timer.Start();
    }
}
