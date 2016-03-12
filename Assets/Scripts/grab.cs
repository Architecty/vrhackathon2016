using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class grab : MonoBehaviour
{
    public GameObject selectedObject = null;
    public  GameObject enteredObject = null;
    SteamVR_TrackedObject trackedObj;

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
                Debug.Log("Entered Object " + enteredObject.name);
                selectedObject = enteredObject;
            }
            Debug.Log("Click DOwn");
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            selectedObject = null;
            Debug.Log("Click Up");
        }
        
    }
}
