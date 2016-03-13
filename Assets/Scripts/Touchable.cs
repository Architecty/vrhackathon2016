using UnityEngine;
using System.Collections;

public class Touchable : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "controller")
        {
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
