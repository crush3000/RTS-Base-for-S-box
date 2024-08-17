using System;

//This is shite but it will have to do for now because I think I'm gonna have to parse the JSON for each prefab to get the actual string out of it
public class UnitPortraitTuple : Component
{
	[Property] public GameObject UnitPrefab { get; set; }

	[Property] public string UnitPortraitImage { get; set; }

}
