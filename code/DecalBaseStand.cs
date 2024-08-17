using Sandbox;
using Sandbox.Citizen;
using System;

public class DecalBaseStand : UnitBaseStandBase
{
	//[Property] public new DecalRenderer BaseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState SelectionOutlineState = UnitModelUtils.OutlineState.Neutral;

	private const float DECAL_PROJECT_DISTANCE = 256;
	private const float DECAL_SIZE_MULTIPLIER = 1.25f;

	public override void setOutlineState(UnitModelUtils.OutlineState newOState )
	{
		SelectionOutlineState = newOState;

		if ( BaseStandModel != null) {
			switch( SelectionOutlineState )
			{
				case UnitModelUtils.OutlineState.Mine:
					BaseStandModel.TintColor = new Color(UnitModelUtils.COLOR_MINE);
					break;
				case UnitModelUtils.OutlineState.Ally:
					BaseStandModel.TintColor = new Color(UnitModelUtils.COLOR_ALLY); 
					break;
				case UnitModelUtils.OutlineState.Neutral: 
					BaseStandModel.TintColor = new Color(UnitModelUtils.COLOR_NEUTRAL);
					break;
				case UnitModelUtils.OutlineState.Hostile:
					BaseStandModel.TintColor = new Color( UnitModelUtils.COLOR_HOSTILE);
					break;
				case UnitModelUtils.OutlineState.Selected:
					BaseStandModel.TintColor = new Color( UnitModelUtils.COLOR_SELECTED);
					break;
				case UnitModelUtils.OutlineState.None:
					BaseStandModel.TintColor = new Color( UnitModelUtils.COLOR_NONE );
					break;
			}
		}
	}

	public override void setSize( Vector3 newSize )
	{
		float targetxyMin = DECAL_SIZE_MULTIPLIER * float.Min( newSize.x, newSize.y );

		BaseStandModel.Size = new Vector3( targetxyMin, targetxyMin, DECAL_PROJECT_DISTANCE );
	}

	protected override void OnUpdate()
	{

	}
}
