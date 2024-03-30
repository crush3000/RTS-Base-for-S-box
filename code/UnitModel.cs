using Sandbox.Citizen;
using System;

class UnitModel : Component
{
	public enum OutlineState
	{
		Mine,
		Ally,
		Neutral,
		Hostile,
		Selected
	}

	[Property] public ModelRenderer model { get; set; }
	[Property] Collider UnitCollider { get; set; }

	[Property] public CitizenAnimationHelper animationHandler { get; set; }

	[Property] public HighlightOutline outline { get; set; }

	[Property] public UnitBase baseStand { get; set; }

	public OutlineState selectionOutlineState = OutlineState.Mine;

Vector3 UnitSize { get; set; }
	//Enum UnitType { get; set; }

	protected override void OnStart()
	{
		animationHandler.AimAngle = Transform.Rotation.Forward.EulerAngles;
		animationHandler.IsGrounded = true;
		animationHandler.WithLook( Transform.Rotation.Forward, 1f, 0.75f, 0.5f );
		animationHandler.MoveStyle = CitizenAnimationHelper.MoveStyles.Auto;
		animationHandler.DuckLevel = 0f;
	}

	protected override void OnUpdate()
	{
	}
}
