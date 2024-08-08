public class UnitFactory
{
	UnitFactory() {
		//TEST CODE
		spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(607.9f, 1004.7f, 93.609f));
	}

	public void spawnUnit(GameObject unitPrefab, int team, Vector3 newUnitPosition)
	{
		if (unitPrefab != null) 
		{
			var newUnit = unitPrefab.Clone();
			newUnit.Transform.Position = newUnitPosition;
			newUnit.Components.Get<Unit>().team = team;
			RTSPlayer.Local.addUnit( newUnit );
			newUnit.Enabled = true;
		}
		else
		{
			Log.Error( "ERROR: Prefab Does Not Exist!" );
		}
	}
}
