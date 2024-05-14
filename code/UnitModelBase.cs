using Sandbox.Citizen;
using System;

public abstract class UnitModelBase : Component
{

	[Property] public ModelRenderer model { get; set; }

	[Property] public HighlightOutline outline { get; set; }

	[Property] public UnitBaseStand baseStand { get; set; }

	[Property] public UnitModelUtils.OutlineState selectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public Vector3 UnitSize { get; set; }

	protected bool attackSet = false;

	protected void addToCorpsePile()
	{
		var corpseListObjects = Scene.GetAllComponents<CorpseList>();
		if ( corpseListObjects.Any() )
		{
			corpseListObjects.First().addCorpse( model, Time.Now );
		}
	}

	public virtual void setOutlineState( UnitModelUtils.OutlineState newState )
	{
		if ( outline != null && baseStand != null )
		{
			baseStand.setOutlineState( newState );
			switch ( newState )
			{
				case UnitModelUtils.OutlineState.Mine:
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_MINE );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_MINE );
					break;
				case UnitModelUtils.OutlineState.Ally:
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_ALLY );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_ALLY );
					break;
				case UnitModelUtils.OutlineState.Neutral:
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_NEUTRAL );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_NEUTRAL );
					break;
				case UnitModelUtils.OutlineState.Hostile:
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_HOSTILE );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_HOSTILE );
					break;
				case UnitModelUtils.OutlineState.Selected:
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_SELECTED );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_SELECTED );
					break;
			}
		}
	}

	public abstract void setModel( Model newModel, AnimationGraph newAnimGraph, Material newMaterial );

	public abstract void animateMovement( Vector3 velocity, Vector3 wishVelocity );

	public abstract void stopMovementAnimate();

	public abstract void animateMeleeAttack();

	public abstract void animateDamageTaken();

	public abstract void animateDeath(); 
	public abstract void setModelSize(Vector3 size);

	protected override void OnUpdate()
	{
	}

	protected override void OnDestroy() 
	{
		outline.Enabled = false;
		outline.Destroy();
		baseStand.Enabled = false;
		baseStand.Destroy();
		base.OnDestroy();
	}
}
