using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCam
	:
	MonoBehaviour
{
	void Start()
	{
		cam = transform.Find( "Main Camera" );

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		Assert.IsTrue( verticalCutoff > 0.0f );

		sensitivity = PlayerPrefs.GetFloat( "sens",1.0f );
	}

	void Update()
	{
		// if( Input.GetKeyDown( KeyCode.Escape ) )
		// {
		// 	Cursor.lockState = CursorLockMode.None;
		// 	Cursor.visible = true;
		// }
		// if( Input.GetMouseButtonDown( 0 ) )
		// {
		// 	Cursor.lockState = CursorLockMode.Locked;
		// 	Cursor.visible = false;
		// }

		aim = new Vector2( Input.GetAxis( "Mouse X" ),
			Input.GetAxis( "Mouse Y" ) ) * sensitivity;
	}

	void FixedUpdate()
	{
		if( PauseMenu.IsOpen() ) return;

		// cam.transform.eulerAngles = new Vector3(
		// 	cam.eulerAngles.x - aim.y * rotationSpeed * Time.deltaTime,
		// 	cam.eulerAngles.y + aim.x * rotationSpeed * Time.deltaTime,
		// 	cam.eulerAngles.z );

		if( aim.y > maxAimMove ) aim.y = maxAimMove;
		if( aim.y < -maxAimMove ) aim.y = -maxAimMove;

		var tempAng = cam.transform.eulerAngles;
		tempAng.x = tempAng.x - aim.y * rotationSpeed * Time.fixedDeltaTime;
		// if( tempAng.x < 0.0f + verticalCutoff ) tempAng.x = 0.0f + verticalCutoff;
		if( tempAng.x > 90.0f - verticalCutoff && tempAng.x < 180.0f ) tempAng.x = 90.0f - verticalCutoff;
		if( tempAng.x < 270.0f + verticalCutoff && tempAng.x > 180.0f ) tempAng.x = 270.0f + verticalCutoff;
		tempAng.y = tempAng.y + aim.x * rotationSpeed * Time.fixedDeltaTime;
		tempAng.z = 0.0f;
		cam.transform.eulerAngles = tempAng;
	}

	public static void SetSensitivity( float sens )
	{
		sensitivity = sens;
	}

	Transform cam;

	[SerializeField] float rotationSpeed = 5.0f;
	[SerializeField] float verticalCutoff = 10.0f;
	const float maxAimMove = 90.0f - 1.0f;

	static float sensitivity = 1.0f;

	Vector3 aim = Vector3.zero;
}
