using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPriorities : MonoBehaviour {
    private List<GameObject> objects;
    private Vector2 previousLoc;
    private float distanceMod = 0.2f;
    private Hashtable basePriorities;

    // Start is called before the first frame update
    void Start() {
        objects = new List<GameObject>();
        basePriorities = new Hashtable();
        basePriorities.Add("Treat", 5f);
        basePriorities.Add("Neighbor", 2f);
        basePriorities.Add("Squirrel", 7f);
        basePriorities.Add("Goal", 0.5f);

        objects.Add(GameObject.FindGameObjectWithTag("Goal"));
    }

    // Update is called once per frame
    void Update() {
        GameObject objectOfInterest = findHighestPriority();
        Vector2 objectLoc = new Vector2(objectOfInterest.transform.position.x, objectOfInterest.transform.position.y);

        if(previousLoc != objectLoc) {
            GameObject.Find("Dog").GetComponent<PlayerMotion>().setTargetDestination(objectLoc);
            previousLoc = objectLoc;
        }
    }

    private GameObject findHighestPriority() {
        int maxIndex = -1;
        float maxPriority = -1;

        for(int i = 0; i < objects.Count; i++) {
            float dist = Vector2.Distance(transform.position, objects[i].transform.position);

            if(dist < 0.5) {
                dist = 0.5f;
            }

            string tag = objects[i].tag;
            float priority = ((1 / dist) * distanceMod) + (float) basePriorities[tag];
            Debug.Log("Tag: " + tag + " Priority: " + priority);

            if(maxIndex == -1) {
                maxIndex = 0;
                maxPriority = priority;
            }
            else if(priority > maxPriority) {
                maxIndex = i;
                maxPriority = priority;
            }
        }

        return objects[maxIndex];
    }

    public void addNewObject(GameObject obj) {
        objects.Add(obj);
    }

    public void removeObject(GameObject obj) {
        objects.Remove(obj);
    }
}
