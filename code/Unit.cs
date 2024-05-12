using Sandbox;
using System;

class Unit : Component
{
	[Property] public UnitModelBase PhysicalModel { get; set; }
	[Property] public NavMeshAgent UnitNavAgent { get; set; }
	[Property] public Collider UnitMeleeCollider { get; set; }
	[Property] public Collider UnitAutoMeleeCollider { get; set; }
	[Property] public Collider UnitRangedAttackCollider { get; set; }
	[Property] public Collider SelectionHitbox { get; set; }

	[Property] public int team { get; set; }
	[Property] public Vector3 UnitSize { get; set; }
	[Property] public float UnitSpeed { get; set; }
	[Property] public int UnitMaxHealth { get; set; }
	[Property] public bool HasMeleeAttack { get; set; }
	[Property] public int MeleeAttackDamage { get; set; }
	[Property] public float MeleeAttackSpeed { get; set; }
	[Property] public bool HasRangedAttack { get; set; }
	[Property] public int RangedAttackDamage { get; set; }
	[Property] public float RangedAttackSpeed { get; set; }
	//[Property] public UnitType ThisUnitType { get; set; }
	[Property] public UnitTriggerListener TriggerListener { get; set; }

	bool selected { get; set; }
	public UnitModelUtils.CommandType commandGiven { get; set; }
	public Vector3 homeTargetLocation { get; set; }
	public Unit targetUnit { get; set; }
	public Unit tempTargetUnit { get; set; }

	// This will be a factor of the unit size I imagine
	private float maxChaseDistanceFromHome = 600f;

	private int currentHealthPoints = 100;
	private const string UNIT_TAG = "unit";

	private float lastMeleeTime = Time.Now;

	private float lastMoveOrderTime = Time.Now;
	private const float MOVE_ORDER_FREQUENCY = .1f;

	protected override void OnStart()
	{
		base.OnStart();
		commandGiven = UnitModelUtils.CommandType.None;
		homeTargetLocation = Transform.Position;
		currentHealthPoints = UnitMaxHealth;
		setRelativeUnitSizeHelper(UnitSize);
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
		if (PhysicalModel != null && UnitNavAgent != null) 
		{
			if ( !UnitNavAgent.Velocity.IsNearZeroLength )
			{
				PhysicalModel.animateMovement(UnitNavAgent.Velocity, UnitNavAgent.WishVelocity);
			}
			else
			{
				PhysicalModel.stopMovementAnimate();
			}
		}
	}

	// Cleanup
	protected override void OnDestroy()
	{
		PhysicalModel.Enabled = false;
		PhysicalModel.Destroy();
		UnitNavAgent.Enabled = false;
		UnitNavAgent.Destroy();
		UnitMeleeCollider.Enabled = false;
		UnitMeleeCollider.Destroy();
		UnitAutoMeleeCollider.Enabled = false;
		UnitAutoMeleeCollider.Destroy();
		SelectionHitbox.Enabled = false;
		SelectionHitbox.Destroy();
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
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Selected );
	}

	public void deSelectUnit()
	{
		selected = false;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Mine );
	}

	public void takeDamage(int damage)
	{
		//Log.Info( this.GameObject.Name + " takes " + damage + " damage!");
		PhysicalModel.animateDamageTaken();
		currentHealthPoints -= damage;
		if( currentHealthPoints < 0 )
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
		PhysicalModel.animateDeath();
		Destroy();
	}

	private void directMeleeAttack(Unit targetUnit)
	{
		PhysicalModel.animateMeleeAttack();
		targetUnit.takeDamage( MeleeAttackDamage );
		lastMeleeTime = Time.Now;
	}

	private void setRelativeUnitSizeHelper(Vector3 unitSize)
	{
		// The scale is going to be calculated from the ratio of the default model size and the unit's given size modified by a global scaling constant
		Vector3 defaultModelSize = PhysicalModel.model.Bounds.Size;
		Vector3 globalScaleModifier = Vector3.One * Scene.GetAllObjects( true ).Where( go => go.Name == "RTSGameOptions" ).First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue( RTSGameOptionsComponent.GLOBAL_UNIT_SCALE );
		Log.Info("defaultModelSize: " +  defaultModelSize);
		Log.Info("scaleModifier: " +  globalScaleModifier);
		Log.Info("unitSize: " +  unitSize);
		Log.Info( "Calculated Scale: " + new Vector3(
			((unitSize.x * globalScaleModifier.x) / defaultModelSize.x),
			((unitSize.y * globalScaleModifier.y) / defaultModelSize.y),
			((unitSize.z * globalScaleModifier.z) / defaultModelSize.z)
			));
		Transform.LocalScale = new Vector3(
			((unitSize.x * globalScaleModifier.x) / defaultModelSize.x),
			((unitSize.y * globalScaleModifier.y) / defaultModelSize.y),
			((unitSize.z * globalScaleModifier.z) / defaultModelSize.z)
			);
		//Transform.LocalScale = Vector3.One * Scene.GetAllObjects( true ).Where( go => go.Name == "RTSGameOptions" ).First().Components.GetAll<RTSGameOptionsComponent>().First().getFloatValue(RTSGameOptionsComponent.GLOBAL_UNIT_SCALE);
	}
}
