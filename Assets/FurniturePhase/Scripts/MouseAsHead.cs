using UnityEngine;

// MouseAsHead lets you use the mouse to stand in for the head-based movements
// from a VR headset. This is useful for testing in development without having
// a device every time.

// To activate during runtime: hold ctrl. Then, move the mouse left and right to
// rotate in yaw and up and down to tilt in pitch.

// To implement: attach this script to the parent of OVRCameraRig (create this
// parent if necessary). This script assumes there is no player avatar.

// This script is based on the MouseLook script provided by Unity's standard
// assets.

public class MouseAsHead : MonoBehaviour {

#if UNITY_EDITOR

	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
	}

#endif
}
