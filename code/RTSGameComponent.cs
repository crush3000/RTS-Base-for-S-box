using System;
using System.Xml.Linq;

public sealed class RTSGameComponent : Component
{
	[Property] RTSGameOptionsComponent Options { get; set; }
	[Property] RTSPlayer ThisPlayer { get; set; }
	[Property] ScreenPanel ThisScreen { get; set; }
	[Property] CorpseList GameCorpseList { get; set; }
	[Property] CommandIndicatorBase GameCommandIndicator { get; set; }


	private RTSGame thisGame;

	protected override void OnStart()
	{
		// Build Game Singleton
		thisGame = RTSGame.Instance;

		// Populate Preselected Options
		thisGame.GameOptions = Options;
		thisGame.ThisPlayer = ThisPlayer;
		thisGame.ThisScreen = ThisScreen;
		thisGame.GameCorpseList = GameCorpseList;
		thisGame.GameCommandIndicator = GameCommandIndicator;
	}

	protected override void OnDestroy()
	{
		thisGame.destroyRefs();
	}
}
