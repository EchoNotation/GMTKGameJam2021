using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {
    public Camera cam;
    public GameObject person;
    private float xMin, xMax, yMin, yMax;
    private float cameraWidth, cameraHeight;

    // Start is called before the first frame update
    void Start() {
        //Calculate the min and max camera x positions from the size of the tilemap.
        GameObject tilemap = GameObject.Find("BaseTilemap");
        xMin = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(0, 0, 0)).x;
        yMin = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(0, 0, 0)).y;

        int maxXTilespace = tilemap.GetComponent<Tilemap>().size.x;
        int maxYTilespace = tilemap.GetComponent<Tilemap>().size.y;
        cameraWidth = cam.orthographicSize * cam.aspect * 2f;
        cameraHeight = cam.orthographicSize * 2f;
        //Debug.Log(cameraWidth);
        xMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(maxXTilespace, 0, 0)).x - cameraWidth;
        xMax = Mathf.Floor(xMax);
        yMax = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(0, maxYTilespace, 0)).y - cameraHeight;
        yMax = Mathf.Floor(yMax);

        Debug.Log("Camera xMin: " + xMin + " xMax: " + xMax + " yMin: " + yMin + " yMax: " + yMax);
    }

    // Update is called once per frame
    void Update() {
        //Camera needs to be able to scroll horizontally only, but up to a point.

        float personX = person.transform.position.x;
        float personY = person.transform.position.y;
        float cameraX = personX;
        float cameraY = personY;

        if(personX >= xMax) {
            cameraX = xMax;
        }
        else if(personX <= xMin) {
            cameraX = xMin;
        }
        
        if(personY >= yMax) {
            cameraY = yMax;
        }
        else if(personY <= yMin) {
            cameraY = yMin;
        }

        transform.position = new Vector3(cameraX, cameraY, 0);
    }
}
