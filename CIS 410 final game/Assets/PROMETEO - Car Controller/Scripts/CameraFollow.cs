using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform carTransform;
	[Range(1, 10)]
	public float followSpeed = 2;
	[Range(1, 10)]
	public float lookSpeed = 5;
	public float distanceToCar = 5;
	Vector3 initialCameraPosition;
	Vector3 newCarPosition;
	Vector3 cameraDistanceFromCar = Vector3.back * 8 + Vector3.up * 3;

	void Start(){
		initialCameraPosition = gameObject.transform.position;
	}

	void FixedUpdate()
	{
		transform.position = carTransform.position + carTransform.rotation * cameraDistanceFromCar;
		transform.rotation = carTransform.rotation;
	}

}
