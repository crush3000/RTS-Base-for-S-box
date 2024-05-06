using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitModelUtils
{
	public const uint COLOR_MINE = 0x8000FF00;
	public const uint COLOR_ALLY = 0x800000FF;
	public const uint COLOR_NEUTRAL = 0x80FFFFFF;
	public const uint COLOR_HOSTILE = 0x80FF0000;
	public const uint COLOR_SELECTED = 0xFF00FF80;

	public enum CommandType
	{
		Move,
		Attack,
		None
	}

	public enum OutlineState
	{
		Mine,
		Ally,
		Neutral,
		Hostile,
		Selected
	}
}
