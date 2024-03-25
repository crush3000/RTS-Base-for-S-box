using Sandbox;
using Sandbox.Diagnostics;
using Sandbox.Services;
using System;
using System.Threading;

public sealed class RTSCamComponent : Component
{
	[Property] ModelRenderer CamModel { get; set; }
	[Property] CameraComponent CamView { get; set; }

	float camMoveSpeed = 5.0F;
	float camZoomSpeed = 20.0F;

	float camHeight = 100.0F;

	protected override void OnUpdate()
	{
		var groundRay = Scene.Trace.Ray( new Ray( Transform.Position, Transform.World.Rotation.Down), 5000.0F);
		var tr = groundRay.Run();
		var currentGroundZLevel = tr.EndPosition.z;
		Log.Info("Z Level Below Me: " +  currentGroundZLevel);


		//var bbox = BBox.FromPositionAndSize( 0, 5 );

		// Move Camera
		Vector3 movement = 0;
		if ( Input.Down( "Forward" ) )
		{
			movement += Transform.World.Forward.WithZ(0) * camMoveSpeed;
		}
		if ( Input.Down( "Backward" ) )
		{
			movement += Transform.World.Backward.WithZ(0) * camMoveSpeed;
		}
		if ( Input.Down( "Left" ) )
		{
			movement += Transform.World.Left.WithZ(0) * camMoveSpeed;
		}
		if ( Input.Down( "Right" ) )
		{
			movement += Transform.World.Right.WithZ(0) * camMoveSpeed;
		}

		if ( Input.MouseWheel.x < 0 )
		{
			movement += Transform.World.Up.WithX(0).WithY(0) * camZoomSpeed;
			camHeight += camZoomSpeed;
		}
		if ( Input.MouseWheel.y < 0 )
		{
			movement += Transform.World.Up.WithX( 0 ).WithY( 0 ) * camZoomSpeed;
			camHeight += camZoomSpeed;
		}
		if ( Input.MouseWheel.x > 0 )
		{
			movement += Transform.World.Down.WithX( 0 ).WithY( 0 ) * camZoomSpeed;
			camHeight -= camZoomSpeed;
		}
		if ( Input.MouseWheel.y > 0 )
		{
			movement += Transform.World.Down.WithX( 0 ).WithY( 0 ) * camZoomSpeed;
			camHeight -= camZoomSpeed;
		}
		if(camHeight < 0) camHeight = 0;
		Log.Info("Movement before camHeight" + movement);
		Log.Info( camHeight + " + " + tr.EndPosition.z + " = " + (tr.EndPosition.z + camHeight));

		//movement.z = (tr.EndPosition.z + camHeight);

		var rot = GameObject.Transform.Rotation;
		var pos = GameObject.Transform.Position + movement * Time.Delta * 100.0f;

		if ( Input.Down( "Pitch_Up" ) )
		{
			rot *= Rotation.From( (Time.Delta * 90.0f).Clamp(0, 90), 0, 0);
		}
		if ( Input.Down( "Pitch_Down" ) )
		{
			rot *= Rotation.From( (Time.Delta * -90.0f).Clamp(0, 90), 0, 0 );
		}

		Transform.Local = new Transform( pos, rot, 1 );
	}
}
