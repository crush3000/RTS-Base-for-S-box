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

	Vector3 UnitSize { get; set; }
	//Enum UnitType { get; set; }
	//Enum UnitState { get; set; }
	bool Selected { get; set; }
	public UnitModelUtils.CommandType CommandGiven { get; set; }
	public Vector3 TargetLocation { get; set; }

	public Unit TargetUnit { get; set; }

	private int health_points = 100;

	private int melee_attack_damage = 20;

	private float melee_attack_speed = 1.5f;

	private float last_melee_time = Time.Now;

	//public Unit()
	//{
	//	Log.Info("Creating Unit");
	//	PhysicalModel = new UnitModel();
	//	UnitNavAgent = new NavMeshAgent();
	//	UnitSize = new Vector3(60, 60);
	//	Selected = false;
	//	CommandGiven = false;
	//	TargetLocation = Vector3.Zero;
	//}

	//protected override void OnAwake()
	//{
		//base.OnAwake();
		//Log.Info( "Creating Unit for " + this.GameObject );
		//PhysicalModel = new UnitModel();

		//UnitNavAgent = new NavMeshAgent();
		//UnitNavAgent.Height = 20;
		//UnitNavAgent.Radius = 5;
		//UnitNavAgent.MaxSpeed = 60;
		//UnitNavAgent.Acceleration = 600;
		//UnitNavAgent.UpdatePosition = true;
		//UnitNavAgent.UpdateRotation = true;

		//UnitSize = new Vector3( 60, 60 );
		//Selected = false;
		//CommandGiven = false;
		//TargetLocation = Vector3.Zero;
	//}

	//protected override void OnStart()
	//{
		//base.OnStart();
		//PhysicalModel = new UnitModelBasic();
		//UnitNavAgent = new NavMeshAgent();
		//team = 99;
		//UnitSize = new Vector3( 5, 5, 5 );
	//}

	protected override void OnStart()
	{
		base.OnStart();
		CommandGiven = UnitModelUtils.CommandType.None;
	}

	protected override void OnUpdate()
	{
		// Handle Move Command
		if ( CommandGiven != UnitModelUtils.CommandType.None )
		{
			if(CommandGiven == UnitModelUtils.CommandType.Attack)
			{
				if(TargetUnit != null )
				{
					UnitNavAgent.MoveTo( TargetUnit.Transform.Position );
				}
				//Reset if unit is killed or deleted or something
				else
				{
					CommandGiven = UnitModelUtils.CommandType.None;
				}

			}
			else if(CommandGiven == UnitModelUtils.CommandType.Move )
			{
				UnitNavAgent.MoveTo( TargetLocation );
				CommandGiven = UnitModelUtils.CommandType.None;
			}
		}

		// Handle Attacks
		if( UnitMeleeCollider != null ) 
		{
			var unitsInMeleeRange = UnitMeleeCollider.Touching;
			if ( unitsInMeleeRange.Any())
			{
				foreach(var collisions in unitsInMeleeRange)
				{
					var collidedObjectGOComponents = collisions.GameObject.Components.GetAll();
					if ( collidedObjectGOComponents.OfType<Unit>().Any() && collidedObjectGOComponents.OfType<Unit>().First().team != team) 
					{
						if(Time.Now - last_melee_time > melee_attack_speed )
						{
							Log.Info( this.GameObject.Name + " attacks " + collisions.GameObject.Name + " for " + melee_attack_damage + " damage!" );
							directMeleeAttack( collidedObjectGOComponents.OfType<Unit>().First());
						}
					}
				}
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

	//TODO GOTTA FIGURE OUT HOW TO MAKE IT ACTUALLY DISSAPEAR
	protected override void OnDestroy()
	{
		PhysicalModel.Enabled = false;
		PhysicalModel.Destroy();
		UnitNavAgent.Enabled = false;
		UnitNavAgent.Destroy();
		UnitMeleeCollider.Enabled = false;
		UnitMeleeCollider.Destroy();
		this.Enabled = false;
		base.OnDestroy();

	}

	public void SelectUnit()
	{
		Selected = true;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Selected );
	}

	public void DeSelectUnit()
	{
		Selected = false;
		PhysicalModel.setOutlineState( UnitModelUtils.OutlineState.Mine );
	}

	public void takeDamage(int damage)
	{
		Log.Info( this.GameObject.Name + " takes " + damage + " damage!");
		PhysicalModel.animateDamageTaken();
		health_points -= damage;
		if( health_points < 0 )
		{
			die();
		}
	}

	private void die()
	{
		Log.Info( this.GameObject.Name + " dies!" );
		PhysicalModel.animateDeath();
		Destroy();
	}

	private void directMeleeAttack(Unit targetUnit)
	{
		PhysicalModel.animateMeleeAttack();
		targetUnit.takeDamage( melee_attack_damage );
		last_melee_time = Time.Now;
	}
}
