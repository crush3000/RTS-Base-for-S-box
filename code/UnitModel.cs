using Sandbox.Citizen;
using System;

class UnitModel : UnitModelBase
{

	[Property] public SkinnedModelRenderer model {  get; set; }

	//public UnitModel()
	//{
	//	Log.Info( "Create Model" );
	//	model =	new SkinnedModelRenderer();
	//	unitCollider = new CapsuleCollider();
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

		Log.Info( "Trying to start the animationhandler" );
		foreach ( var anim in model.SceneModel.DirectPlayback.Animations )
		{
			Log.Info( anim );
		}
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
