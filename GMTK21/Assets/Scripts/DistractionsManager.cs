using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

//pass a vector2 to the dog, recived data from subcribing people
public class DistractionsManager : MonoBehaviour
{

    private Dictionary <string, UnityEvent> distractionsDictionary;

    private static DistractionsManager distractionsManager;

    public static DistractionsManager instance
    {
        get
        {
            if (!distractionsManager)
            {
                distractionsManager = FindObjectOfType (typeof (DistractionsManager)) as DistractionsManager;

                if (!distractionsManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    distractionsManager.Init ();
                }
            }

            return distractionsManager;
        }
    }

    void Init()
    {
        if (distractionsDictionary == null)
        {
            distractionsDictionary = new Dictionary<string, UnityEvent>();
        }
    }


    //position, type
    public static string AddDistraction(Vector2 pos, string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.distractionsDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        return "genrate a uniqueId";
    }

    public void AddListener()
    {

    }

    public void RemoveDistraction(string uId)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
