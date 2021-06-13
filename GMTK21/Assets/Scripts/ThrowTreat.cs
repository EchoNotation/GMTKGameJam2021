using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ThrowTreat : MonoBehaviour
{
    private double throwRange = 5;
    private Stopwatch timer;
    private long lastTime;
    private long treatRateOfFire = 250;
    public Camera cam;
    public GameObject treat;
    private GameObject dog;
    private GameObject cameracart;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
        cameracart = GameObject.Find("CameraCart");
        dog = GameObject.FindGameObjectWithTag("Dog");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && timer.ElapsedMilliseconds - lastTime >= treatRateOfFire && !cameracart.GetComponent<CameraController>().currentlyInTransition() && dog.GetComponent<DogTriggers>().isGameTime) {
            //Attempt to throw treat.
            lastTime = timer.ElapsedMilliseconds;

            Vector3 mouseClickLocation = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 treatDestination = new Vector2(mouseClickLocation.x, mouseClickLocation.y);

            //Check to see if destination point is within range of player
            if(Vector2.Distance(transform.position, treatDestination) > throwRange) return;

            //Initialize treat with destination position

            GameObject spawnedTreat = Instantiate(treat, transform.position, Quaternion.identity);
            spawnedTreat.GetComponent<Treat>().setTreatDestination(treatDestination);

            //Does this need to notify the priority system?
        }
    }
}
