using System;
public class DynamicToggleButton : DynamicButton
{
	public string inactiveBackgroundImage { get; set; }

	public bool buttonState { get; set; }

	public DynamicToggleButton( char hotkey, string bg1, string bg2, Action clickAction ) : base(hotkey, bg1, clickAction)
	{
		inactiveBackgroundImage = bg2;
	}

	public void setButtonState(string desiredBG)
	{
		if(desiredBG != activeBackgroundImage && desiredBG != inactiveBackgroundImage)
		{
			Log.Error( desiredBG + " is not a valid state for this button!" );
		}
		else if(desiredBG == inactiveBackgroundImage) 
		{
			toggleButtonState();
		}
	}

	public void toggleButtonState()
	{
		var tempBIContainer = activeBackgroundImage;
		activeBackgroundImage = inactiveBackgroundImage;
		inactiveBackgroundImage = tempBIContainer;
	}

}
