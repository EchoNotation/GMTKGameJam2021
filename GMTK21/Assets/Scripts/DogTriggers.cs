using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogTriggers : MonoBehaviour {
    //timer variables
    public float levelTimeSec = 60f;
    private float timeRemaining;
    public GameObject panel;
    private RectTransform mercury;
    public bool isGameTime = false;
    public GameObject endScreen;

    private void Start() {
        timeRemaining = levelTimeSec;
        mercury = (RectTransform)panel.transform.GetChild(0);
        isGameTime = true;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Goal") && isGameTime) {
            //Level complete!
            Debug.Log("Level complete!");
            isGameTime = false;
            GameObject.Find("CameraCart").GetComponent<CameraController>().setInTransition();
            endScreen.SetActive(true);
            //Enable level complete message
        }
        else if(collider.CompareTag("Neighbor")) {
            collider.GetComponent<Neighbor>().beginPettingDog();
            gameObject.GetComponent<DogPriorities>().setIsBeingPet(true);
        }
        else if(collider.CompareTag("Treat")) {
            gameObject.GetComponent<DogPriorities>().removeObject(collider.gameObject);
            Destroy(collider.gameObject);
        }
    }

    private void Update() {
        if(!GameObject.Find("CameraCart").GetComponent<CameraController>().currentlyInTransition()) {
            if(timeRemaining <= 0) {
                isGameTime = false;
                endScreen.GetComponentInChildren<Text>().text = "You lose! The chicken has burned!";
                endScreen.SetActive(true);
                //you lose
            }
            if(isGameTime) {
                timeRemaining -= Time.deltaTime;
                float temp = Mathf.Round(timeRemaining * 100f) / 100f;
                panel.transform.GetComponentInChildren<Text>().text = temp.ToString();
                mercury.sizeDelta = new Vector2(23, 125 * (1 - timeRemaining / levelTimeSec));
                //for some reason the following math makes it work
                mercury.localPosition = new Vector2(0, (63 * (1 - (timeRemaining / levelTimeSec))) - 47);
            }
        }
    }
}
