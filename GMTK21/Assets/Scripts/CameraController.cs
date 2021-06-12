using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {
    public Camera cam;
    public GameObject person;
    private float xMin, xMax;
    private float cameraWidth;
    private float cameraHeight = -10;

    // Start is called before the first frame update
    void Start() {
        //Calculate the min and max camera x positions from the size of the tilemap.
        cam = this.gameObject.GetComponent<Camera>();
        GameObject tilemap = GameObject.Find("BaseTilemap");
        xMin = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(0, 0, 0)).x;

        int maxXTilespace = tilemap.GetComponent<Tilemap>().size.x;
        cameraWidth = cam.orthographicSize * cam.aspect * 2f;
        //Debug.Log(cameraWidth);
        xMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(maxXTilespace, 0, 0)).x - cameraWidth;
        xMax = Mathf.Floor(xMax);

        //Debug.Log("Camera xMin: " + xMin + " xMax: " + xMax);
    }

    // Update is called once per frame
    void Update() {
        //Camera needs to be able to scroll horizontally only, but up to a point.

        float personX = person.transform.position.x;

        if(personX >= xMax) {
            cam.transform.position = new Vector3(xMax, 0, -10);
        }
        else if(personX <= xMin) {
            cam.transform.position = new Vector3(xMin, 0, -10);
        }
        else {
            cam.transform.position = new Vector3(person.transform.position.x, 0, cameraHeight);
        }
    }
}
