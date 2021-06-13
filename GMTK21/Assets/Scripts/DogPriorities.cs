using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DogPriorities : MonoBehaviour {
    private List<GameObject> objects = new List<GameObject>();
    private Vector2 previousLoc;
    private float distanceMod = 1f;
    private Hashtable basePriorities;
    private bool isBeingPet;

    // Start is called before the first frame update
    void Start() {
        basePriorities = new Hashtable();
        isBeingPet = false;
        basePriorities.Add("Treat", 3f);
        basePriorities.Add("Neighbor", 2f);
        basePriorities.Add("Squirrel", 7f);
        basePriorities.Add("Goal", 0.5f);

        objects.Add(GameObject.FindGameObjectWithTag("Goal"));
    }

    // Update is called once per frame
    void Update() {
        if(!GameObject.Find("CameraCart").GetComponent<CameraController>().currentlyInTransition()) {
            if(isBeingPet) {
                gameObject.GetComponent<PlayerMotion>().setSpeed(0);
            }
            else {
                GameObject objectOfInterest = findHighestPriority();
                Vector2 objectLoc = new Vector2(objectOfInterest.transform.position.x, objectOfInterest.transform.position.y);

                if(previousLoc != objectLoc) {
                    GameObject.Find("Dog").GetComponent<PlayerMotion>().setTargetDestination(objectLoc);
                    previousLoc = objectLoc;
                }
            }
        }
    }

    private GameObject findHighestPriority() {
        int maxIndex = -1;
        float maxPriority = -1;
        GameObject[] temp = objects.ToArray();
        //Debug.Log(temp.Length);

        for(int i = 0; i < temp.Length; i++) {
            float dist = Vector2.Distance(transform.position, temp[i].transform.position);

            if(dist < 0.5) {
                dist = 0.5f;
            }

            string tag = temp[i].tag;

            if(tag == "Squirrel" || tag == "Neighbor") {
                if(dist > 13) {
                    continue;
                }
            }

            float priority = ((1 / Mathf.Pow(dist, 2)) * distanceMod) * (float) basePriorities[tag];
            //Debug.Log("Tag: " + tag + " Priority: " + priority);

            if(maxIndex == -1) {
                maxIndex = 0;
                maxPriority = priority;
            }
            else if(priority > maxPriority) {
                maxIndex = i;
                maxPriority = priority;
            }
        }

        float dogSpeedPercent = maxPriority / 10f;
        gameObject.GetComponent<PlayerMotion>().setSpeed(dogSpeedPercent);

        return temp[maxIndex];
    }

    public void addNewObject(GameObject obj) {
        //Debug.Log("Added " + obj.tag);
        objects.Add(obj);
    }

    public void removeObject(GameObject obj) {
        objects.Remove(obj);
    }

    public void setIsBeingPet(bool newStatus) {
        isBeingPet = newStatus;
    }
}
