using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SquirrelAI : MonoBehaviour {

    public enum SquirrelState {
        WANDER,
        RUN,
        RETREAT,
        HIDE,
    }

    public float buffer = 8;
    public float dangerRange = 2;
    public float hideForSeconds = 8;
    private Vector2 origin;
    private SquirrelState state;
    private float speed = 13;
    private Stopwatch timer;
    private Vector2 wanderDirection;
    private int wanderCounter;
    private bool wandering;
    private GameObject dog, localTree;

    // Start is called before the first frame update
    void Start() {
        origin = transform.position;
        state = SquirrelState.WANDER;
        timer = new Stopwatch();
        wandering = false;
        wanderCounter = 20;

        dog = GameObject.FindGameObjectWithTag("Dog");
        dog.GetComponent<DogPriorities>().addNewObject(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(!GameObject.Find("CameraCart").GetComponent<CameraController>().currentlyInTransition()) {
            Vector2 dogLoc = dog.transform.position;
            float dist = Vector2.Distance(transform.position, dogLoc);

            switch(state) {
            case SquirrelState.WANDER:
                wander();
                if(dist <= dangerRange) {
                    state = SquirrelState.RUN;
                    goto case SquirrelState.RUN;
                }
                break;
            case SquirrelState.RUN:
                transform.position = Vector2.MoveTowards(transform.position, dogLoc, Time.deltaTime * -speed);
                transform.up = (Vector2)transform.position - dogLoc;
                if (dist > (dangerRange + buffer)) {
                    state = SquirrelState.RETREAT;
                    localTree = FindNearestByTag("Tree");
                    goto case SquirrelState.RETREAT;
                }
                break;
            case SquirrelState.RETREAT:
                Vector2 treeLoc = localTree.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, treeLoc, Time.deltaTime * speed);
                transform.up = treeLoc - (Vector2) transform.position;
                if (dist < (dangerRange + buffer)) {
                    state = SquirrelState.RUN;
                }
                else if(Vector2.Distance(transform.position, treeLoc) == 0) {
                    GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().removeObject(gameObject);
                    timer.Start();
                    state = SquirrelState.HIDE;
                }
                break;
            case SquirrelState.HIDE:
                if(timer.ElapsedMilliseconds > (hideForSeconds * 1000)) {
                    state = SquirrelState.WANDER;
                    timer.Stop();
                    timer.Reset();
                    GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().addNewObject(gameObject);
                }
                break;
            default:
                UnityEngine.Debug.LogError("Squirrel state invalid: " + state);
                break;
            }
        }
    }

    void wander() {
        if(wandering) {
            transform.position = new Vector3(transform.position.x + (wanderDirection.x * speed * 0.25f * Time.deltaTime), transform.position.y + (wanderDirection.y * speed * 0.5f * Time.deltaTime), 0);
            transform.up = wanderDirection;
            wanderCounter--;

            if(wanderCounter <= 0) {
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

    public GameObject FindNearestByTag(string tag) {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
// run away first, the outside of danger zone run towards nearest tree