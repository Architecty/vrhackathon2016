using UnityEngine;
using System.Collections;

public class wallBuilderMenu : MonoBehaviour {

    void Start()
    {
        GameObject[] allControllers = GameObject.FindGameObjectsWithTag("controller");
        foreach (GameObject Controller in allControllers)
        {
               Controller.GetComponent<buildWall>().isEnabled = true;
        }
    }

    void OnDestroy()
    {
        GameObject[] allControllers = GameObject.FindGameObjectsWithTag("controller");
        foreach (GameObject Controller in allControllers)
        {
            Destroy(Controller.GetComponent<buildWall>().trackObject);
            Controller.GetComponent<buildWall>().isEnabled = false;
        }
    }
}