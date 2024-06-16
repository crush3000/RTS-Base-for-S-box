using Sandbox;

public class HealthBar : Component
{

	[Property] public WorldPanel UnitStatusWorldPanel;
	[Property] public HealthBarUI healthBarUI;

	private const float WIDTH_MULTIPLIER = 20f;
	private const float SET_HEIGHT = 200f;
	public void setHealth(int currentHealth, int maxHealth)
	{
		healthBarUI.setHealth((int)(((float)currentHealth/(float)maxHealth) * 100));
		healthBarUI.StateHasChanged();
	}

	public void setShowHealthBar(bool showBar)
	{
		healthBarUI.Enabled = showBar;
		healthBarUI.StateHasChanged();
	}

	public void setBarColor(string newColor)
	{
		healthBarUI.setBarColor(newColor);
		healthBarUI.StateHasChanged();
	}

	public void setSize(Vector3 size)
	{
		float targetWidth = WIDTH_MULTIPLIER * float.Max( size.x, size.y );
		float targetHeight = SET_HEIGHT;
		UnitStatusWorldPanel.PanelSize = new Vector2( targetWidth, targetHeight );
	}

	protected override void OnDestroy()
	{
		UnitStatusWorldPanel.Enabled = false;
		UnitStatusWorldPanel.Destroy();
		healthBarUI.Enabled = false;
		healthBarUI.Destroy();
		base.OnDestroy();
	}
}
