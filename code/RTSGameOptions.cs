using Sandbox;
using System;
public sealed class RTSGameOptions
{
	private static RTSGameOptions instance = null;
	private static readonly object mut = new object();
	private Dictionary<string, object> optionDict = new Dictionary<string, object>();

	RTSGameOptions()
	{
	}

	public static RTSGameOptions Instance
	{
		get
		{
			lock ( mut )
			{
				if ( instance == null )
				{
					instance = new RTSGameOptions();
				}
				return instance;
			}
		}
	}

	public void addOption( string key, object value )
	{
		optionDict.Add( key, value );
	}

	public void removeOption( string key )
	{
		optionDict[ key ] = null;
	}

	public object getOption( string key )
	{
		return optionDict[ key ];
	}
}
