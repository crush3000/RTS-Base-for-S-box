
using Sandbox.UI;
using System.Drawing;

public class SkinnedRTSObject : SelectableObject, IScalable, IDamageable, ISelectable
{
	[Group( "Gameplay" )]
	[Property] public int MaxHealth { get; set; }

	[Group( "Visuals" )]
	[Property] public AnimationGraph AnimGraph { get; set; }

	[Group( "Visuals" )]
	[Property] public HealthBar ThisHealthBar { get; set; }




	// Class Vars
	bool selected { get; set; }

	[Sync] public int currentHealthPoints { get; private set; }

	// Constants
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = 1f;

	protected override void OnStart()
	{
		//TODO there is a bug where units can attack this one before it fully initializes its size or is able to fight back. Need a solution
		setRelativeSizeHelper(Size);
		//base.OnStart();
		buttons = new List<DynamicButton>();
		PhysicalModelRenderer.setModel( ModelFile, AnimGraph, ModelMaterial );
		onTeamChange();
		Tags.Add( objectTypeTag );
		if (!Network.IsOwner) { return; }
		setHealth(MaxHealth);
	}

	public override void select()
	{
		if(!Network.IsOwner) {  return; }
		selected = true;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.Selected );
		ThisHealthBar.setShowHealthBar( true );
	}

	public override void deSelect()
	{
		if (!Network.IsOwner) { return; }
		selected = false;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.None );
		ThisHealthBar.setShowHealthBar( false );
	}

	public virtual void takeDamage( int damage )
	{
		//Log.Info( this.GameObject.Name + " takes " + damage + " damage!");
		PhysicalModelRenderer.animateDamageTaken();
		setHealth(currentHealthPoints - damage);
		if ( currentHealthPoints <= 0 )
		{
			die();
		}
	}

	private void setHealth(int newHealth)
	{
		currentHealthPoints = newHealth;
		ThisHealthBar.setHealth(newHealth, MaxHealth);
	}

	//[Broadcast]
	public virtual void die()
	{
		//Log.Info( this.GameObject.Name + " dies!" );
		PhysicalModelRenderer.animateDeath();
		Destroy();
	}

	public override void setRelativeSizeHelper( Vector3 unitSize )
	{
		// The scale is going to be calculated from the ratio of the default model size and the object's given size modified by a global scaling constant
		Vector3 defaultModelSize = ModelFile.Bounds.Size;

		Vector3 globalScaleModifier = Vector3.One * Scene.GetAllObjects( true ).Where( go => go.Name == "RTSGameOptions" ).First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue( RTSGameOptionsComponent.GLOBAL_UNIT_SCALE );
		Vector3 targetModelSize = new Vector3( (unitSize.x * globalScaleModifier.x), (unitSize.y * globalScaleModifier.y), (unitSize.z * globalScaleModifier.z) );
		float targetxyMin = float.Min( targetModelSize.x, targetModelSize.y );
		float targetxyMax = float.Max( targetModelSize.x, targetModelSize.y );
		float defaultxyMin = float.Min( defaultModelSize.x, defaultModelSize.y );
		float defaultxyMax = float.Max( defaultModelSize.x, defaultModelSize.y );

		Transform.LocalScale = new Vector3(
			(targetModelSize.x / defaultModelSize.x),
			(targetModelSize.y / defaultModelSize.y),
			(targetModelSize.z / defaultModelSize.z)
			);

		// Auto calculate object's Selection Collider scaling and relative position
		SelectionHitbox.Center = new Vector3( 0, 0, defaultModelSize.z / 2 );
		SelectionHitbox.Scale = new Vector3( defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultModelSize.z );

		// Auto Calculate other visual element sizes
		PhysicalModelRenderer.setModelSize( defaultModelSize );
		ThisHealthBar.setSize( defaultModelSize );
	}

	public override List<DynamicButton> getDynamicButtons()
	{
		return buttons;
	}

	public void onTeamChange()
	{
		if (team == RTSPlayer.Local.Team)
		{
			PhysicalModelRenderer.setOutlineState(UnitModelUtils.OutlineState.Mine);
			ThisHealthBar.setBarColor("#40ff40");
			ThisHealthBar.setShowHealthBar(false);
		}
		else
		{
			PhysicalModelRenderer.setOutlineState(UnitModelUtils.OutlineState.Hostile);
			ThisHealthBar.setBarColor("red");
			ThisHealthBar.setShowHealthBar(true);
		}
	}

	public override void setTeam(int newTeam)
	{
		base.setTeam(newTeam);
		onTeamChange();
	}
}
