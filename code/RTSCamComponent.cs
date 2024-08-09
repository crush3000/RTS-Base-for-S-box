
public class RTSCamComponent : Component
{
	[Property] public CameraComponent CamView { get; set; }
	[Property] public float CamMoveSpeed {  get; set; }
	[Property] public float CamZoomSpeed { get; set; }
	[Property] public float CamRotateSpeed { get; set; }

	float currentCamHeight = 100.0F;

	float camYaw;
	float camPitch;

	protected override void OnStart()
	{
		base.OnStart();
		if (Network.IsProxy)
		{
			CamView.IsMainCamera = false;
			Enabled = false;
			CamView.Enabled = false;
		}
		else
		{
			CamView.IsMainCamera = true;
		}
	}

	protected override void OnUpdate()
	{
		if (Network.IsProxy) {
			return; 
		}

		//Downwards ray calculates global z-level of the ground
		var groundRay = Scene.Trace.Ray( new Ray( Transform.Position, Vector3.Down), 5000.0F);
		var tr = groundRay.Run();
		var currentGroundZLevel = tr.EndPosition.z;
		//Log.Info("Z Level Below Me: " +  currentGroundZLevel);

		// Move Camera
		Vector3 movement = 0;

		// Handle WASD Movement
		if ( Input.Down( "Forward" ) )
		{
			movement += Transform.World.Forward.WithZ(0) * CamMoveSpeed;
		}
		if ( Input.Down( "Backward" ) )
		{
			movement += Transform.World.Backward.WithZ(0) * CamMoveSpeed;
		}
		if ( Input.Down( "Left" ) )
		{
			movement += Transform.World.Left.WithZ(0) * CamMoveSpeed;
		}
		if ( Input.Down( "Right" ) )
		{
			movement += Transform.World.Right.WithZ(0) * CamMoveSpeed;
		}
		// Handle Zoom Level
		if ( Input.MouseWheel.x < 0 || Input.MouseWheel.y < 0 )
		{
			currentCamHeight += CamZoomSpeed;
		}
		if ( Input.MouseWheel.x > 0 || Input.MouseWheel.y > 0 )
		{
			currentCamHeight -= CamZoomSpeed;
		}
		// Cam Height must mandatorily stay above the floor by some amount
		if(currentCamHeight < 100) currentCamHeight = 100;
		// If we fly above a gigantic pitt or off the map, don't launch us to hell
		if (float.Abs( tr.EndPosition.z - Transform.Position.z) >  4000.0)
		{
			movement.z = 0;
		}
		else
		{
			// Calculated z value
			movement.z = ((tr.EndPosition.z + currentCamHeight) - Transform.Position.z);
		}

		// Rotate Camera
		camYaw = Transform.Rotation.Yaw();
		camPitch = Transform.Rotation.Pitch();

		// Add to current position
		var rot = GameObject.Transform.Rotation;
		var pos = GameObject.Transform.Position + movement;

		//Handle Horizontal Rotation
		if ( Input.Down( "Rotate Left" ) )
		{
			camYaw += Time.Delta * CamRotateSpeed;
		}
		if ( Input.Down( "Rotate Right" ) )
		{
			camYaw -= Time.Delta * CamRotateSpeed;
		}

		//Log.Info( "Rotation After Horizontal Rotate" + rot.Angles() );

		// Handle Pitch Rotation
		if ( Input.Down( "Pitch Up" ) )
		{
			camPitch += Time.Delta * CamRotateSpeed;
		}
		if ( Input.Down( "Pitch Down" ) )
		{
			camPitch -= Time.Delta * CamRotateSpeed;
		}
		camPitch = camPitch.Clamp( 0, 89.9f );
		
		// Create Quat Rotation
		rot = Rotation.From( camPitch, camYaw, 0 );

		// Set Transform
		Transform.LerpTo( new Transform( pos, rot, 1 ), 0.1f );
	}
}
