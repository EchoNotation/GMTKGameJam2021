using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogTriggers : MonoBehaviour
{
    //timer variables
    public float levelTimeSec = 60f;
    private float timeRemaining;
    public GameObject panel;
    private RectTransform mercury;

    private void Start()
    {
        timeRemaining = levelTimeSec;
        mercury = (RectTransform) panel.transform.GetChild(0);
    }

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

    private void Update()
    {

        timeRemaining -= Time.deltaTime;
        float temp = Mathf.Round(timeRemaining * 100f) / 100f;
        panel.transform.GetComponentInChildren<Text>().text = temp.ToString();
        mercury.sizeDelta = new Vector2(23, 125*(1 - timeRemaining/levelTimeSec));
        //for some reason
        mercury.localPosition = new Vector2(0, (63 * (1 - (timeRemaining/levelTimeSec)))-47);
    }
}
