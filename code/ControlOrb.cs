
using Sandbox;
using Sandbox.UI;
using System.Drawing;
using static Sandbox.TextRendering;

public class ControlOrb : SelectableObject, IScalable, ISelectable
{
	[Group("Gameplay")]
	[Property] public OrbType Type { get; set; }

	[Group("Visuals")]
	[Property] public HighlightOutline OrbHighlight { get; set; }

	// Class Vars
	//public int team = 0;
	bool selected { get; set; }

	// Constants
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = 1f;

	public const uint COLOR_SELECTED = 0x8000FFFF;
	public const uint COLOR_SPAWNER = 0x800000FF;
	public const uint COLOR_TRAP = 0x80005EFF;

	public enum OrbType
	{
		Spawner,
		Trap
	}

	protected override void OnStart()
	{
		Log.Info("Orb Object OnStart");
		objectTypeTag = "control_orb";
		base.OnStart();
		setDefaultColor();
		setTeam(0);
		Tags.Add(objectTypeTag);
		if (!Network.IsOwner) {
			this.Enabled = true;
			this.PhysicalModelRenderer.Enabled = true;
			this.SelectionHitbox.Enabled = true;
			this.OrbHighlight.Enabled = true;
			return; 
		}
		;
	}

	public override void select()
	{
		Log.Info("Orb select called!");
		if (!Network.IsOwner) { return; }
		Log.Info("Orb Selected!");
		selected = true;
		var spawnerColor = new Color(COLOR_SELECTED);
		PhysicalModelRenderer.skinnedModel.Tint = spawnerColor;
		OrbHighlight.InsideObscuredColor = spawnerColor;
		OrbHighlight.ObscuredColor = spawnerColor;
	}

	public override void deSelect()
	{
		if (!Network.IsOwner) { return; }
		Log.Info("Orb DeSelected!");
		selected = false;
		setDefaultColor();
	}

	public void onOwnerJoin()
	{
		this.Enabled = true;
		this.PhysicalModelRenderer.Enabled = true;
		this.SelectionHitbox.Enabled = true;
		this.OrbHighlight.Enabled = true;
		setDefaultColor();
	}

	private void setDefaultColor()
	{
		if (Type == OrbType.Spawner)
		{
			var spawnerColor = new Color(COLOR_SPAWNER);
			PhysicalModelRenderer.skinnedModel.Tint = spawnerColor;
			OrbHighlight.InsideObscuredColor = spawnerColor;
			OrbHighlight.ObscuredColor = spawnerColor;
		}
		else
		{
			var spawnerColor = new Color(COLOR_TRAP);
			PhysicalModelRenderer.skinnedModel.Tint = spawnerColor;
			OrbHighlight.InsideObscuredColor = spawnerColor;
			OrbHighlight.ObscuredColor = spawnerColor;
		}
	}
}
