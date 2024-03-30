using System;

class Unit : Component
{
	[Property] public UnitModel PhysicalModel { get; set; }
	[Property] public NavMeshAgent UnitNavAgent { get; set; }

	Vector3 UnitSize { get; set; }
	//Enum UnitType { get; set; }
	//Enum UnitState { get; set; }
	bool Selected { get; set; }
	public bool CommandGiven { get; set; }
	public Vector3 TargetLocation { get; set; }

	protected override void OnUpdate()
	{
		// Handle Move Command
		if ( CommandGiven )
		{
			UnitNavAgent.MoveTo( TargetLocation );
		}
		CommandGiven = false;
		// Handle Animations
		if( !UnitNavAgent.Velocity.IsNearZeroLength ) 
		{
			PhysicalModel.animationHandler.MoveStyle = Sandbox.Citizen.CitizenAnimationHelper.MoveStyles.Run;
			PhysicalModel.animationHandler.WithVelocity( UnitNavAgent.Velocity );
			PhysicalModel.animationHandler.WithWishVelocity( UnitNavAgent.WishVelocity );
		}
		else
		{
			PhysicalModel.animationHandler.MoveStyle = Sandbox.Citizen.CitizenAnimationHelper.MoveStyles.Auto;
			PhysicalModel.animationHandler.WithVelocity( Vector3.Zero );
			PhysicalModel.animationHandler.WithWishVelocity( Vector3.Zero );
		}
	}

	public void SelectUnit()
	{
		Selected = true;
	}

	public void DeSelectUnit()
	{
		Selected = false;
	}
}
