using Sandbox.Citizen;
using System;

public abstract class UnitModelBase : Component
{

	//[Property] public ModelRenderer model { get; set; }
	[Property] Collider unitCollider { get; set; }

	[Property] public HighlightOutline outline { get; set; }

	[Property] public UnitBaseStand baseStand { get; set; }

	[Property] public UnitModelUtils.OutlineState selectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public Vector3 UnitSize { get; set; }

	protected bool attackSet = false;

	public virtual void setOutlineState( UnitModelUtils.OutlineState newState )
	{
		if ( outline != null && baseStand != null )
		{
			baseStand.setOutlineState( newState );
			switch ( newState )
			{
				case UnitModelUtils.OutlineState.Mine:
					outline.InsideObscuredColor = new Color( UnitModelUtils.mineColor );
					outline.ObscuredColor = new Color( UnitModelUtils.mineColor );
					break;
				case UnitModelUtils.OutlineState.Ally:
					outline.InsideObscuredColor = new Color( UnitModelUtils.allyColor );
					outline.ObscuredColor = new Color( UnitModelUtils.allyColor );
					break;
				case UnitModelUtils.OutlineState.Neutral:
					outline.InsideObscuredColor = new Color( UnitModelUtils.neutralColor );
					outline.ObscuredColor = new Color( UnitModelUtils.neutralColor );
					break;
				case UnitModelUtils.OutlineState.Hostile:
					outline.InsideObscuredColor = new Color( UnitModelUtils.hostileColor );
					outline.ObscuredColor = new Color( UnitModelUtils.hostileColor );
					break;
				case UnitModelUtils.OutlineState.Selected:
					outline.InsideObscuredColor = new Color( UnitModelUtils.selectedColor );
					outline.ObscuredColor = new Color( UnitModelUtils.selectedColor );
					break;
			}
		}
	}

	public abstract void animateMovement( Vector3 velocity, Vector3 wishVelocity );

	public abstract void stopMovementAnimate();

	public abstract void animateMeleeAttack();

	public abstract void animateDamageTaken();

	public abstract void animateDeath();

	protected override void OnUpdate()
	{
	}
}
