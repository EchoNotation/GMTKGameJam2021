using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public GameObject dog;
    public GameObject person;
    public GameObject leash;
    public Vector2 target = new Vector2();
    private float maxSpeed = 10f;
    private float speed = 5f;
    private float pullSpeed = 3f;
    private float leashLen = 2f;
    private int count = 0;

    //sprites
    public Sprite walk1;
    public Sprite walk2;
    public Sprite walk3;
    public Sprite hold;
    public Sprite wag1;
    public Sprite wag2;
    public Sprite wag3;
    public Sprite wag4;

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
            if (count % 6 == 0)
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag1;
            }
            else if (count % 6 == 1)
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag2;
            }
            else if (count % 6 == 2)
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag3;
            }
            else if (count % 6 == 3)
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag4;
            }
            else if (count % 6 == 4)
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag3;
            }
            else
            {
                dog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wag2;
            }
            count++;
            dogPos = transform.position;
            dog.transform.GetChild(0).up = target - dogPos;

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
        if (speedPercent < 0)
        {
            speedPercent = 0;
        }
        else if (speedPercent > 1) {
            speedPercent = 1;
        }
        speed = (maxSpeed/2) + speedPercent * (maxSpeed / 2);//speedPercent should be between 0 and 1
    }


}
