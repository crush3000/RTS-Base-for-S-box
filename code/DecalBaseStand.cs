using Sandbox;
using Sandbox.Citizen;
using System;

public class DecalBaseStand : UnitBaseStandBase
{
	[Property] public DecalRenderer BaseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState SelectionOutlineState = UnitModelUtils.OutlineState.Neutral;

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
			}
		}
	}

	protected override void OnUpdate()
	{

	}

	protected override void OnDestroy()
	{
		BaseStandModel.Enabled = false;
		BaseStandModel.Destroy();
		base.OnDestroy();
	}
}
