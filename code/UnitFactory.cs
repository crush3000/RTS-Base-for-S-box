public class UnitFactory
{

	public void SpawnDebugUnits()
	{
		//TEST CODE
		if (RTSPlayer.Local.Team == 0)
		{
			Log.Info("Spawning skeltals?");
			//Friendly Group
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(607.9f, 1004.7f, 93.609f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(715.14f, 1004.7f, 99.609f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(689.9f, 1023.7f, 99.609f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(689.9f, 981.7f, 99.609f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 0, new Vector3(665.9f, 1004.7f, 99.609f));
			//Unfriendly Group
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 1, new Vector3(894f, 1127f, 98f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 1, new Vector3(716f, 1253f, 98f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 1, new Vector3(733f, 1274f, 98f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 1, new Vector3(753f, 1253f, 98f));
			spawnUnit(RTSPlayer.Local.skeltalPrefab, 1, new Vector3(733f, 1231f, 98f));

			//TEST CODE
		}
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
			newUnit.NetworkSpawn();
		}
		else
		{
			Log.Error( "ERROR: Prefab Does Not Exist!" );
		}
	}
}
