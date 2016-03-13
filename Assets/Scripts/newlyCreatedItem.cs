using UnityEngine;
using System.Collections;

public class newlyCreatedItem : MonoBehaviour {

    public Transform creatingController;

	// Use this for initialization
	void Awake () {
        StartCoroutine(resizeToFull(transform.gameObject, 1f));

    }

    void Start()
    {

        findGrabber();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void findGrabber()
    {
        GameObject[] allControllers = GameObject.FindGameObjectsWithTag("controller");
        foreach(GameObject Controller in allControllers)
        {
            Debug.Log("Found Controller");
            if(Controller.GetComponent<grab>().justUsed == true)
            {

                Debug.Log("Found Right Controller");
                Controller.GetComponent<grab>().GrabObject(transform.gameObject);
            }
        }
    }

    IEnumerator resizeToFull(GameObject whichObject, float howLong)
    {
        Vector3 startingScale = new Vector3(.1f, .1f, .1f);
        float startTime = Time.time;
        while (Time.time - startTime < howLong)
        {
            whichObject.transform.localScale = Vector3.Lerp(startingScale, new Vector3(1f, 1f, 1f), (Time.time - startTime) / howLong);
            yield return null;
        }
        whichObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
