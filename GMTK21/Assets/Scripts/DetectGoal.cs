using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Goal")) {
            //Level complete!
            Debug.Log("Level complete!");
        }
    }
}
