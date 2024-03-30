using System;

class Unit : Component
{
	[Property] public ModelRenderer UnitModel { get; set; }
	[Property] Collider UnitCollider { get; set; }
	[Property] public NavMeshAgent UnitNavAgent { get; set; }

	Vector3 UnitSize { get; set; }
	//Enum UnitType { get; set; }
	public bool CommandGiven { get; set; }
	public Vector3 TargetLocation { get; set; }

	protected override void OnUpdate()
	{
		if ( CommandGiven )
		{
			UnitNavAgent.MoveTo( TargetLocation );
		}
		CommandGiven = false;
	}
}
