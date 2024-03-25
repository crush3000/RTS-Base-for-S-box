using Sandbox.UI;

public class MyPanel : Panel
{
	public Label MyLabel { get; set; }

	public MyPanel()
	{
		MyLabel = new Label();
		MyLabel.Parent = this;
	}

	public override void Tick()
	{
		MyLabel.Text = "Debig Text";
	}
}
