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
	[Property] public GameObject skeltalPrefab {  get; set; }
	[Property] public GameObject skeltalHousePrefab { get; set; }

	private List<GameObject> myUnits = new List<GameObject>();
	public UnitFactory myUnitFactory = new UnitFactory();

	private static RTSPlayer _local = null;

	protected override void OnStart()
	{
		if(Network.IsProxy) { return; }
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
