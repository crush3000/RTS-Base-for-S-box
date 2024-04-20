using System;

class CorpseList : Component
{
	[Property] public bool ImposeCorpseLimit { get; set; }
	[Property] public int CorpseLimit { get; set; }
	[Property] public bool ImposeCorpseTimeout { get; set; }
	[Property] public float CorpseTimeoutTime {  get; set; }

	private List<(ModelRenderer, float)> corpsePile = new List<(ModelRenderer, float)>();

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (ImposeCorpseTimeout)
		{
			float currentTime = Time.Now;
			var removeCorpses = new List<(ModelRenderer, float)>();
			foreach ( var corpse in corpsePile )
			{
				if(currentTime - corpse.Item2 > CorpseTimeoutTime )
				{
					removeCorpses.Add( corpse );
				}
			}
			foreach( var corpse in removeCorpses )
			{
				corpsePile.Remove( corpse );
				corpse.Item1.Enabled = false;
				corpse.Item1.Destroy();
			}
		}
	}

	public void addCorpse(ModelRenderer corpseModel, float timeOfDeath)
	{
		corpsePile.Add( (corpseModel, timeOfDeath) );
		if( ImposeCorpseLimit)
		{
			if ( corpsePile.Count > CorpseLimit )
			{
				corpsePile.Remove( corpsePile.First() );
			}
		}
	}
}
