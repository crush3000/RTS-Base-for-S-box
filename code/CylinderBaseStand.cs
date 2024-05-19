using Sandbox;
using Sandbox.Citizen;
using System;

public class CylinderBaseStand : UnitBaseStandBase
{
	[Property] public ModelRenderer BaseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState SelectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public override void setOutlineState(UnitModelUtils.OutlineState newOState )
	{
		SelectionOutlineState = newOState;

		if ( BaseStandModel != null) {
			switch( SelectionOutlineState )
			{
				case UnitModelUtils.OutlineState.Mine:
					BaseStandModel.Tint = new Color(UnitModelUtils.COLOR_MINE);
					break;
				case UnitModelUtils.OutlineState.Ally:
					BaseStandModel.Tint = new Color(UnitModelUtils.COLOR_ALLY); 
					break;
				case UnitModelUtils.OutlineState.Neutral: 
					BaseStandModel.Tint = new Color(UnitModelUtils.COLOR_NEUTRAL);
					break;
				case UnitModelUtils.OutlineState.Hostile:
					BaseStandModel.Tint = new Color( UnitModelUtils.COLOR_HOSTILE);
					break;
				case UnitModelUtils.OutlineState.Selected:
					BaseStandModel.Tint = new Color( UnitModelUtils.COLOR_SELECTED);
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
