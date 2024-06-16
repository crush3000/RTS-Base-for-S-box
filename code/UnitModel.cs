using Sandbox.Citizen;
using System;

class UnitModel : UnitModelBase
{

	[Property] public SkinnedModelRenderer model {  get; set; }

	public override void setModel( Model newModel, AnimationGraph newAnimGraph, Material newMaterial )
	{
		model.Model = newModel;
		model.AnimationGraph = newAnimGraph;
		model.MaterialOverride = newMaterial;
	}

	public override void animateMovement(Vector3 velocity, Vector3 wishVelocity)
	{
		model.SceneModel.SetAnimParameter( "isMoving", true );
	}

	public override void stopMovementAnimate()
	{
		model.SceneModel.SetAnimParameter( "isMoving", false );
	}

	public override void animateMeleeAttack()
	{
		model.SceneModel.SetAnimParameter( "onAttack", true );
	}

	public override void animateDamageTaken()
	{
		model.SceneModel.SetAnimParameter( "onDamage", true );
	}

	public override void animateDeath()
	{
		model.SceneModel.SetAnimParameter( "onDeath", true );
		addToCorpsePile();
	}

	protected override void OnStart()
	{
		//Log.Info( "Trying to start the animationhandler" );
		//foreach ( var anim in model.SceneModel.DirectPlayback.Animations )
		//{
		//	Log.Info( anim );
		//}
	}

	protected override void OnUpdate()
	{
		//Log.Info( model.SceneModel.Tags.TryGetAll() );
		//if ( model.SceneModel.Tags. )
		//{
		//	attackSet = false;
		//	model.SceneModel.SetAnimParameter( "isAttacking", false );
		//}
	}
}
