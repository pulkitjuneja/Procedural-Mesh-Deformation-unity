using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereShooter : MonoBehaviour {

	public GameObject sphereObject;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
		ShootSpheres();
		}
	}

	void ShootSpheres () {
		Vector3 mousePosition = Input.mousePosition;
		Vector3 cameraPosition = Camera.main.transform.position;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,mousePosition.y, Camera.main.nearClipPlane));
		Vector3 direction = (mouseWorldPosition - cameraPosition).normalized;
		GameObject sphere = Instantiate(sphereObject, transform.position, Quaternion.identity);
		sphere.GetComponent<Rigidbody>().velocity = direction* 40;
		Destroy(sphere,4.0f);
	}
}
