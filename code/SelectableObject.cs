
using Sandbox.UI;
using System.Drawing;

public class SelectableObject : Component, IScalable, ISelectable
{
	[Group("Gameplay")]
	[Property] public string name { get; set; }

	[Group("Gameplay")]
	[Property] public Vector3 Size { get; set; }

	[Group("Visuals")]
	[Property] public Model ModelFile { get; set; }
	[Group("Visuals")]
	[Property] public Material ModelMaterial { get; set; }
	[Group("Visuals")]
	[Property] public UnitModelBase PhysicalModelRenderer { get; set; }

	[Group("Triggers And Collision")]
	[Property] public BoxCollider SelectionHitbox { get; set; }

	[Group("User Interface")]
	[Property] public string PortraitImage { get; set; }



	// Class Vars
	[Sync] public int team { get; private set; }
	bool selected { get; set; }

	protected string objectTypeTag = "";

	protected List<DynamicButton> buttons { get; set; }

	// Constants
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = 1f;

	protected override void OnStart()
	{
		//TODO there is a bug where units can attack this one before it fully initializes its size or is able to fight back. Need a solution
		setRelativeSizeHelper(Size);
		Log.Info("Selectable Object OnStart");
		base.OnStart();
		PhysicalModelRenderer.setModel(ModelFile, null, ModelMaterial);
		Tags.Add(objectTypeTag);
		buttons = new List<DynamicButton>();
	}

	public virtual void select()
	{
		if (!Network.IsOwner) { return; }
		selected = true;
		PhysicalModelRenderer.setOutlineState(UnitModelUtils.OutlineState.Selected);
	}

	public virtual void deSelect()
	{
		if (!Network.IsOwner) { return; }
		selected = false;
		PhysicalModelRenderer.setOutlineState(UnitModelUtils.OutlineState.None);
	}

	public virtual List<DynamicButton> getDynamicButtons()
	{
		return buttons;
	}

	public virtual void setRelativeSizeHelper(Vector3 unitSize)
	{
		Log.Info("Base Object Size Func");
		// The scale is going to be calculated from the ratio of the default model size and the object's given size modified by a global scaling constant
		Vector3 defaultModelSize = ModelFile.Bounds.Size;

		//Vector3 globalScaleModifier = Vector3.One * Scene.GetAllObjects(true).Where(go => go.Name == "RTSGameOptions").First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue(RTSGameOptionsComponent.GLOBAL_UNIT_SCALE);
		Vector3 targetModelSize = new Vector3((unitSize.x), (unitSize.y), (unitSize.z));
		float targetxyMin = float.Min(targetModelSize.x, targetModelSize.y);
		float targetxyMax = float.Max(targetModelSize.x, targetModelSize.y);
		float defaultxyMin = float.Min(defaultModelSize.x, defaultModelSize.y);
		float defaultxyMax = float.Max(defaultModelSize.x, defaultModelSize.y);

		Transform.LocalScale = new Vector3(
			(targetModelSize.x / defaultModelSize.x),
			(targetModelSize.y / defaultModelSize.y),
			(targetModelSize.z / defaultModelSize.z)
			);

		// Auto calculate object's Selection Collider scaling and relative position
		SelectionHitbox.Center = new Vector3(0, 0, defaultModelSize.z / 2);
		SelectionHitbox.Scale = new Vector3(defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultModelSize.z);

		// Auto Calculate other visual element sizes
		PhysicalModelRenderer.setModelSize(defaultModelSize);
	}

	public virtual void setTeam(int newTeam)
	{
		team = newTeam;
	}
}
