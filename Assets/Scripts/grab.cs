using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class grab : MonoBehaviour
{
    public GameObject selectedObject = null;
    public  GameObject enteredObject = null;
    SteamVR_TrackedObject trackedObj;

    Vector3 lastPosition;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if(enteredObject != null)
            {
                selectedObject = enteredObject;
                GrabObject(selectedObject);
            }
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            releaseObject(selectedObject);
            selectedObject = null;
        }
        if (selectedObject != null)
        {
            lastPosition = transform.position;
        }

    }

    void GrabObject(GameObject whichObject)
    {
        switch (whichObject.tag)
        {
            case "furniture":
                pickupFurniture(whichObject);
                break;
            case "wallMounted":

                break;
            case "ceilingMounted":

                break;
        }
    }

    void releaseObject(GameObject whichObject)
    {
        switch (whichObject.tag)
        {
            case "furniture":
                releaseFurniture(whichObject);

                break;
            case "wallMounted":

                break;
            case "ceilingMounted":

                break;
        }
    }

    void pickupFurniture(GameObject whichObject)
    {
        Destroy(whichObject.GetComponent<Rigidbody>());
        whichObject.transform.SetParent(transform);
    }
    void releaseFurniture(GameObject whichObject)
    {
        whichObject.AddComponent<Rigidbody>();
        whichObject.transform.SetParent(null);
        whichObject.GetComponent<Rigidbody>().velocity = (transform.position - lastPosition) / Time.deltaTime;
    }

    void pickupWallMounted(GameObject whichObject)
    {
        whichObject.transform.SetParent(transform);
    }
    void releaseWallMounted(GameObject whichObject)
    {
        whichObject.AddComponent<Rigidbody>();
        whichObject.transform.SetParent(null);
        whichObject.GetComponent<Rigidbody>().velocity = (transform.position - lastPosition) / Time.deltaTime;
    }
}
