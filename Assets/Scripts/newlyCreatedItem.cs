using UnityEngine;
using System.Collections;

public class newlyCreatedItem : MonoBehaviour {

    public Transform creatingController;

	// Use this for initialization
	void Start () {
        StartCoroutine(resizeToFull(transform.gameObject, 1f));
	}
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator resizeToFull(GameObject whichObject, float howLong)
    {
        Vector3 startingScale = new Vector3(.1f, .1f, .1f);
        float startTime = Time.time;
        while (Time.time - startTime < howLong)
        {
            whichObject.transform.localScale = Vector3.Lerp(startingScale, new Vector3(1f, 1f, 1f), (Time.time - howLong) / howLong);
            yield return null;
        }
        whichObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
