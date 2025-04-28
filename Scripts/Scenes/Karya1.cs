using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using System.Numerics;
public partial class Karya1 : BaseKarya1n2
{
	

	public override void _Process(double delta)
	{
		
	}
	public override void _Ready()
	{
	
	}

	public override void _Draw()
	{
		if (isDrawAxis)
			DrawAxis(Colors.Green);
		if (isDrawMarginBox)
			DrawMarginBox(Colors.Yellow);


		GambarAngklung(new Vector2(120, 450), 0.7f);  
		GambarAngklung(new Vector2(240, 450), 0.7f);
		GambarAngklung(new Vector2(360, 450), 0.7f);
		GambarAngklung(new Vector2(480, 450), 0.7f);
		GambarAngklung(new Vector2(606, 450), 0.7f);
		GambarAngklung(new Vector2(726, 450), 0.7f);
		GambarAngklung(new Vector2(846, 450), 0.7f);
		GambarAngklung(new Vector2(966, 450), 0.7f);
		GambarPendopo(new Vector2(576, 420), 0.9f);
		MotifAngklung(new Vector2(576, 380), 0.7f);
		GambarBunga(new Vector2(164, 125), 1.0f);
		GambarBunga(new Vector2(988, 125), 1.0f);
		GambarPohonBambu(new Vector2(100, 410), 1.0f);
		GambarPohonBambu(new Vector2(1052, 410), 1.0f);
	}

}
