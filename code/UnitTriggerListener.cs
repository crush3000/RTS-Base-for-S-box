using Sandbox;

public sealed class UnitTriggerListener : Component, Component.ITriggerListener
{
	public void OnTriggerEnter(Collider other)
	{
		//if ( collisionInfo.GetType() == )
		//Log.Info( "OnTriggerEnter(" + other + ")");
	}

	public void OnTiggerExit( Collider other )
	{
		//Log.Info( "OnTriggerExit(" + other + ")" );
	}
}
