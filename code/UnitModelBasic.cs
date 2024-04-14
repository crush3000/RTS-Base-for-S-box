using Sandbox.Citizen;
using System;

class UnitModelBasic : UnitModelBase
{

	//public UnitModel()
	//{
	//	Log.Info( "Create Model" );
	//	model =	new SkinnedModelRenderer();
	//	unitCollider = new CapsuleCollider();
	//	animationHandler = new CitizenAnimationHelper();
	//	outline = new HighlightOutline();
	//	baseStand = new UnitBaseStand( OutlineState.Neutral );
	//	setOutlineState( OutlineState.Neutral );
	//}

	//public UnitModel( OutlineState origState )
	//{
	//	Log.Info( "Create Specific Model" );
	//	outline = new HighlightOutline();
	//	baseStand = new UnitBaseStand( origState );
	//	setOutlineState( origState );
	//}

	public override void animateMovement( Vector3 velocity, Vector3 wishVelocity )
	{
		//Log.Info( "Animate Basic Start" );
	}

	public override void stopMovementAnimate()
	{
		//Log.Info( "Animate Basic Stop" );
	}

	//protected override void OnStart()
	//{
	//base.OnStart();

	//Log.Info( "Create Model" );
	//model = new SkinnedModelRenderer();
	//model.Model = Model.Load( "core/models/citizen/citizen.vmdl" );
	//model.Model = Model.Load( "addons/citizen/models/citizen/citizen.vmdl" );
	//model.MaterialOverride = Material.Load( "core/models/citizen/skin/citizen_skin_zombie_02.vmat_c" );
	//model.AnimationGraph?
	//bone merge target?

	//unitCollider = new CapsuleCollider();


	//animationHandler = new CitizenAnimationHelper();

	//outline = new HighlightOutline();
	//outline.Color = Color.Transparent;
	//outline.ObscuredColor = new Color( mineColor );
	//outline.InsideColor = Color.Transparent;
	//outline.InsideObscuredColor = Color.Transparent;

	//baseStand = new UnitBaseStand();
	//setOutlineState( OutlineState.Neutral );

	//Log.Info( "Trying to start the animationhandler" );
	//animationHandler.AimAngle = Transform.Rotation.Forward.EulerAngles;
	//animationHandler.IsGrounded = true;
	//animationHandler.WithLook( Transform.Rotation.Forward, 1f, 0.75f, 0.5f );
	//animationHandler.MoveStyle = CitizenAnimationHelper.MoveStyles.Auto;
	//animationHandler.DuckLevel = 0f;
	//}

	protected override void OnUpdate()
	{
	}
}
