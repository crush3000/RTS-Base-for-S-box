﻿using Sandbox.Citizen;
using System;

class UnitModelBasic : UnitModelBase
{

	[Property] public ModelRenderer model { get; set; }

	public override void setModel( Model newModel, AnimationGraph newAnimGraph, Material newMaterial )
	{
		model.Model = newModel;
		//model.AnimationGraph = newAnimGraph;
		model.MaterialOverride = newMaterial;
	}

	public override void animateMovement( Vector3 velocity, Vector3 wishVelocity )
	{
		//Log.Info( "Animate Basic Start" );
	}

	public override void stopMovementAnimate()
	{
		//Log.Info( "Animate Basic Stop" );
	}

	public override void animateMeleeAttack()
	{
		//
	}

	public override void animateDamageTaken()
	{
		//
	}

	public override void animateDeath()
	{
		//
	}

	protected override void OnUpdate()
	{
	}

	/*protected override void OnDestroy()
	{
		outline.Enabled = false;
		outline.Destroy();
		baseStand.Enabled = false;
		baseStand.Destroy();
		model.Enabled = false;
		model.Destroy();
		base.OnDestroy();
	}*/
}
