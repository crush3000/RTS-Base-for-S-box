using System;

public class DynamicButton
{
	public const string BUTTON_DISABLED_STYLE = "flex";
	public const string BUTTON_ENABLED_STYLE = "none";
	public const string BUTTON_EVENTS_ENABLED_STYLE = "all";
	public const string BUTTON_EVENTS_DISABLED_STYLE = "none";
	public const string BUTTON_DISABLED_HINT_STYLE = "transparent";
	public const string BUTTON_ENABLED_HINT_STYLE = "black";

	public char hotkeyChar { get; set; }
	public string activeBackgroundImage { get; set; }
	public Action thisButtonAction { get; set; }
	public bool isEnabled { get; set; }

	private string isEnabledStyleString = BUTTON_ENABLED_STYLE;
	private string buttonPointerEventsString = BUTTON_EVENTS_ENABLED_STYLE;
	private string isEnabledStyleHintString = BUTTON_ENABLED_HINT_STYLE;

	public DynamicButton() { }

	public DynamicButton(char hotkey, string bgFile, Action clickAction)
	{
		hotkeyChar = hotkey;
		activeBackgroundImage = bgFile;
		thisButtonAction = clickAction;
	}

	public virtual void OnClick()
	{
		thisButtonAction();
	}

	public void setEnabled(bool enabled)
	{
		isEnabled = enabled;
		if (isEnabled)
		{
			isEnabledStyleString = BUTTON_ENABLED_STYLE;
			isEnabledStyleHintString = BUTTON_ENABLED_HINT_STYLE;
			buttonPointerEventsString = BUTTON_EVENTS_ENABLED_STYLE;
		}
		else
		{
			isEnabledStyleString = BUTTON_DISABLED_STYLE;
			isEnabledStyleHintString = BUTTON_DISABLED_HINT_STYLE;
			buttonPointerEventsString = BUTTON_EVENTS_DISABLED_STYLE;
		}
	}

	public string getIsDisabledStyle()
	{
		return isEnabledStyleString;
	}

	public string getIsDisabledStyleHint()
	{
		return isEnabledStyleHintString;
	}

	public string getPointerEventsStyle()
	{
		return buttonPointerEventsString;
	}
}
