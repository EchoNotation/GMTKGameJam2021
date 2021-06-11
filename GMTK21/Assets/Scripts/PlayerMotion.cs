using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public GameObject dog;
    public GameObject person;
    public GameObject leash;
    public Vector2 target = new Vector2();
    public float speed = 10f;

    public float leashLen = 2f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        float distance = Vector2.Distance(transform.position, person.transform.position);
        if (distance >= leashLen)
        {
            person.transform.position = Vector2.MoveTowards(person.transform.position, transform.position, distance * Time.deltaTime);
        }
    }
}