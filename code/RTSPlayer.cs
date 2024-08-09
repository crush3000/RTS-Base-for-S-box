using System;

public class RTSPlayer : Component
{
	public static RTSPlayer Local
	{
		get
		{
			if(!_local.IsValid())
			{
				_local = Game.ActiveScene.GetAllComponents<RTSPlayer>().FirstOrDefault(x => x.Network.IsOwner);
			}
			return _local;
		}
	}

	[Property] public PlayerUnitControl UnitControl;
	[Property] public int Team;
	[Property] public RTSGameComponent LocalGame;
	//DEBUG REMOVE
	[Property] public GameObject skeltalPrefab {  get; set; }
	[Property] public GameObject skeltalHousePrefab { get; set; }
	//DEBUG REMOVE

	private List<GameObject> myUnits = new List<GameObject>();
	public UnitFactory myUnitFactory = new UnitFactory();

	private static RTSPlayer _local = null;

	protected override void OnStart()
	{
		if(Network.IsProxy) 
		{ 
			UnitControl.Enabled = false;
			LocalGame.Enabled = false;
			return;
		}

		//Set Team. This will probably be replaced when we have a lobby setup
		if(Game.ActiveScene.GetAllComponents<RTSPlayer>().Count() == 1)
		{
			this.Team = 0;
		}
		else
		{
			this.Team = Game.ActiveScene.GetAllComponents<RTSPlayer>().MaxBy(x => x.Team).Team + 1;
		}

		//Get Ownership over your units
		var myUnitList = Game.ActiveScene.GetAllComponents<Unit>().Where(x => x.team == Team);
		foreach( var unit in myUnitList)
		{
			//Log.Info("Taking Ownership of " + unit.GameObject.Name);
			unit.Network.TakeOwnership();
		}
		
		base.OnStart();

	}

	public void addUnit(GameObject newUnit)
	{
		myUnits.Add( newUnit );
	}

	public void removeUnit(GameObject unitToRemove)
	{
		myUnits.Remove( unitToRemove );
	}
}
