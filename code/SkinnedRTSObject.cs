
using Sandbox.UI;
using System.Drawing;

public class SkinnedRTSObject : Component, IScalable, IDamageable, ISelectable
{
	[Group( "Gameplay" )]
	[Property] public string name { get; set; }
	[Group( "Gameplay" )]
	[Property] public int team { get; set; }
	[Group( "Gameplay" )]
	[Property] public Vector3 Size { get; set; }
	[Group( "Gameplay" )]
	[Property] public int MaxHealth { get; set; }

	[Group( "Visuals" )]
	[Property] public Model ModelFile { get; set; }
	[Group( "Visuals" )]
	[Property] public AnimationGraph AnimGraph { get; set; }
	[Group( "Visuals" )]
	[Property] public Material ModelMaterial { get; set; }
	[Group( "Visuals" )]
	[Property] public UnitModelBase PhysicalModelRenderer { get; set; }
	[Group( "Visuals" )]
	[Property] public HealthBar ThisHealthBar { get; set; }

	[Group( "Triggers And Collision" )]
	[Property] public BoxCollider SelectionHitbox { get; set; }

	[Group( "User Interface" )]
	[Property] public string PortraitImage { get; set; }



	// Class Vars
	bool selected { get; set; }

	public int currentHealthPoints;
	protected string objectTypeTag = "";

	// Constants
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = 1f;

	protected override void OnStart()
	{
		Log.Info( "Base Object OnStart" );
		base.OnStart();
		currentHealthPoints = MaxHealth;
		setRelativeSizeHelper( Size );
		PhysicalModelRenderer.setModel( ModelFile, AnimGraph, ModelMaterial );
		if ( team != RTSPlayer.Local.Team )
		{
			ThisHealthBar.setBarColor( "red" );
			ThisHealthBar.setShowHealthBar( true );
			PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.Hostile );	
		}
		else
		{
			ThisHealthBar.setBarColor( "#40ff40" );
			ThisHealthBar.setShowHealthBar( false );
			PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.None );
		}
		Tags.Add( objectTypeTag );
	}

	// Cleanup
	/*protected override void OnDestroy()
	{
		Log.Info( "Base Object OnDestroy" );
		PhysicalModelRenderer.Enabled = false;
		PhysicalModelRenderer.Destroy();
		SelectionHitbox.Enabled = false;
		SelectionHitbox.Destroy();
		ThisHealthBar.Enabled = false;
		ThisHealthBar.Destroy();
		this.Enabled = false;
		base.OnDestroy();
	}*/

	public virtual void select()
	{
		selected = true;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.Selected );
		ThisHealthBar.setShowHealthBar( true );
	}

	public virtual void deSelect()
	{
		selected = false;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.None );
		ThisHealthBar.setShowHealthBar( false );
	}

	public virtual void takeDamage( int damage )
	{
		//Log.Info( this.GameObject.Name + " takes " + damage + " damage!");
		PhysicalModelRenderer.animateDamageTaken();
		currentHealthPoints -= damage;
		ThisHealthBar.setHealth( currentHealthPoints, MaxHealth );
		if ( currentHealthPoints <= 0 )
		{
			die();
		}
	}

	public virtual void die()
	{
		//Log.Info( this.GameObject.Name + " dies!" );
		PhysicalModelRenderer.animateDeath();
		Destroy();
	}

	public virtual void setRelativeSizeHelper( Vector3 unitSize )
	{
		Log.Info( "Base Object Size Func" );
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
}
