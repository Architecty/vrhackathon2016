using UnityEngine;
using System.Collections;

public class toggleWallBuilding : MonoBehaviour {

    void Start()
    {

        GameObject[] allControllers = GameObject.FindGameObjectsWithTag("controller");
        foreach (GameObject Controller in allControllers)
        {
            if (Controller.GetComponent<grab>().justUsed == true)
            {
                Controller.GetComponent<buildWall>().isEnabled = true;
            }
        }
    }

    void OnDestroy()
    {

        GameObject[] allControllers = GameObject.FindGameObjectsWithTag("controller");
        foreach (GameObject Controller in allControllers)
        {
            Controller.GetComponent<buildWall>().isEnabled = false;
        }
    }
}