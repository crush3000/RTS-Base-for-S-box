using Sandbox.Citizen;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Utility.Svg;
using System;
using System.Reflection.Metadata;
using System.Threading;
class PlayerUnitControl : Component
{

	[Property]	RTSCamComponent RTSCam {  get; set; }
	[Property] SelectionPanel selectionPanel { get; set; }
	List<Unit> SelectedUnits { get; set; }

	private bool isSelecting = false;
	private Rect selectionRect = new Rect();
	private Vector2 startSelectPos {  get; set; }
	private Vector2 endSelectPos { get; set; }
	private Vector2 endRectPos { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		SelectedUnits = new List<Unit>();
	}

	protected override void OnUpdate()
	{
		if ( Input.Down( "Select" ) )
		{
			if ( !isSelecting ) 
			{
				isSelecting = true;
				startSelectPos = Mouse.Position;
			}
			Log.Info( "Still Selecting!" );
			endRectPos = Mouse.Position;
			drawSelectionRect();
			//var groundRay = Scene.Trace.Ray( new Ray( CamPosition, ), 5000.0F );
		}
		else if(Input.Released("Select"))
		{
			isSelecting= false;
			endRectPos= Mouse.Position;
			endSelectPos = Mouse.Position;
			stopDrawSelectionRect();
		}
		if (Input.Pressed("Select"))
		{
			Log.Info( "Click" );
			////SELECTION CODE
			//TODO Deselection
			var mouseScreenPos = Mouse.Position;
			var units = Scene.GetAllComponents<Unit>();
			if ( units != null )
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
						//Log.Info( "Unit Pos: " + unitRec );
						if ( unitRec.IsInside( mouseScreenPos ) )
						{
							Log.Info( "Unit Selected!" );
							SelectedUnits.Add( unit );
							unit.SelectUnit();
						}
					}
				}
			}
		}
		if( Input.Down( "Command" ) && SelectedUnits.Count > 0)
		{
			foreach( var unit in SelectedUnits )
			{
				if( unit != null )
				{
					Log.Info( "Unit Moving!" );
					var mouseDirection = RTSCam.CamView.ScreenPixelToRay( Mouse.Position );
					var tr = Scene.Trace.Ray( mouseDirection, 5000f ).Run();
					unit.CommandGiven = true;
					unit.TargetLocation = tr.EndPosition;
				}
			}
			/////COMMAND CODE
		}
	}

	private void drawSelectionRect()
	{
		//selectionPanel.Enabled = true;
		//selectionPanel
		selectionRect = new Rect(
			Math.Min(startSelectPos.x, endRectPos.x),
			Math.Min( startSelectPos.y, endRectPos.y),
			Math.Abs( startSelectPos.x - endRectPos.x),
			Math.Abs( startSelectPos.y - endRectPos.y)
			);

		selectionPanel.panelArea = selectionRect;
		selectionPanel.Enabled = true;
		selectionPanel.StateHasChanged();
	}

	private void stopDrawSelectionRect()
	{
		selectionPanel.Enabled = false;
	}
}
