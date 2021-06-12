using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SquirrelAI : MonoBehaviour
{

    public enum SquirrelState {
        WANDER,
        RUN,
        HIDE,
    }

    public float buffer = 3;
    public float dangerRange = 5;
    private Vector2 origin;
    private SquirrelState state;
    private float speed = 9;
    private Stopwatch timer;
    private Vector2 wanderDirection;
    private int wanderCounter;
    private bool wandering;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        state = SquirrelState.WANDER;
        timer = new Stopwatch();
        wandering = false;
        wanderCounter = 20;

        GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().addNewObject(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dogLoc = GameObject.FindGameObjectWithTag("Dog").transform.position;

        if(Vector2.Distance(dogLoc, transform.position) <= dangerRange)
        {
            state = SquirrelState.RUN;
        }

        switch(state) {
            case SquirrelState.WANDER:
                wander();
                break;
            case SquirrelState.RUN:
                if (Vector2.Distance(transform.position, dogLoc) <= (dangerRange + buffer))
                {
                    //Move towards the dog-- Check to see if the dog is wet?
                    transform.position = Vector2.MoveTowards(transform.position, dogLoc, Time.deltaTime * -speed);
                }
                else
                {
                    state = SquirrelState.HIDE;
                }
                break;
            case SquirrelState.HIDE:
                transform.position = Vector2.MoveTowards(transform.position, FindNearestByTag("Tree").transform.position, Time.deltaTime);
                break;
            default:
                UnityEngine.Debug.LogError("Squirrel state invalid: " + state);
                break;
        }
    }

    void wander()
    {
        if (wandering)
        {
            transform.position = new Vector3(transform.position.x + (wanderDirection.x * speed * 0.5f * Time.deltaTime), transform.position.y + (wanderDirection.y * speed * 0.5f * Time.deltaTime), 0);
            wanderCounter--;

            if (wanderCounter <= 0)
            {
                wandering = false;
            }
        }
        else
        {
            float angle = UnityEngine.Random.Range(0, 359);
            wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            wanderCounter = UnityEngine.Random.Range(45, 125);
            wandering = true;
        }
    }

    public GameObject FindNearestByTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
// run away first, the outside of danger zone run towards nearest tree