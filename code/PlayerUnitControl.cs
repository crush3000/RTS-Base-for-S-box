using Sandbox.Citizen;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Utility.Svg;
using System;
using System.Reflection.Metadata;
using System.Threading;
public class PlayerUnitControl : Component
{
	const float CLICK_TIME = 0.1f;

	[Property]	RTSCamComponent RTSCam {  get; set; }
	[Property] SelectionPanel selectionPanel { get; set; }
	[Property] int team { get; set; }
	public List<SkinnedRTSObject> SelectedObjects { get; set; }

	private Rect selectionRect = new Rect();
	private Vector2 startSelectPos {  get; set; }
	private float startSelectTime { get; set; }
	private Vector2 endRectPos { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		SelectedObjects = new List<SkinnedRTSObject>();
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
			// TODO: I think I can fix the jank here if I make multiselect ALSO depend on how large of a rectangle you actually draw
			if ( Time.Now - startSelectTime > CLICK_TIME )
			{
				// TODO: Make the stance stuff less cheesy
				RTSGame.Instance.GameHUD.setSelectionVars( false, false );
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
								SelectedObjects.Add( unit );
								unit.select();
							}
							// Deselect and units that are not selected
							else
							{
								SelectedObjects.Remove( unit );
								unit.deSelect();
							}
						}
					}
				}
				stopDrawSelectionRect();
			}
			// This is for a single click
			else
			{
				//RTSGame.Instance.GameHUD.setSelectionVars( false, false );

				var mouseScreenPos = Mouse.Position;
				// Delesect all currently selected
				foreach ( SkinnedRTSObject obj in SelectedObjects )
				{
					obj.deSelect();
				}
				SelectedObjects.Clear();
				// Set up and run mouse ray to find what we're now selecting
				var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
				var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
				var tr = mouseRay.Run();

				// Get unit under ray
				var hitUnitComponents = tr.GameObject.Components.GetAll().OfType<Unit>();
				var hitBuildingComponents = tr.GameObject.Components.GetAll().OfType<Building>();

				// Select unit if one is hit
				if ( hitUnitComponents.Any() )
				{
					var selectedUnit = hitUnitComponents.First();
					// Make sure the unit is ours
					if ( selectedUnit.team == team)
					{
						//Log.Info( "Team " + team + " " + selectedUnit.GameObject.Name + " Selected from team " + selectedUnit.team );
						// Select Unit
						SelectedObjects.Add( selectedUnit );
						selectedUnit.select();
						RTSGame.Instance.GameHUD.setSelectionVars( true, selectedUnit.isInAttackMode );
					}
				}
				else
				{
					RTSGame.Instance.GameHUD.setSelectionVars( false, false );
				}

				// Select building if one is hit
				if ( hitBuildingComponents.Any() )
				{
					var selectedBuilding = hitBuildingComponents.First();
					// Make sure the unit is ours
					if ( selectedBuilding.team == team )
					{
						//Log.Info( "Team " + team + " " + selectedUnit.GameObject.Name + " Selected from team " + selectedUnit.team );
						// Select Building
						SelectedObjects.Add( selectedBuilding );
						selectedBuilding.select();
						RTSGame.Instance.GameHUD.setSelectionVars( false, false );
					}
				}
			}
		}

		// Command Controls
		// Command Button was just pressed
		if( Input.Pressed( "Command" ) && SelectedObjects.Count > 0)
		{
			// Init command vars
			UnitModelUtils.CommandType commandType = UnitModelUtils.CommandType.None;
			SkinnedRTSObject commandTarget = null;
			Vector3 moveTarget = new Vector3();
			// Draw mouse ray
			var mouseScreenPos = Mouse.Position;
			var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
			var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
			var tr = mouseRay.Run();
			// Get hit components
			var hitRtsObjects = tr.GameObject.Components.GetAll().OfType<SkinnedRTSObject>();
			Log.Info(tr.GameObject.Name);
			var hitWorldObjects = tr.GameObject.Components.GetAll().OfType<MapCollider>();
			
			// Set Up Attack Command if we hit an enemy unit
			// TODO Probably make sure that if we hit friendly units instead that it goes to a move, actually just test this code
			// TODO Cleanup generic command code with tags
			if ( hitRtsObjects.Any() && hitRtsObjects.First().team != team )
			{
				commandType = UnitModelUtils.CommandType.Attack;
				Log.Info( "Team " + team + " " + ((SkinnedRTSObject)(hitRtsObjects.First())).GameObject.Name + " Selected to be attacked!" );
				commandTarget = (SkinnedRTSObject)(hitRtsObjects.First());
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
			foreach ( var unit in SelectedObjects )
			{
				if ( unit != null && unit.Tags.Has(Unit.UNIT_TAG))
				{
					switch(commandType)
					{
						case UnitModelUtils.CommandType.Move:
							((Unit)unit).homeTargetLocation = moveTarget;
							RTSGame.Instance.GameCommandIndicator.PlayMoveIndicatorHere(moveTarget);
							break;
						case UnitModelUtils.CommandType.Attack:
							((Unit)unit).targetObject = commandTarget;
							RTSGame.Instance.GameCommandIndicator.PlayAttackIndicatorHere( commandTarget.GameObject );
							break;
					}
					((Unit)unit).commandGiven = commandType;
				}
			}
		}

		if(Input.Pressed("spawn_skeltal"))
		{
			// Set up and run mouse ray to find what we're now selecting
			var mouseScreenPos = Mouse.Position;
			var mouseDirection = RTSCam.CamView.ScreenPixelToRay( mouseScreenPos );
			var mouseRay = Scene.Trace.Ray( mouseDirection, 5000f );
			var tr = mouseRay.Run();

			//Call Unit Factory Here.
			//Log.Info( "Spawning Skeltal!" );
			//RTSGame.Instance.ThisPlayer.myUnitFactory.spawnUnit(RTSGame.Instance.ThisPlayer.skeltalPrefab, tr.EndPosition);
			Log.Info( "Spawning Skeltal House!" );
			RTSGame.Instance.ThisPlayer.myUnitFactory.spawnUnit( RTSGame.Instance.ThisPlayer.skeltalHousePrefab, tr.EndPosition );
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
