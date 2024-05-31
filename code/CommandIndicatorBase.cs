public abstract class CommandIndicatorBase : Component
{
	abstract public void PlayMoveIndicatorHere( Vector3 location );
	abstract public void PlayAttackIndicatorHere( Vector3 location );
	abstract public void PlayAttackIndicatorHere( GameObject newAttackedObject );
	abstract public void PlayPingIndicatorHere( Vector3 location );
}
