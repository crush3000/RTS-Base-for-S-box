using System;
using System.Reflection.Metadata;
class PlayerUnitControl : Component
{

	[Property]	RTSCamComponent RTSCam {  get; set; }
	List<Unit> SelectedUnits { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		SelectedUnits = new List<Unit>();
	}

	protected override void OnUpdate()
	{
		if ( Input.Down( "Select" ) )
		{
			Log.Info( "Click");
			////SELECTION CODE
			var mouseScreenPos = Mouse.Position;
			var units = Scene.GetAllComponents<Unit>();
			if( units != null )
			{
				foreach ( var unit in units )
				{
					if ( unit != null )
					{
						var screenScale = RTSCam.CamView.PointToScreenPixels( unit.Transform.Position );
						var screenX = screenScale.x;
						var screenY = screenScale.y;
						var unitRec = new Rect( screenX, screenY, 100f, 100f );
						Log.Info( "Mouse Pos: " + mouseScreenPos );
						Log.Info( "Unit Pos: " + unitRec );
						if ( unitRec.IsInside( mouseScreenPos ) )
						{
							Log.Info( "Unit Selected!" );
							SelectedUnits.Add( unit );
						}
					}
				}
			}
			//var groundRay = Scene.Trace.Ray( new Ray( CamPosition, ), 5000.0F );
		}
		if( Input.Down( "Command" ) && SelectedUnits.Count > 0)
		{
			foreach( var unit in SelectedUnits )
			{
				if( unit != null )
				{
					Log.Info( "Unit Moving!" );
					//var yee = new ScreenPanel();
					var mouseDirection = RTSCam.CamView.ScreenPixelToRay( Mouse.Position );
					var tr = Scene.Trace.Ray( mouseDirection, 5000f ).Run();
					unit.CommandGiven = true;
					unit.TargetLocation = tr.EndPosition;
					//unit.UnitNavAgent.MoveTo( tr.EndPosition );
					//unit.
				}
			}
			/////COMMAND CODE
		}
	}
}
