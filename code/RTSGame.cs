public sealed class RTSGame
{
	public RTSGameOptionsComponent GameOptions { get; set; }
	public RTSPlayer ThisPlayer { get; set; }
	public ScreenPanel ThisScreen { get; set; }
	public CorpseList GameCorpseList { get; set; }
	public CommandIndicatorBase GameCommandIndicator { get; set; }
	public RTSHud GameHUD { get; set; }
}
