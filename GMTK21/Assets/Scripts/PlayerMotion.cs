using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public GameObject dog;
    public GameObject person;
    public GameObject leash;
    public Vector2 target = new Vector2();
    public float maxSpeed = 10f;
    private float speed = 5f;
    public float pullSpeed = 5f;
    public float leashLen = 2f;
    private int count = 0;

    //sprites
    public Sprite walk1;
    public Sprite walk2;
    public Sprite walk3;
    public Sprite hold;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dogPos = transform.position;
        Vector2 personPos = person.transform.position;
        Vector2 leashPos = leash.transform.position;

        if (!GameObject.Find("CameraCart").GetComponent<CameraController>().currentlyInTransition()) {
            if(Input.GetKey("space") && GetComponent<DogTriggers>().isGameTime) {
                //Debug.Log("space");
                transform.position = Vector2.MoveTowards(transform.position, target, Mathf.Max(0, (speed - pullSpeed)) * Time.deltaTime);
                person.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hold;
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if((count / 10) % 4 == 0) {
                    person.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walk1;
                }
                else if((count / 10) % 4 == 1) {
                    person.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walk2;
                }
                else if((count / 10) % 4 == 2) {
                    person.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walk3;
                }
                else {
                    person.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walk2;
                }
            }
            count++;
            dogPos = transform.position;

            //manage person
            float distance = Vector2.Distance(dogPos, personPos);
            if(distance >= leashLen) {
                person.transform.position = Vector2.MoveTowards(personPos, dogPos, distance * Time.deltaTime * 5);
            }
            person.transform.GetChild(0).up = dog.transform.position - person.transform.position;
            personPos = person.transform.position;

            //manage leash
            leash.transform.position = ((dogPos + personPos)  + (dogPos - personPos).normalized) / 2;
            leash.transform.right = dogPos - leashPos;
            leash.transform.localScale = new Vector3(Vector2.Distance(dog.transform.position, person.transform.position), 0.1f, 0);
        }
    }

    public void setTargetDestination(Vector2 newTarget) {
        target = newTarget;
    }

    public void setSpeed(float speedPercent) {
        speed = (maxSpeed/4) + speedPercent * 3 * (maxSpeed / 4);//speedPercent should be between 0 and 1
    }


}
