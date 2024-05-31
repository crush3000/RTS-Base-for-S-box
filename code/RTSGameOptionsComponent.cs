using System;
public sealed class RTSGameOptionsComponent : Component
{
	// Option Parameters
	[Property] public float GlobalUnitScale { get; set; }

	// Option Container
	private RTSGameOptions gameOptions;

	// Key References
	public const string GLOBAL_UNIT_SCALE = "global_unit_scale";

	protected override void OnStart()
	{
		//base.OnStart();
		// Build Game Options Singleton
		gameOptions = RTSGameOptions.Instance;

		// Populate Preselected Options
		setValue( GLOBAL_UNIT_SCALE, GlobalUnitScale );
	}

	protected override void OnDestroy() 
	{
		gameOptions.destroyList();
	}

	public void setValue(string key, object value)
	{
		gameOptions.addOption(key, value);
	}

	public string getStringValue(string key)
	{
		try
		{
			return (string)(gameOptions.getOption( key ));
		}
		catch ( Exception )
		{
			throw;
		}
	}

	public int getIntValue(string key)
	{
		try
		{
			return (int)(gameOptions.getOption( key ));
		}
		catch ( Exception )
		{
			throw;
		}
	}

	public float getFloatValue(string key)
	{
		try
		{
			return (float)(gameOptions.getOption( key ));
		}
		catch ( Exception )
		{
			Log.Error( "Sum Ting Wong" );
			throw;
		}
	}

}
