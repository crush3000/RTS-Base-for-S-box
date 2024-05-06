using Sandbox.Citizen;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Utility.Svg;
using System;
using System.Reflection.Metadata;
using System.Threading;
class PlayerUnitControl : Component
{
	const float CLICK_TIME = 0.1f;

	[Property]	RTSCamComponent RTSCam {  get; set; }
	[Property] SelectionPanel selectionPanel { get; set; }
	[Property] int team { get; set; }
	List<Unit> SelectedUnits { get; set; }

	private Rect selectionRect = new Rect();
	private Vector2 startSelectPos {  get; set; }
	private float startSelectTime { get; set; }
	private Vector2 endRectPos { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		SelectedUnits = new List<Unit>();
	}

	protected override void OnUpdate()
	{
		// Select Controls
		// Select Is now Pressed
		if ( Input.Pressed( "Select" ) )
		{
			startSelectTime = Time.Now;
			startSelectPos = Mouse.Position;
			//TODO Make some kind of debug interface for logging what's clicked on
			//var mouseDirection = RTSCam.CamView.ScreenPixelToRay( Mouse.Position );
			//var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
			//var tr = mouseRay.HitTriggers().Run();
			//Log.Info( "Hit " + tr.GameObject.Name + "!" );
			//var hitObjectComponents = tr.GameObject.Components.GetAll().OfType<Unit>();
			//var hitObjectComponents = tr.GameObject.Components.GetAll();
			//foreach ( var hitObjectComponent in hitObjectComponents )
			//{
			//	Log.Info( "Hit " + hitObjectComponent.GetType().ToString() + "!" );
			//}
			//if ( hitObjectComponents.Any() )
			//{
			//Log.Info( "Hit " + hitObjectComponents.First().GetType().ToString() + "!" );
			//}
		}

		// Select is held down
		else if ( Input.Down( "Select" ) )
		{
			if(Time.Now - startSelectTime > CLICK_TIME)
			{
				endRectPos = Mouse.Position;
				drawSelectionRect();
			}
		}

		// Select button has been released
		else if(Input.Released("Select"))
		{
			// For a drag just make sure we give them some time to actually click
			if ( Time.Now - startSelectTime > CLICK_TIME )
			{
				//Log.Info( "Release" );
				endRectPos = Mouse.Position;
				// Get ALL units. This is possibly a bad idea for speed
				var units = Scene.GetAllComponents<Unit>();
				// Select Units under rectangle
				if ( units != null )
				{
					foreach ( var unit in units )
					{
						// Ensure these are our units
						if ( unit != null && unit.team == team )
						{
							var unitPos = RTSCam.CamView.PointToScreenPixels( unit.Transform.Position );
							//Log.Info( "Unit Pos: " + unitRec );
							if ( selectionRect.IsInside( unitPos ) )
							{
								//Log.Info( "Team " + team + " " + unit.GameObject.Name + " Selected from team " + unit.team );
								//Select Unit
								SelectedUnits.Add( unit );
								unit.selectUnit();
							}
							// Deselect and units that are not selected
							else
							{
								SelectedUnits.Remove( unit );
								unit.deSelectUnit();
							}
						}
					}
				}
				stopDrawSelectionRect();
			}
			// This is for a single click
			else
			{
				var mouseScreenPos = Mouse.Position;
				// Delesect all currently selected
				foreach ( Unit unit in SelectedUnits )
				{
						unit.deSelectUnit();
				}
				SelectedUnits.Clear();
				// Set up and run mouse ray to find what we're now selecting
				var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
				var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
				var tr = mouseRay.Run();

				// Get unit under ray
				var hitUnitComponents = tr.GameObject.Components.GetAll().OfType<Unit>();

				// Select unit if one is hit
				if ( hitUnitComponents.Any() )
				{
					var selectedUnit = hitUnitComponents.First();
					// Make sure the unit is ours
					if ( selectedUnit.team == team)
					{
						//Log.Info( "Team " + team + " " + selectedUnit.GameObject.Name + " Selected from team " + selectedUnit.team );
						// Select Unit
						SelectedUnits.Add( selectedUnit );
						selectedUnit.selectUnit();
					}
				}
			}

		}

		// Command Controls
		// Command Button was just pressed
		if( Input.Pressed( "Command" ) && SelectedUnits.Count > 0)
		{
			// Init command vars
			UnitModelUtils.CommandType commandType = UnitModelUtils.CommandType.None;
			Unit commandTarget = null;
			Vector3 moveTarget = new Vector3();
			// Draw mouse ray
			var mouseScreenPos = Mouse.Position;
			var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
			var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
			var tr = mouseRay.Run();
			// Get hit components
			var hitUnits = tr.GameObject.Components.GetAll().OfType<Unit>();
			Log.Info(tr.GameObject.Name);
			var hitWorldObjects = tr.GameObject.Components.GetAll().OfType<MapCollider>();
			
			// Set Up Attack Command if we hit an enemy unit
			// TODO Probably make sure that if we hit friendly units instead that it goes to a move, actually just test this code
			if ( hitUnits.Any() && hitUnits.First().team != team )
			{
				commandType = UnitModelUtils.CommandType.Attack;
				Log.Info( "Team " + team + " " + ((Unit)(hitUnits.First())).GameObject.Name + " Selected to be attacked!" );
				commandTarget = (Unit)(hitUnits.First());
			}
			// Otherwise Set Up Move Command
			else
			{
				if ( hitWorldObjects.Any())
				{
					commandType = UnitModelUtils.CommandType.Move;
					moveTarget = tr.EndPosition;
				}
			}
			// Issue Command
			foreach ( var unit in SelectedUnits )
			{
				if ( unit != null )
				{
					switch(commandType)
					{
						case UnitModelUtils.CommandType.Move:
							unit.homeTargetLocation = moveTarget;
							break;
						case UnitModelUtils.CommandType.Attack:
							unit.targetUnit = commandTarget;
							break;
					}
					unit.commandGiven = commandType;
				}
			}
		}
	}

	private void drawSelectionRect()
	{
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
