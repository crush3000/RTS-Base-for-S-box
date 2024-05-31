using Sandbox;
[Title( "Particle Command Indicator" ), Category( "Particles" )]
class ParticleCommandIndicator : CommandIndicatorBase
{
	[Property] LegacyParticleSystem moveCommandEmitter { set; get; }
	[Property] LegacyParticleSystem attackCommandEmitter { set; get; }

	private SceneParticles currentParticles;
	private GameObject attackedObject;
	private bool isFollowingObject = false;

	override public void PlayMoveIndicatorHere( Vector3 location )
	{
		if ( currentParticles != null && !currentParticles.Finished )
		{
			currentParticles.Delete();
		}
		isFollowingObject = false;
		attackedObject = null;
		moveCommandEmitter.Transform.Position = location;
		currentParticles = new SceneParticles( Scene.SceneWorld, moveCommandEmitter.Particles );
		currentParticles.SetControlPoint( 0, location);
	}
	override public void PlayAttackIndicatorHere( Vector3 location )
	{
		if ( currentParticles != null && !currentParticles.Finished )
		{
			currentParticles.Delete();
		}
		isFollowingObject = false;
		attackedObject = null;
		attackCommandEmitter.Transform.Position = location;
		currentParticles = new SceneParticles( Scene.SceneWorld, attackCommandEmitter.Particles );
		currentParticles.SetControlPoint( 0, location );

	}
	override public void PlayAttackIndicatorHere( GameObject newAttackedObject )
	{
		if ( currentParticles != null && !currentParticles.Finished )
		{
			currentParticles.Delete();
		}
		isFollowingObject = true;
		attackedObject = newAttackedObject;
		currentParticles = new SceneParticles( Scene.SceneWorld, attackCommandEmitter.Particles );
		currentParticles.SetControlPoint( 0, attackedObject.Transform.Position );
	}
	override public void PlayPingIndicatorHere( Vector3 location )
	{
		Log.Info( "TODO: Ping not implemented" );
	}

	protected override void OnUpdate()
	{
		if(currentParticles != null)
		{
			if ( isFollowingObject == true )
			{
				attackCommandEmitter.Transform.Position = attackedObject.Transform.Position;
				currentParticles.SetControlPoint( 0, attackedObject.Transform.Position );
			}
			currentParticles.Simulate( Time.Delta );
		}
	}
}
