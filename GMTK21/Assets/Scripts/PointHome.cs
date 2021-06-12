using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHome : MonoBehaviour
{
    public float radius = 5f;
    public GameObject home;
    public GameObject parent;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = home.transform.position - parent.transform.position;
        transform.localPosition = (home.transform.position - parent.transform.position).normalized * radius;
        if (Vector3.Distance(home.transform.position, parent.transform.position) >= cam.orthographicSize * 1.5f) {
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }
        else{
            transform.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
}
