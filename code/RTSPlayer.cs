using System;

public class RTSPlayer : Component
{

	[Property] public PlayerUnitControl UnitControl;
	[Property] public int Team;
	[Property] public GameObject skeltalPrefab {  get; set; }
	[Property] public GameObject skeltalHousePrefab { get; set; }

	private List<GameObject> myUnits = new List<GameObject>();
	public UnitFactory myUnitFactory = new UnitFactory();

	protected override void OnStart()
	{
		base.OnStart();
		//UnitControl = new PlayerUnitControl();
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
