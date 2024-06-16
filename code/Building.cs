
class Building: SkinnedRTSObject
{
	[Group( "Gameplay" )]
	[Property] public bool HasRangedAttack { get; set; }
	[Group( "Gameplay" )]
	[Property] public float RangedAttackRange { get; set; }
	[Group( "Gameplay" )]
	[Property] public int RangedAttackDamage { get; set; }
	[Group( "Gameplay" )]
	[Property] public float RangedAttackSpeed { get; set; }

	[Group( "Triggers And Collision" )]
	[Property] public Collider UnitRangedAttackCollider { get; set; }



	// Class Vars
	bool selected { get; set; }
	//public UnitModelUtils.CommandType commandGiven { get; set; }
	//public Unit targetUnit { get; set; }

	// Unit Constants
	public const string BUILDING_TAG = "building";
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = 1f;

	protected override void OnStart()
	{
		Log.Info( "Building Object OnStart" );
		objectTypeTag = BUILDING_TAG;
		base.OnStart();
		foreach ( var tag in Tags )
		{
			Log.Info( tag );
		}
		PhysicalModelRenderer.outline.Enabled = false;
	}

	protected override void OnUpdate()
	{
		// Attack Command
		/*if ( commandGiven == UnitModelUtils.CommandType.Attack )
		{
			if ( targetUnit.Enabled == true )
			{
				move( targetUnit.Transform.Position, false );
			}
			//Reset if unit is killed or deleted or something
			else
			{
				commandGiven = UnitModelUtils.CommandType.None;
				stopMoving();
			}
		}*/


		// Handle Animations
		
	}

	// Cleanup
	protected override void OnDestroy()
	{
		Log.Info( "Building Object OnDestroy" );
		if ( UnitRangedAttackCollider != null )
		{
			UnitRangedAttackCollider.Enabled = false;
			UnitRangedAttackCollider.Destroy();
		}
		base.OnDestroy();
	}


	public override void takeDamage( int damage )
	{
		Log.Info( GameObject.Name + " takes " + damage + " damage!\n It now has " + currentHealthPoints + " health.");
		//PhysicalModelRenderer.animateDamageTaken();
		currentHealthPoints -= damage;
		ThisHealthBar.setHealth( currentHealthPoints, MaxHealth );
		if ( currentHealthPoints <= 0 )
		{
			die();
		}
	}

	public override void die()
	{
		//Log.Info( this.GameObject.Name + " dies!" );
		this.PhysicalModelRenderer.animateDeath();
		Destroy();
	}

	public override void setRelativeSizeHelper( Vector3 unitSize )
	{
		Log.Info( "Building Object Size Func" );
		// The scale is going to be calculated from the ratio of the default model size and the unit's given size modified by a global scaling constant
		Vector3 defaultModelSize = this.ModelFile.Bounds.Size;

		Vector3 globalScaleModifier = Vector3.One * Scene.GetAllObjects( true ).Where( go => go.Name == "RTSGameOptions" ).First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue( RTSGameOptionsComponent.GLOBAL_UNIT_SCALE );
		Vector3 targetModelSize = new Vector3( (unitSize.x * globalScaleModifier.x), (unitSize.y * globalScaleModifier.y), (unitSize.z * globalScaleModifier.z) );
		float targetxyMin = float.Min( targetModelSize.x, targetModelSize.y );
		float targetxyMax = float.Max( targetModelSize.x, targetModelSize.y );
		float defaultxyMin = float.Min( defaultModelSize.x, defaultModelSize.y );
		float defaultxyMax = float.Max( defaultModelSize.x, defaultModelSize.y );

		Log.Info( "defaultModelSize: " + defaultModelSize );
		Log.Info( "Target Model Size: " + targetModelSize );
		Log.Info( "Calculated Scale: " + new Vector3(
			((unitSize.x * globalScaleModifier.x) / defaultModelSize.x),
			((unitSize.y * globalScaleModifier.y) / defaultModelSize.y),
			((unitSize.z * globalScaleModifier.z) / defaultModelSize.z)
			) );

		Transform.LocalScale = new Vector3(
			(targetModelSize.x / defaultModelSize.x),
			(targetModelSize.y / defaultModelSize.y),
			(targetModelSize.z / defaultModelSize.z)
			);

		// Auto calculate unit's ranged attack range collider size


		// Auto calculate unit's Selection Collider scaling and relative position
		this.SelectionHitbox.Center = new Vector3( 0, 0, defaultModelSize.z / 2 );
		this.SelectionHitbox.Scale = new Vector3( defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultModelSize.z );

		// Auto Calculate other visual element sizes
		PhysicalModelRenderer.setModelSize( defaultModelSize );
		ThisHealthBar.setSize( defaultModelSize );
	}
}
