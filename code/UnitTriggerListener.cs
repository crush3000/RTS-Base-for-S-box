using Sandbox;

public sealed class UnitTriggerListener : Component, Component.ITriggerListener
{
	public void OnTriggerEnter(Collider other)
	{
		//if ( collisionInfo.GetType() == )
		//Log.Info( "Trigger code is being called: " + other );
	}

	public void OnTiggerExit( Collider other )
	{

	}
}
