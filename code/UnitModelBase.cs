using Sandbox.Citizen;
using System;

public abstract class UnitModelBase : Component
{

	[Property] public ModelRenderer model { get; set; }

	[Property] public HighlightOutline outline { get; set; }

	[Property] public UnitBaseStandBase baseStand { get; set; }

	[Property] public UnitModelUtils.OutlineState selectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public Vector3 UnitSize { get; set; }

	protected bool attackSet = false;

	public void addToCorpsePile()
	{
		RTSGame.Instance.GameCorpseList.addCorpse( model, Time.Now );
	}

	public virtual void setOutlineState( UnitModelUtils.OutlineState newState )
	{
		if ( outline != null && baseStand != null )
		{
			baseStand.setOutlineState( newState );
			switch ( newState )
			{
				case UnitModelUtils.OutlineState.Mine:
					outline.Enabled = true;
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_MINE );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_MINE );
					break;
				case UnitModelUtils.OutlineState.Ally:
					outline.Enabled = true;
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_ALLY );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_ALLY );
					break;
				case UnitModelUtils.OutlineState.Neutral:
					outline.Enabled = true;
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_NEUTRAL );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_NEUTRAL );
					break;
				case UnitModelUtils.OutlineState.Hostile:
					outline.Enabled = true;
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_HOSTILE );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_HOSTILE );
					break;
				case UnitModelUtils.OutlineState.Selected:
					outline.Enabled = true;
					outline.InsideObscuredColor = new Color( UnitModelUtils.COLOR_SELECTED );
					outline.ObscuredColor = new Color( UnitModelUtils.COLOR_SELECTED );
					break;
				case UnitModelUtils.OutlineState.None:
					outline.Enabled = false;
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
	public virtual void setModelSize( Vector3 size ) 
	{
		baseStand.setSize( size );
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
		//model.Enabled = false;
		//model.Destroy();
		base.OnDestroy();
	}*/
}
