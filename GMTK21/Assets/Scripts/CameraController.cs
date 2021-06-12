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
        GameObject tilemap = GameObject.Find("BaseTilemap");
        xMin = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(0, 0, 0)).x;

        int maxXTilespace = tilemap.GetComponent<Tilemap>().size.x;
        cameraWidth = cam.orthographicSize * cam.aspect * 2f;
        Debug.Log(cameraWidth);
        xMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(maxXTilespace, 0, 0)).x - cameraWidth;
        xMax = Mathf.Floor(xMax);

        Debug.Log("Camera xMin: " + xMin + " xMax: " + xMax);
    }

    // Update is called once per frame
    void Update() {
        //Camera needs to be able to scroll horizontally only, but up to a point.a

        //Determine if the person object's position is too far to one side or the other.
        //If so, then move the camera in the opposite direction by the same displacement.
        /*float personX = person.transform.position.x;
        if(personX >= xMax) {
            float xDisplacement = xMax - personX;
            cam.transform.position = new Vector3(xDisplacement, 0, cameraHeight);

        }
        else if(personX <= xMin) {
            float xDisplacement = xMin - personX;
            cam.transform.position = new Vector3(xDisplacement, 0, cameraHeight);
        }
        else {
            cam.transform.position = new Vector3(0, 0, cameraHeight);
        }*/
    }
}
