using Sandbox;

public class HealthBar : Component
{

	[Property] public WorldPanel UnitStatusWorldPanel;
	[Property] public HealthBarUI healthBarUI;

	public void setHealth(int currentHealth, int maxHealth)
	{
		healthBarUI.setHealth((int)(((float)currentHealth/(float)maxHealth) * 100));
		healthBarUI.StateHasChanged();
	}

	public void setShowHealthBar(bool showBar)
	{
		UnitStatusWorldPanel.Enabled = showBar;
		healthBarUI.StateHasChanged();
	}

	public void setBarColor(string newColor)
	{
		healthBarUI.setBarColor(newColor);
		healthBarUI.StateHasChanged();
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
