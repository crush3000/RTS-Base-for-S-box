public class UnitFactory
{
	public void spawnUnit(GameObject unitPrefab, Vector3 newUnitPosition)
	{
		if (unitPrefab != null) 
		{
			var newUnit = unitPrefab.Clone();
			newUnit.Transform.Position = newUnitPosition;
			RTSGame.Instance.ThisPlayer.addUnit( newUnit );
			newUnit.Enabled = true;
		}
		else
		{
			Log.Error( "ERROR: Prefab Does Not Exist!" );
		}
	}
}
