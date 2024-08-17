using System;
using System.Xml.Linq;

public class RTSGameComponent : Component
{
	[Property] public CorpseList GameCorpseList { get; set; }
	[Property] public CommandIndicatorBase GameCommandIndicator { get; set; }
	[Property] public RTSHud GameHud { get; set; }

	[Property] public RTSGameOptionsComponent GameOptions { get; set; }
	[Property] public ScreenPanel ThisScreen { get; set; }

	protected override void OnStart()
	{
		if (Network.IsProxy)
		{
			Enabled = false;
			GameCorpseList.Enabled = false;
			GameCommandIndicator.Enabled = false;
			GameHud.Enabled = false;
			GameOptions.Enabled = false;
			ThisScreen.Enabled = false;
			return;
		}
		base.OnStart();
	}
}
