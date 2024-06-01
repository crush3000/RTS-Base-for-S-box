using Sandbox;
using System;

class Unit : Component
{
	[Group( "Gameplay" )]
	[Property] public int team { get; set; }
	[Group( "Gameplay" )]
	[Property] public Vector3 UnitSize { get; set; }
	[Group( "Gameplay" )]
	[Property] public float UnitSpeed { get; set; }
	[Group( "Gameplay" )]
	[Property] public int UnitMaxHealth { get; set; }
	[Group( "Gameplay" )]
	[Property] public bool HasMeleeAttack { get; set; }
	[Group( "Gameplay" )]
	[Property] public int MeleeAttackDamage { get; set; }
	[Group( "Gameplay" )]
	[Property] public float MeleeAttackSpeed { get; set; }
	[Group( "Gameplay" )]
	[Property] public bool HasRangedAttack { get; set; }
	[Group( "Gameplay" )]
	[Property] public float RangedAttackRange { get; set; }
	[Group( "Gameplay" )]
	[Property] public int RangedAttackDamage { get; set; }
	[Group( "Gameplay" )]
	[Property] public float RangedAttackSpeed { get; set; }

	[Group( "Visuals" )]
	[Property] public Model UnitModelFile { get; set; }
	[Group( "Visuals" )]
	[Property] public AnimationGraph UnitAnimGraph { get; set; }
	[Group( "Visuals" )]
	[Property] public Material UnitModelMaterial { get; set; }
	[Group( "Visuals" )]
	[Property] public UnitModelBase PhysicalModelRenderer { get; set; }
	[Group( "Visuals" )]
	[Property] public HealthBar UnitHealthBar { get; set; }

	[Group( "Triggers And Collision" )]
	[Property] public UnitTriggerListener TriggerListener { get; set; }
	[Group( "Triggers And Collision" )]
	[Property] public CapsuleCollider UnitMeleeCollider { get; set; }
	[Group( "Triggers And Collision" )]
	[Property] public SphereCollider UnitAutoMeleeCollider { get; set; }
	[Group( "Triggers And Collision" )]
	[Property] public Collider UnitRangedAttackCollider { get; set; }
	[Group( "Triggers And Collision" )]
	[Property] public BoxCollider SelectionHitbox { get; set; }
	[Group( "Triggers And Collision" )]
	[Property] public NavMeshAgent UnitNavAgent { get; set; }

	// Class Vars
	bool selected { get; set; }
	public UnitModelUtils.CommandType commandGiven { get; set; }
	public Vector3 homeTargetLocation { get; set; }
	public Unit targetUnit { get; set; }
	public Unit tempTargetUnit { get; set; }

	// This will be a factor of the unit size I imagine
	private float maxChaseDistanceFromHome = 600f;
	private int currentHealthPoints = 100;
	private float lastMeleeTime = Time.Now;
	private float lastMoveOrderTime = Time.Now;

	// Unit Constants
	private const string UNIT_TAG = "unit";
	private const float MOVE_ORDER_FREQUENCY = .1f;
	private const float CHASE_DIST_MULTIPLIER = 30f;
	private const float AUTO_MELEE_RAD_MULTIPLIER = 30f;
	private const float NAV_AGENT_RAD_MULTIPLIER = .5f;
	private const float CLICK_HITBOX_RADIUS_MULTIPLIER = .5f;

	protected override void OnStart()
	{
		base.OnStart();
		commandGiven = UnitModelUtils.CommandType.None;
		homeTargetLocation = Transform.Position;
		currentHealthPoints = UnitMaxHealth;
		setRelativeUnitSizeHelper(UnitSize);
		PhysicalModelRenderer.setModel( UnitModelFile, UnitAnimGraph, UnitModelMaterial );
		if ( team != RTSGame.Instance.ThisPlayer.Team)
		{
			UnitHealthBar.setBarColor( "red" );
			UnitHealthBar.setShowHealthBar( true );
		}
		else
		{
			UnitHealthBar.setBarColor( "#40ff40" );
			UnitHealthBar.setShowHealthBar( false );
		}
		Tags.Add( UNIT_TAG );
	}

	protected override void OnUpdate()
	{
		// Handle Player Commands
		if ( commandGiven != UnitModelUtils.CommandType.None )
		{
			// Attack Command
			if(commandGiven == UnitModelUtils.CommandType.Attack)
			{
				if(targetUnit.Enabled == true )
				{
					move( targetUnit.Transform.Position, false );
				}
				//Reset if unit is killed or deleted or something
				else
				{
					commandGiven = UnitModelUtils.CommandType.None;
					stopMoving();
				}
			}
			// Move Command
			else if (commandGiven == UnitModelUtils.CommandType.Move )
			{
				move( homeTargetLocation, true );
				commandGiven = UnitModelUtils.CommandType.None;
			}
		}
		// Move To closeby enemy
		else if( tempTargetUnit != null)
		{
			//Log.Info( tempTargetUnit.GameObject.Name);
			if( tempTargetUnit.Transform.Position.Distance( homeTargetLocation ) < maxChaseDistanceFromHome )
			{
				move( tempTargetUnit.Transform.Position, false);
			}
			else
			{
				move( homeTargetLocation, false );
			}
		}
		else
		{
			move( homeTargetLocation, false );
		}

		// Handle Attacks
		// Attack Unit in melee range
		if ( UnitMeleeCollider != null ) 
		{
			// Get touching trigger colliders
			var collidersInMeleeRange = UnitMeleeCollider.Touching;
			// Select colliders belonging to Units
			if ( collidersInMeleeRange.Where( col => col.Tags.Has( UNIT_TAG ) ).Any() )
			{
				// Select only melee colliders
				foreach ( var collision in collidersInMeleeRange.Where( col => col == (col.GameObject.Components.Get<Unit>()).UnitMeleeCollider ))
				{
					var unitCollidedWith = collision.GameObject.Components.GetAll().OfType<Unit>().First();
					// If it is a unit of the opposite team
					if ( unitCollidedWith.team != team) 
					{
						if (Time.Now - lastMeleeTime > MeleeAttackSpeed )
						{
							//Log.Info( this.GameObject.Name + " attacks " + collisions.GameObject.Name + " for " + melee_attack_damage + " damage!" );
							directMeleeAttack( unitCollidedWith);
						}
					}
				}
			}
		}
		// Auto Melee
		if(UnitAutoMeleeCollider != null)
		{
			var validUnitFound = false;
			if ( tempTargetUnit == null )
			{
				// Get touching trigger colliders
				var collidersInAutoMeleeRange = UnitAutoMeleeCollider.Touching;
				// Select colliders belonging to Units
				if ( collidersInAutoMeleeRange.Where( col => col.Tags.Has( UNIT_TAG ) ).Any() )
				{
					// Select only melee colliders
					foreach ( var collision in collidersInAutoMeleeRange.Where( col => col == (col.GameObject.Components.Get<Unit>()).UnitMeleeCollider ) )
					{
						var unitCollidedWith = collision.GameObject.Components.GetAll().OfType<Unit>().First();
						// If it is a unit of the opposite team
						if ( unitCollidedWith.team != team )
						{
							//Log.Info( this.GameObject.Name + " will attack " + collisions.GameObject.Name + "!" );
							tempTargetUnit = unitCollidedWith;
							validUnitFound = true;
						}
					}
				}
			}
			// Reset automelee if nothing in range
			if(validUnitFound != true) 
			{
				tempTargetUnit = null;
			}
		}

		// Handle Animations
		if (PhysicalModelRenderer != null && UnitNavAgent != null) 
		{
			if ( !UnitNavAgent.Velocity.IsNearZeroLength )
			{
				PhysicalModelRenderer.animateMovement(UnitNavAgent.Velocity, UnitNavAgent.WishVelocity);
			}
			else
			{
				PhysicalModelRenderer.stopMovementAnimate();
			}
		}
	}

	// Cleanup
	protected override void OnDestroy()
	{
		PhysicalModelRenderer.Enabled = false;
		PhysicalModelRenderer.Destroy();
		UnitNavAgent.Enabled = false;
		UnitNavAgent.Destroy();
		UnitMeleeCollider.Enabled = false;
		UnitMeleeCollider.Destroy();
		UnitAutoMeleeCollider.Enabled = false;
		UnitAutoMeleeCollider.Destroy();
		SelectionHitbox.Enabled = false;
		SelectionHitbox.Destroy();
		UnitHealthBar.Enabled = false;
		UnitHealthBar.Destroy();
		if(UnitRangedAttackCollider != null )
		{
			UnitRangedAttackCollider.Enabled = false;
			UnitRangedAttackCollider.Destroy();
		}
		this.Enabled = false;
		base.OnDestroy();

	}

	public void selectUnit()
	{
		selected = true;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.Selected );
		UnitHealthBar.setShowHealthBar(true);
	}

	public void deSelectUnit()
	{
		selected = false;
		PhysicalModelRenderer.setOutlineState( UnitModelUtils.OutlineState.Mine );
		UnitHealthBar.setShowHealthBar(false);
	}

	public void takeDamage(int damage)
	{
		//Log.Info( this.GameObject.Name + " takes " + damage + " damage!");
		PhysicalModelRenderer.animateDamageTaken();
		currentHealthPoints -= damage;
		UnitHealthBar.setHealth( currentHealthPoints, UnitMaxHealth);
		if( currentHealthPoints <= 0 )
		{
			die();
		}
	}

	public void move(Vector3 location, bool isNewMoveCommand)
	{
		if(location != UnitNavAgent.TargetPosition)
		{
			if(isNewMoveCommand || Time.Now - lastMoveOrderTime >= MOVE_ORDER_FREQUENCY )
			{
				lastMoveOrderTime = Time.Now;
				//Log.Info( "Move Command Sent: " + UnitNavAgent.TargetPosition );
				UnitNavAgent.MoveTo( location );
			}
		}
	}

	public void stopMoving()
	{
		homeTargetLocation = Transform.Position;
		UnitNavAgent.Stop();
	}

	private void die()
	{
		//Log.Info( this.GameObject.Name + " dies!" );
		PhysicalModelRenderer.animateDeath();
		Destroy();
	}

	private void directMeleeAttack(Unit targetUnit)
	{
		PhysicalModelRenderer.animateMeleeAttack();
		targetUnit.takeDamage( MeleeAttackDamage );
		lastMeleeTime = Time.Now;
	}

	private void setRelativeUnitSizeHelper(Vector3 unitSize)
	{
		// The scale is going to be calculated from the ratio of the default model size and the unit's given size modified by a global scaling constant
		Vector3 defaultModelSize = UnitModelFile.Bounds.Size;
		
		Vector3 globalScaleModifier = Vector3.One * Scene.GetAllObjects( true ).Where( go => go.Name == "RTSGameOptions" ).First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue( RTSGameOptionsComponent.GLOBAL_UNIT_SCALE );
		Vector3 targetModelSize = new Vector3((unitSize.x * globalScaleModifier.x), (unitSize.y * globalScaleModifier.y), (unitSize.z * globalScaleModifier.z));
		float targetxyMin = float.Min( targetModelSize.x, targetModelSize.y );
		float targetxyMax = float.Max( targetModelSize.x, targetModelSize.y );
		float defaultxyMin = float.Min( defaultModelSize.x, defaultModelSize.y );
		float defaultxyMax = float.Max( defaultModelSize.x, defaultModelSize.y );
		//Log.Info("defaultModelSize: " +  defaultModelSize);
		//Log.Info("Target Model Size: " + targetModelSize );
		//Log.Info( "Calculated Scale: " + new Vector3(
			//((unitSize.x * globalScaleModifier.x) / defaultModelSize.x),
			//((unitSize.y * globalScaleModifier.y) / defaultModelSize.y),
			//((unitSize.z * globalScaleModifier.z) / defaultModelSize.z)
			//));
		Transform.LocalScale = new Vector3(
			(targetModelSize.x / defaultModelSize.x),
			(targetModelSize.y / defaultModelSize.y),
			(targetModelSize.z / defaultModelSize.z)
			);

		// Auto calculate unit's nav agent size
		UnitNavAgent.Height = targetModelSize.z;
		UnitNavAgent.Radius = targetxyMin * NAV_AGENT_RAD_MULTIPLIER;

		// Auto calculate unit's melee collider size
		UnitMeleeCollider.Radius = defaultxyMax;
		UnitMeleeCollider.Start = Vector3.Zero;
		UnitMeleeCollider.End = new Vector3(0, 0, defaultModelSize.z);

		// Auto calculate unit's auto melee collider size
		UnitAutoMeleeCollider.Center = Vector3.Zero;
		UnitAutoMeleeCollider.Radius = AUTO_MELEE_RAD_MULTIPLIER * targetxyMax;

		// Auto calculate unit's ranged attack range collider size


		// Auto calculate unit's Selection Collider scaling and relative position
		SelectionHitbox.Center = new Vector3( 0, 0, defaultModelSize.z / 2 );
		SelectionHitbox.Scale = new Vector3( defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultxyMin * CLICK_HITBOX_RADIUS_MULTIPLIER, defaultModelSize.z );

		// Auto calculate unit's chase distance
		maxChaseDistanceFromHome = CHASE_DIST_MULTIPLIER * targetxyMax;
	}
}
