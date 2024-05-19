using Sandbox;
using Sandbox.Citizen;
using System;

public abstract class UnitBaseStandBase : Component
{
	[Property] public Renderer BaseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState SelectionOutlineState = UnitModelUtils.OutlineState.Mine;

	public abstract void setOutlineState( UnitModelUtils.OutlineState newOState );

	protected override void OnDestroy()
	{
		BaseStandModel.Enabled = false;
		BaseStandModel.Destroy();
		base.OnDestroy();
	}
}
