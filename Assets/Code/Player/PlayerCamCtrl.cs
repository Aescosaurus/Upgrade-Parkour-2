using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamCtrl
	:
	MonoBehaviour
{
	void Start()
	{
		player = GameObject.Find( "Player" );
		cam = Camera.main;

		distToPlayer = ( minDistToPlayer + maxDistToPlayer ) / 2.0f;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		playerInv = FindObjectOfType<InventoryHandler>();

		worldMask = LayerMask.GetMask( "World" );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			escape = true;
		}
		if( Input.GetMouseButtonDown( 0 ) && !playerInv.IsOpen() )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			escape = false;
		}

		if( !escape && !playerInv.IsOpen() )
		{
			var aim = new Vector2( Input.GetAxis( "Mouse X" ),
				Input.GetAxis( "Mouse Y" ) );

			aim.y = aim.y * rotationSpeed * Time.deltaTime;
			if( aim.y > maxAimMove ) aim.y = maxAimMove;
			if( aim.y < -maxAimMove ) aim.y = -maxAimMove;

			var tempAng = transform.eulerAngles;
			tempAng.x = tempAng.x - aim.y;
			if( tempAng.x > 90.0f - verticalCutoff && tempAng.x < 180.0f ) tempAng.x = 90.0f - verticalCutoff;
			if( tempAng.x < 270.0f + verticalCutoff && tempAng.x > 180.0f ) tempAng.x = 270.0f + verticalCutoff;
			tempAng.y = tempAng.y + aim.x * rotationSpeed * Time.deltaTime;
			transform.eulerAngles = tempAng;

			// distToPlayer -= Input.GetAxis( "Mouse ScrollWheel" ) *
			// 	scrollSpeed * Time.deltaTime;

			// distToPlayer = Mathf.Max( minDistToPlayer,distToPlayer );
			// distToPlayer = Mathf.Min( maxDistToPlayer,distToPlayer );
		}

		transform.position = player.transform.position +
			transform.right * offset.x +
			transform.up * offset.y +
			transform.forward * offset.z;
		transform.position -= transform.forward * distToPlayer;

		var ray = new Ray( player.transform.position,transform.position - player.transform.position );
		RaycastHit hit;
		if( Physics.Raycast( ray,out hit,distToPlayer,worldMask ) )
		{
			transform.position = hit.point + hit.normal * cam.nearClipPlane * 1.5f;
		}
	}

	[SerializeField] float minDistToPlayer = 4.0f;
	[SerializeField] float maxDistToPlayer = 6.0f;
	[SerializeField] Vector3 offset = Vector3.zero;
	float distToPlayer;

	[SerializeField] float rotationSpeed = 100.0f;
	// [SerializeField] float scrollSpeed = 50.0f;

	[SerializeField] float verticalCutoff = 10.0f;
	const float maxAimMove = 90.0f - 1.0f;

	GameObject player;
	Camera cam;

	bool escape = false;

	InventoryHandler playerInv;

	LayerMask worldMask;
}
