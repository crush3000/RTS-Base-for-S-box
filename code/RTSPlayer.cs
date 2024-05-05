using System;

class RTSPlayer : Component
{

	[Property] public PlayerUnitControl UnitControl;
	[Property] public int team;

	protected override void OnStart()
	{
		base.OnStart();
		UnitControl = new PlayerUnitControl();
	}
}
