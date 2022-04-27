using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	// We store a reference to the car's transform, so we can get its position and rotation.
	public Transform carTransform;
	
	// Do we want the camera to swing around, instead of being rigidly locked to the car?
	public bool swingAround = true;

	// How fast does the camera move, when attempting to get to the target position behind the car?
	[Range (0.0f, 1.0f)]
	public float cameraMoveSnap = 0.1f;

	// How fast does the camera rotate, when attempting to get to the target rotation looking at the car?
	[Range (0.0f, 1.0f)]
	public float cameraTurnSnap = 0.1f;

	// What is the target position for the camera to be relative to the car? i.e. 8 meters behind, 3 meters above.
	Vector3 cameraDistanceFromCar = Vector3.back * 8 + Vector3.up * 3;


	void Start(){
		transform.position = carTransform.position + carTransform.rotation * cameraDistanceFromCar;
		transform.rotation = carTransform.rotation;
	}

	void FixedUpdate()
	{
		Vector3 targetPosition = carTransform.position + carTransform.rotation * cameraDistanceFromCar;
		Quaternion targetRotation = carTransform.rotation;

		if (swingAround) {
			// If we want the camera to swing around, we don't snap immediately to the target position and rotation, but
			// slowly move towards it.
			transform.position = Vector3.Lerp(transform.position, targetPosition, cameraMoveSnap);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraTurnSnap);
		} else {
			transform.position = targetPosition;
			transform.rotation = carTransform.rotation;
		}
		
	}

}
