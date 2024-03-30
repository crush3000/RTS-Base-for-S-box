using Sandbox.Citizen;
using System;

class UnitBase : Component
{

	[Property] public SpriteRenderer sprite { get; set; }

	[Property] public ModelRenderer model { get; set; }

	public UnitModel.OutlineState selectionOutlineState = UnitModel.OutlineState.Mine;

protected override void OnStart()
{
}

protected override void OnUpdate()
{
}
}
