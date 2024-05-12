using Sandbox;
using System;
public sealed class RTSGameOptions
{
	//private static RTSGameOptions instance = null;
	//private static readonly object mut = new object();
	private Dictionary<string, object> optionDict = new Dictionary<string, object>();

	private RTSGameOptions()
	{
	}

	public static RTSGameOptions Instance 
	{
		get
		{
			return RTSGameOptionsInstance.instance;
		}
	}

	private class RTSGameOptionsInstance
	{
		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static RTSGameOptionsInstance()
		{
		}

		internal static readonly RTSGameOptions instance = new RTSGameOptions();
	}

	public void addOption( string key, object value )
	{
		//optionDict.Add( key, value );
		optionDict[key] = value;
	}

	public void removeOption( string key )
	{
		optionDict[ key ] = null;
	}

	public object getOption( string key )
	{
		return optionDict[ key ];
	}

	public void destroyList()
	{
		optionDict.Clear();
	}
}
