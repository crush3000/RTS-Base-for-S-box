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

	private bool isSelecting = false;
	private Rect selectionRect = new Rect();
	private Vector2 startSelectPos {  get; set; }
	private float startSelectTime { get; set; }
	private Vector2 endRectPos { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		SelectedUnits = new List<Unit>();
		//team = 99;
	}

	protected override void OnUpdate()
	{
		// Select Controls
		if ( Input.Pressed( "Select" ) )
		{
			startSelectTime = Time.Now;
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

		else if ( Input.Down( "Select" ) )
		{
			if ( !isSelecting ) 
			{
				isSelecting = true;
				startSelectPos = Mouse.Position;
			}
			//Log.Info( "Still Selecting!" );
			if(Time.Now - startSelectTime > CLICK_TIME)
			{
				endRectPos = Mouse.Position;
				drawSelectionRect();
			}
			//var groundRay = Scene.Trace.Ray( new Ray( CamPosition, ), 5000.0F );
		}

		else if(Input.Released("Select"))
		{
			if( Time.Now - startSelectTime > CLICK_TIME )
			{
				//Log.Info( "Release" );
				isSelecting = false;
				endRectPos = Mouse.Position;
				var units = Scene.GetAllComponents<Unit>();
				if ( units != null )
				{
					foreach ( var unit in units )
					{
						if ( unit != null && unit.team == team )
						{
							var unitPos = RTSCam.CamView.PointToScreenPixels( unit.Transform.Position );
							//Log.Info( "Unit Pos: " + unitRec );
							if ( selectionRect.IsInside( unitPos ) )
							{
								Log.Info( "Team " + team + " " + unit.GameObject.Name + " Selected from team " + unit.team );
								SelectedUnits.Add( unit );
								unit.SelectUnit();
							}
							else
							{
								SelectedUnits.Remove( unit );
								unit.DeSelectUnit();
							}
						}
					}
				}
				stopDrawSelectionRect();
			}
			else
			{
				SelectedUnits.Clear();
				var mouseScreenPos = Mouse.Position;
				var units = Scene.GetAllComponents<Unit>();
				foreach ( Unit unit in units )
				{
					if ( unit.team == team )
					{
						unit.DeSelectUnit();
					}
				}
				var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
				var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
				var tr = mouseRay.HitTriggers().Run();

				var hitUnitComponents = tr.GameObject.Components.GetAll().OfType<Unit>();

				if ( hitUnitComponents.Any() )
				{
					var selectedUnit = ((Unit)(hitUnitComponents.First()));
					if ( selectedUnit.team == team)
					{
						Log.Info( "Team " + team + " " + selectedUnit.GameObject.Name + " Selected from team " + selectedUnit.team );
						SelectedUnits.Add( selectedUnit );
						selectedUnit.SelectUnit();
					}
				}
			}

		}

		// Command Controls
		if( Input.Down( "Command" ) && SelectedUnits.Count > 0)
		{
			UnitModelUtils.CommandType commandType = UnitModelUtils.CommandType.None;
			Unit commandTarget = null;
			Vector3 moveTarget = new Vector3();
			var mouseScreenPos = Mouse.Position;
			var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
			var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
			var tr = mouseRay.HitTriggers().Run();

			var hitUnitComponents = tr.GameObject.Components.GetAll().OfType<Unit>();
			var hitWorldObjects = tr.GameObject.Components.GetAll().OfType<MapCollider>();

			if ( hitUnitComponents.Any() )
			{
				commandType = UnitModelUtils.CommandType.Attack;
				if (((Unit)(hitUnitComponents.First())).team != team)
				{
					Log.Info( "Team " + team + " " + ((Unit)(hitUnitComponents.First())).GameObject.Name + " Selected to be attacked!" );
					commandTarget = (Unit)(hitUnitComponents.First());
				}
			}
			else
			{
				//Log.Info( "Should be a move command" );
				//Move Command
				if ( hitWorldObjects.Any())
				{
					commandType = UnitModelUtils.CommandType.Move;
					tr = Scene.Trace.Ray( mouseDirection, 5000f ).Run();
					moveTarget = tr.EndPosition;
				}
			}
			/////COMMAND CODE
			foreach ( var unit in SelectedUnits )
			{
				if ( unit != null )
				{
					switch(commandType)
					{
						case UnitModelUtils.CommandType.Move:
							unit.TargetLocation = moveTarget;
							break;
						case UnitModelUtils.CommandType.Attack:
							unit.TargetUnit = commandTarget;
							break;
					}
					unit.CommandGiven = commandType;
				}
			}
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
