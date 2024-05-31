public sealed class RTSGame
{
	public RTSGameOptionsComponent GameOptions { get; set; }
	public RTSPlayer ThisPlayer { get; set; }
	public ScreenPanel ThisScreen { get; set; }
	public CorpseList GameCorpseList { get; set; }
	public CommandIndicatorBase GameCommandIndicator { get; set; }

	private RTSGame()
	{
	}

	public static RTSGame Instance
	{
		get
		{
			return RTSGameInstance.instance;
		}
	}

	private class RTSGameInstance
	{
		static RTSGameInstance()
		{ }

		internal static readonly RTSGame instance = new RTSGame();
	}

	public void destroyRefs()
	{
		GameOptions.Destroy();
		ThisPlayer.Destroy();
		ThisScreen.Destroy();
		GameCorpseList.Destroy();
		GameCommandIndicator.Destroy();
	}
}
