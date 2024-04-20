
public sealed class RTSCamComponent : Component
{
	[Property] public CameraComponent CamView { get; set; }

	float camMoveSpeed = 2.0F;
	float camZoomSpeed = 20.0F;

	float camHeight = 100.0F;

	float camYaw;
	float camPitch;

	protected override void OnUpdate()
	{
		//Downwards ray calculates global z-level of the ground
		var groundRay = Scene.Trace.Ray( new Ray( Transform.Position, Vector3.Down), 5000.0F);
		var tr = groundRay.Run();
		var currentGroundZLevel = tr.EndPosition.z;
		//Log.Info("Z Level Below Me: " +  currentGroundZLevel);


		//var bbox = BBox.FromPositionAndSize( 0, 5 );

		// Move Camera
		Vector3 movement = 0;
		// Handle WASD Movement
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
		// Handle Zoom Level
		if ( Input.MouseWheel.x < 0 || Input.MouseWheel.y < 0 )
		{
			camHeight += camZoomSpeed;
		}
		if ( Input.MouseWheel.x > 0 || Input.MouseWheel.y > 0 )
		{
			camHeight -= camZoomSpeed;
		}
		// Cam Height must mandatorily stay above the floor by some amount
		if(camHeight < 100) camHeight = 100;
		//Log.Info("Movement before camHeight " + movement);
		//Log.Info( "Current Position " + Transform.Position);
		//Log.Info( camHeight + " + " + tr.EndPosition.z + " = " + (tr.EndPosition.z + camHeight));
		//Log.Info( (tr.EndPosition.z + camHeight) + " - " + Transform.Position.z + " = " + ((tr.EndPosition.z + camHeight) - Transform.Position.z));

		movement.z = ((tr.EndPosition.z + camHeight) - Transform.Position.z);

		//Log.Info( "Movement after camHeight " + movement );

		// Rotate Camera
		camYaw = Transform.Rotation.Yaw();
		camPitch = Transform.Rotation.Pitch();

		var rot = GameObject.Transform.Rotation;
		var pos = GameObject.Transform.Position + movement; //* Time.Delta * 100.0f;

		//Log.Info( "Position Calculated" + pos );
		//Log.Info( "Current Rotation" + rot.Angles() );

		//Handle Horizontal Rotation
		if ( Input.Down( "Rotate Left" ) )
		{
			camYaw += Time.Delta * 90.0f;
		}
		if ( Input.Down( "Rotate Right" ) )
		{
			camYaw -= Time.Delta * 90.0f;
		}

		//Log.Info( "Rotation After Horizontal Rotate" + rot.Angles() );

		// Handle Pitch Rotation
		if ( Input.Down( "Pitch Up" ) )
		{
			camPitch += Time.Delta * 90.0f;
		}
		if ( Input.Down( "Pitch Down" ) )
		{
			camPitch -= Time.Delta * 90.0f;
		}
		camPitch = camPitch.Clamp( 0, 89.9f );
		
		// Create Quat Rotation
		rot = Rotation.From( camPitch, camYaw, 0 );

		// Set Transform
		//Transform.LerpTo( new Transform( pos, rot, 1 ), 0.1f );
		//GameObject.Transform.ClearLerp();
		//GameObject.Transform.LerpTo( new Transform( pos, rot, 1 ), 1f );
		Transform.Local = new Transform( pos, rot, 1 );
	}
}
