using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponBase
    :
    MonoBehaviour
{
    void Start()
	{
        shotSpot = transform.Find( "ShotSpot" );
        cam = Camera.main;

        refire.Update( refire.GetDuration() );
	}

    void Update()
	{
        refire.Update( Time.deltaTime );
        if( SpiffyInput.CheckFree( "Fire1" ) && refire.IsDone() )
		{
            refire.Reset();
            var proj = Instantiate( projectile );
            proj.transform.position = shotSpot.position;
            proj.GetComponent<Rigidbody>().AddForce( cam.transform.forward * shotSpd,ForceMode.Impulse );
            proj.GetComponent<Projectile>().SetTeam( 1 );
		}
	}

    Transform shotSpot;
    Camera cam;

    [SerializeField] Timer refire = new Timer( 1.0f );
    [SerializeField] GameObject projectile = null;
    [SerializeField] float shotSpd = 10.0f;
}
