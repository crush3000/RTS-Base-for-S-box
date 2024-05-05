using Sandbox;
using System;

class Unit : Component
{
	[Property] public UnitModelBase PhysicalModel { get; set; }
	[Property] public NavMeshAgent UnitNavAgent { get; set; }
	[Property] public Collider UnitMeleeCollider { get; set; }
	[Property] public Collider UnitAutoMeleeCollider { get; set; }
	[Property] public Collider UnitRangedAttackCollider { get; set; }

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
	private const string MELEE_COLLIDER_TAG = "melee_collider";
	private const string UNIT_TAG = "unit";

	private float lastMeleeTime = Time.Now;

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
				if(targetUnit != null )
				{
					UnitNavAgent.MoveTo( targetUnit.Transform.Position );
				}
				//Reset if unit is killed or deleted or something
				else
				{
					commandGiven = UnitModelUtils.CommandType.None;
					UnitNavAgent.Stop();
				}
			}
			// Move Command
			else if (commandGiven == UnitModelUtils.CommandType.Move )
			{
				UnitNavAgent.MoveTo( homeTargetLocation );
				commandGiven = UnitModelUtils.CommandType.None;
			}
		}
		// Move To closeby enemy
		else if( tempTargetUnit != null)
		{
			if( tempTargetUnit.Transform.Position.Distance( homeTargetLocation ) < maxChaseDistanceFromHome )
			{
				UnitNavAgent.MoveTo(tempTargetUnit.Transform.Position);
			}
			else
			{
				UnitNavAgent.MoveTo( homeTargetLocation );
			}
		}
		else
		{
			UnitNavAgent.MoveTo( homeTargetLocation );
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
		if(UnitAutoMeleeCollider != null && tempTargetUnit == null )
		{
			// Get touching trigger colliders
			var collidersInAutoMeleeRange = UnitAutoMeleeCollider.Touching;
			var validUnitFound = false;
			// Select colliders belonging to Units
			if(collidersInAutoMeleeRange.Where(col => col.Tags.Has(UNIT_TAG)).Any())
			{
				// Select only melee colliders
				foreach ( var collision in collidersInAutoMeleeRange.Where( col => col == (col.GameObject.Components.Get<Unit>()).UnitMeleeCollider ))
				{
					var unitCollidedWith = collision.GameObject.Components.GetAll().OfType<Unit>().First();
					// If it is a unit of the opposite team
					if (unitCollidedWith.team != team)
					{
						//Log.Info( this.GameObject.Name + " will attack " + collisions.GameObject.Name + "!" );
						tempTargetUnit = unitCollidedWith;
						validUnitFound = true;
					}
				}
			}
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
		if(UnitRangedAttackCollider != null )
		{
			UnitRangedAttackCollider.Enabled = false;
			UnitRangedAttackCollider.Destroy();
		}
		this.Enabled = false;
		base.OnDestroy();

	}

	public void SelectUnit()
	{
		selected = true;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Selected );
	}

	public void DeSelectUnit()
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

		//TODO
	}
}
