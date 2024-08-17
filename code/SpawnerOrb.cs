using System;
public class SpawnerOrb : ControlOrb
{
	[Property] List<UnitPortraitTuple> spawnableUnitList {  get; set; }
	protected override void OnStart()
	{
		base.OnStart();
		ThisOrbType = OrbType.Spawner;
		foreach(var unitType in spawnableUnitList) 
		{
			buttons.Add(new ConstructUnitButton('.', unitType.UnitPortraitImage, unitType.UnitPrefab, Transform.Position));
		}
	}
}
