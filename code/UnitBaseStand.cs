using Sandbox;
using Sandbox.Citizen;
using System;

public class UnitBaseStand : Component
{
	[Property] public ModelRenderer baseStandModel { get; set; }

	[Property] public UnitModelUtils.OutlineState selectionOutlineState = UnitModelUtils.OutlineState.Mine;


	//public UnitBaseStand()
	//{
	//	Log.Info( "Create Base Stand!" );
	//	baseStandModel = new ModelRenderer();
	//	selectionOutlineState = UnitModel.OutlineState.Neutral;
	//}

	//public UnitBaseStand(UnitModel.OutlineState originalOState)
	//{
	//	Log.Info( "Create Specific Base Stand!" );
	//	baseStandModel = new ModelRenderer();
	//	selectionOutlineState = originalOState;
	//}

	//protected override void OnStart()
	//{
		//base.OnStart();
		//baseStandModel = new ModelRenderer();
		//baseStandModel.Model = Model.Load( "models/vidya/cylinder_white_64.vmdl_c" );
		//baseStandModel.Model = Model.Load( "models/cylinder_white_64.vmdl_c" );
		//baseStandModel.Transform.LocalScale = new Vector3( 0.5f, 0.5f, 0.01f );
		//baseStandModel.Tint = new Color( UnitModel.mineColor );


		//selectionOutlineState = UnitModel.OutlineState.Neutral;
	//}

	public void setOutlineState(UnitModelUtils.OutlineState newOState )
	{
		selectionOutlineState = newOState;

		if ( baseStandModel != null) {
			switch( selectionOutlineState )
			{
				case UnitModelUtils.OutlineState.Mine:
					baseStandModel.Tint = new Color(UnitModelUtils.mineColor);
					break;
				case UnitModelUtils.OutlineState.Ally:
					baseStandModel.Tint = new Color(UnitModelUtils.allyColor); 
					break;
				case UnitModelUtils.OutlineState.Neutral: 
					baseStandModel.Tint = new Color(UnitModelUtils.neutralColor);
					break;
				case UnitModelUtils.OutlineState.Hostile:
					baseStandModel.Tint = new Color( UnitModelUtils.hostileColor);
					break;
				case UnitModelUtils.OutlineState.Selected:
					baseStandModel.Tint = new Color( UnitModelUtils.selectedColor);
					break;
			}
		}
	}

	protected override void OnUpdate()
	{

	}

	protected override void OnDestroy()
	{
		baseStandModel.Enabled = false;
		baseStandModel.Destroy();
		base.OnDestroy();
	}
}
