using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTriggers : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Goal")) {
            //Level complete!
            Debug.Log("Level complete!");
        }
        else if(collider.CompareTag("Neighbor")) {
            collider.GetComponent<Neighbor>().beginPettingDog();
            //Alert the dog that it is being pet!
        }
    }
}
