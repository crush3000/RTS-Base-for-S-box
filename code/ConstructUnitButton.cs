using System;
public class ConstructUnitButton : DynamicButton
{

	public GameObject buttonUnitPrefab { get; set; }

	public Vector3 thisSpawnerPosition { get; set; }

	public ConstructUnitButton(char hotkey, string bg1, GameObject unitPrefab, Vector3 spawnerPosition) : base()
	{
		hotkeyChar = hotkey;
		activeBackgroundImage = bg1;
		buttonUnitPrefab = unitPrefab;
		thisButtonAction = () => constructUnit(unitPrefab);
		thisSpawnerPosition = spawnerPosition;
	}

	public void constructUnit(GameObject unitPrefab)
	{
		RTSPlayer.Local.myUnitFactory.spawnUnit(unitPrefab, RTSPlayer.Local.Team, thisSpawnerPosition);
	}
}
