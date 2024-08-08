using System;
using System.Xml.Linq;

public class RTSGameComponent : Component
{
	[Property] public CorpseList GameCorpseList { get; set; }
	[Property] public CommandIndicatorBase GameCommandIndicator { get; set; }
	[Property] public RTSHud GameHud { get; set; }

	[Property] public RTSGameOptionsComponent GameOptions { get; set; }
	[Property] public ScreenPanel ThisScreen { get; set; }

	//protected override void OnStart()
	//{
	//}

	/*protected override void OnDestroy()
	{
		thisGame.destroyRefs();
	}*/
}
