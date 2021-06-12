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
    private double throwRange = 7;
    private Stopwatch timer;
    private long lastTime;
    private long treatRateOfFire = 250;
    public Camera cam;
    public GameObject treat;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && timer.ElapsedMilliseconds - lastTime >= treatRateOfFire) {
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
            GameObject.FindGameObjectWithTag("Dog").GetComponent<DogPriorities>().addNewObject(spawnedTreat);
        }
    }
}
