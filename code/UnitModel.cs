using Sandbox.Citizen;
using System;

class UnitModel : UnitModelBase
{

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
}
