using System;

class Unit : Component
{
	[Property] public UnitModelBase PhysicalModel { get; set; }
	[Property] public NavMeshAgent UnitNavAgent { get; set; }

	Vector3 UnitSize { get; set; }
	//Enum UnitType { get; set; }
	//Enum UnitState { get; set; }
	bool Selected { get; set; }
	public bool CommandGiven { get; set; }
	public Vector3 TargetLocation { get; set; }

	//public Unit()
	//{
	//	Log.Info("Creating Unit");
	//	PhysicalModel = new UnitModel();
	//	UnitNavAgent = new NavMeshAgent();
	//	UnitSize = new Vector3(60, 60);
	//	Selected = false;
	//	CommandGiven = false;
	//	TargetLocation = Vector3.Zero;
	//}

	//protected override void OnAwake()
	//{
		//base.OnAwake();
		//Log.Info( "Creating Unit for " + this.GameObject );
		//PhysicalModel = new UnitModel();

		//UnitNavAgent = new NavMeshAgent();
		//UnitNavAgent.Height = 20;
		//UnitNavAgent.Radius = 5;
		//UnitNavAgent.MaxSpeed = 60;
		//UnitNavAgent.Acceleration = 600;
		//UnitNavAgent.UpdatePosition = true;
		//UnitNavAgent.UpdateRotation = true;

		//UnitSize = new Vector3( 60, 60 );
		//Selected = false;
		//CommandGiven = false;
		//TargetLocation = Vector3.Zero;
	//}

	//protected override void OnStart()
	//{
		//base.OnStart();

	//}

	protected override void OnUpdate()
	{
		// Handle Move Command
		if ( CommandGiven )
		{
			UnitNavAgent.MoveTo( TargetLocation );
		}
		CommandGiven = false;
		//Log.Info( "Do I still have a gameobject? " + this.GameObject );
		// Handle Animations
		if (PhysicalModel != null && UnitNavAgent != null) 
		{
			//Log.Info( PhysicalModel );
			//Log.Info( UnitNavAgent );
			//Log.Info( UnitNavAgent.Velocity );
			if ( !UnitNavAgent.Velocity.IsNearZeroLength )
			{
				PhysicalModel.animateMovement(UnitNavAgent.Velocity, UnitNavAgent.WishVelocity);
			}
			else
			{
				PhysicalModel.stopMovementAnimate();
			}
		}
	}

	public void SelectUnit()
	{
		Selected = true;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Selected );
	}

	public void DeSelectUnit()
	{
		Selected = false;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Mine );
	}
}
