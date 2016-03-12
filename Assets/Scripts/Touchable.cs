using UnityEngine;
using System.Collections;

public class Touchable : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if(other.gameObject.tag == "controller")
        {
            Debug.Log("Main Entered");
            other.GetComponent<grab>().enteredObject = transform.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "controller")
        {
            other.GetComponent<grab>().enteredObject = null;
        }
    }
}
