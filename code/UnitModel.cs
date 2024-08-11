using Sandbox.Citizen;
using System;

class UnitModel : UnitModelBase
{

	/*[Sync]*/ //[Property] public new SkinnedModelRenderer skinnedModel {  get; set; }

	public override void setModel( Model newModel, AnimationGraph newAnimGraph, Material newMaterial )
	{
		skinnedModel.Model = newModel;
		skinnedModel.AnimationGraph = newAnimGraph;
		skinnedModel.MaterialOverride = newMaterial;
	}

	[Broadcast]
	public override void animateMovement(Vector3 velocity, Vector3 wishVelocity)
	{
		skinnedModel.SceneModel.SetAnimParameter( "isMoving", true );
	}

	[Broadcast]
	public override void stopMovementAnimate()
	{
		skinnedModel.SceneModel.SetAnimParameter( "isMoving", false );
	}

	[Broadcast]
	public override void animateMeleeAttack()
	{
		skinnedModel.SceneModel.SetAnimParameter( "onAttack", true );
	}

	[Broadcast]
	public override void animateDamageTaken()
	{
		skinnedModel.SceneModel.SetAnimParameter( "onDamage", true );
	}

	[Broadcast]
	public override void animateDeath()
	{
		skinnedModel.SceneModel.SetAnimParameter( "onDeath", true );
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
