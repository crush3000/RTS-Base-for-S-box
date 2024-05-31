using System;

public class RTSPlayer : Component
{

	[Property] public PlayerUnitControl UnitControl;
	[Property] public int Team;

	protected override void OnStart()
	{
		base.OnStart();
		UnitControl = new PlayerUnitControl();
	}
}
