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
		itemMask = ~LayerMask.GetMask( "Player" );
	}

	void Update()
	{
		if( SpiffyInput.CheckAxis( "Menu" ) )
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			escape = true;
		}
		if( SpiffyInput.CheckAxis( "Fire1" ) && !playerInv.IsOpen() && !PauseMenu.IsOpen() )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			escape = false;
		}

		if( !escape && !playerInv.IsOpen() && !PauseMenu.IsOpen() )
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

		var clipRay = new Ray( player.transform.position,transform.position - player.transform.position );
		RaycastHit clipHit;
		if( Physics.Raycast( clipRay,out clipHit,distToPlayer,worldMask ) )
		{
			transform.position = clipHit.point + clipHit.normal * cam.nearClipPlane * 1.5f;
		}

		var interactRay = new Ray( transform.position + transform.forward * distToPlayer / 2.0f,transform.forward );
		RaycastHit interactHit;
		if( Physics.Raycast( interactRay,out interactHit,100.0f,itemMask ) )
		{
			interactHit.transform.GetComponent<InteractiveBase>()?.Look();
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
	LayerMask itemMask;
}
