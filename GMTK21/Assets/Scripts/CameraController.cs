using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

    private enum TransitionState {
        WAIT_TO_FOCUS_ON_HOUSE,
        NOT_IN_TRANSITION,
        TRANSITION_TO_HOUSE,
        FOCUS_ON_HOUSE,
        TRANSITION_FROM_HOUSE
    }

    public Camera cam;
    public GameObject person;
    private TransitionState state;
    private float xMin, xMax, yMin, yMax;
    private float cameraWidth, cameraHeight;
    private long timeToWaitBeforeMoving = 1000;
    private long timeToFocusOnHouse = 1000;
    private Vector3 previousPosition;
    private float cameraSpeed = 9f;
    private Vector2 houseLocation, origin;
    private Stopwatch timer;
    private bool inTransition;

    // Start is called before the first frame update
    void Start() {
        origin = new Vector2(transform.position.x, transform.position.y);
        //Calculate the min and max camera x positions from the size of the tilemap.
        GameObject tilemap = GameObject.Find("BaseTilemap");
        cameraWidth = cam.orthographicSize * cam.aspect * 2f;
        cameraHeight = cam.orthographicSize * 2f;

        xMin = Mathf.Ceil(tilemap.GetComponent<Tilemap>().CellToWorld(tilemap.GetComponent<Tilemap>().origin).x + (cameraWidth / 2) + 1);
        yMin = Mathf.Ceil(tilemap.GetComponent<Tilemap>().CellToWorld(tilemap.GetComponent<Tilemap>().origin).y + (cameraHeight / 2) + 1);

        int maxXTilespace = tilemap.GetComponent<Tilemap>().size.x;
        int maxYTilespace = tilemap.GetComponent<Tilemap>().size.y;

        //Debug.Log(cameraWidth);
        /*xMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(maxXTilespace, Mathf.FloorToInt(yMin), 0)).x - (cameraWidth / 2);
        xMax = Mathf.Floor(xMax);
        yMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(Mathf.FloorToInt(xMin), maxYTilespace, 0)).y - (cameraHeight / 2);
        yMax = Mathf.Floor(yMax);*/
        xMax = Mathf.Floor(xMin + maxXTilespace - cameraWidth - 1);
        yMax = Mathf.Floor(yMin + maxYTilespace - cameraHeight - 1);

        UnityEngine.Debug.Log("Camera xMin: " + xMin + " xMax: " + xMax + " yMin: " + yMin + " yMax: " + yMax);
        timer = new Stopwatch();
        timer.Start();

        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
        houseLocation = new Vector2(goal.transform.position.x, goal.transform.position.y);

        state = TransitionState.NOT_IN_TRANSITION;

        //Comment this line out if/when the transition on boot becomes annoying.
        //state = TransitionState.WAIT_TO_FOCUS_ON_HOUSE;
    }

    // Update is called once per frame
    void Update() {
        //inTransition = !(state == TransitionState.NOT_IN_TRANSITION);

        //Camera needs to be able to scroll horizontally only, but up to a point.
        if(state != TransitionState.NOT_IN_TRANSITION) {
            UpdateTransition();
        }
        else {
            //UnityEngine.Debug.Log("Running!");
            float personX = person.transform.position.x;
            float personY = person.transform.position.y;
            float cameraX = personX;
            float cameraY = personY;

            if(personX >= xMax) {
                cameraX = xMax;
            }
            else if(personX <= xMin) {
                cameraX = xMin;
            }

            if(personY >= yMax) {
                cameraY = yMax;
            }
            else if(personY <= yMin) {
                cameraY = yMin;
            }

            transform.position = new Vector3(cameraX, cameraY, 0);
        }
    }

    void UpdateTransition() {
        //UnityEngine.Debug.Log(state);
        inTransition = true;
        if(state == TransitionState.WAIT_TO_FOCUS_ON_HOUSE) {
            if(timer.ElapsedMilliseconds >= timeToWaitBeforeMoving) {
                state = TransitionState.TRANSITION_TO_HOUSE;
                timer.Stop();
                timer.Reset();
            }
        }
        else if(state == TransitionState.FOCUS_ON_HOUSE) {
            //UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
            if(timer.ElapsedMilliseconds >= timeToFocusOnHouse) {
                state = TransitionState.TRANSITION_FROM_HOUSE;
                timer.Stop();
                timer.Reset();
            }
        }
        else {
            if(state == TransitionState.TRANSITION_TO_HOUSE) {
                Vector2 tgtPosition = Vector2.MoveTowards(transform.position, houseLocation, cameraSpeed * Time.deltaTime);
                Vector3 finalPosition = new Vector3(tgtPosition.x, tgtPosition.y, 0);

                if(finalPosition.x <= xMin) finalPosition.x = xMin;
                if(finalPosition.x >= xMax) finalPosition.x = xMax;
                if(finalPosition.y <= yMin) finalPosition.y = yMin;
                if(finalPosition.y >= yMax) finalPosition.y = yMax;

                transform.position = finalPosition;

                if(Vector3.Distance(finalPosition, previousPosition) <= 0.001) {
                    state = TransitionState.FOCUS_ON_HOUSE;
                    timer.Start();
                }
                previousPosition = finalPosition;
            }
            else {
                //TRANSITION_FROM_HOUSE
                Vector2 tgtPosition = Vector2.MoveTowards(transform.position, origin, cameraSpeed * Time.deltaTime);
                Vector3 finalPosition = new Vector3(tgtPosition.x, tgtPosition.y, 0);

                if(finalPosition.x <= xMin) finalPosition.x = xMin;
                if(finalPosition.x >= xMax) finalPosition.x = xMax;
                if(finalPosition.y <= yMin) finalPosition.y = yMin;
                if(finalPosition.y >= yMax) finalPosition.y = yMax;

                transform.position = finalPosition;

                if(Vector3.Distance(finalPosition, previousPosition) <= 0.001) {
                    state = TransitionState.NOT_IN_TRANSITION;
                    inTransition = false;
                }
                previousPosition = finalPosition;
            }
        }
    }

    public bool currentlyInTransition() {
        return inTransition;
    }
}
