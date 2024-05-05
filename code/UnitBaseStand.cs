using Sandbox;
using Sandbox.Citizen;
using System;

public class UnitBaseStand : Component
{
	[Property] public ModelRenderer baseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState selectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public void setOutlineState(UnitModelUtils.OutlineState newOState )
	{
		selectionOutlineState = newOState;

		if ( baseStandModel != null) {
			switch( selectionOutlineState )
			{
				case UnitModelUtils.OutlineState.Mine:
					baseStandModel.Tint = new Color(UnitModelUtils.mineColor);
					break;
				case UnitModelUtils.OutlineState.Ally:
					baseStandModel.Tint = new Color(UnitModelUtils.allyColor); 
					break;
				case UnitModelUtils.OutlineState.Neutral: 
					baseStandModel.Tint = new Color(UnitModelUtils.neutralColor);
					break;
				case UnitModelUtils.OutlineState.Hostile:
					baseStandModel.Tint = new Color( UnitModelUtils.hostileColor);
					break;
				case UnitModelUtils.OutlineState.Selected:
					baseStandModel.Tint = new Color( UnitModelUtils.selectedColor);
					break;
			}
		}
	}

	protected override void OnUpdate()
	{

	}

	protected override void OnDestroy()
	{
		baseStandModel.Enabled = false;
		baseStandModel.Destroy();
		base.OnDestroy();
	}
}
