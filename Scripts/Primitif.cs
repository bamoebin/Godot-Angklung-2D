namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Primitif : RefCounted
{
	// Menampilkan "hello world" ke output console.
	public void Helloworld()
	{
		GD.Print("hello world");
	}

	public List<Vector2> LineDDA(float xa, float ya, float xb, float yb, string style = "solid")
	{
		float dx = xb - xa;
		float dy = yb - ya;
		int steps;
		float xIncrement, yIncrement;
		float x = xa, y = ya;
		List<Vector2> res = new List<Vector2>();

		steps = Math.Abs(dx) > Math.Abs(dy) ? (int)Math.Abs(dx) : (int)Math.Abs(dy);
		xIncrement = dx / steps;
		yIncrement = dy / steps;

		int patternIndex = 0;

		int[] solidPattern = { 1 };
		int[] dashedPattern = { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
		int[] dottedPattern = { 1, 0, 0, 0, 0, 0 };
		int[] dashDotPattern = { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0 };

		int[] selectedPattern = solidPattern;
		switch (style)
		{
			case "dashed":
				selectedPattern = dashedPattern;
				break;
			case "dotted":
				selectedPattern = dottedPattern;
				break;
			case "dash-dot":
				selectedPattern = dashDotPattern;
				break;
		}

		for (int k = 0; k <= steps; k++)
		{
			if (selectedPattern[patternIndex % selectedPattern.Length] == 1)
			{
				res.Add(new Vector2(Mathf.Round(x), Mathf.Round(y)));
			}

			patternIndex++;
			x += xIncrement;
			y += yIncrement;
		}

		return res;
	}

	
	public List<Vector2> LineBresenham(float xa, float ya, float xb, float yb, string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();

		int ix0 = (int)Math.Round(xa);
		int iy0 = (int)Math.Round(ya);
		int ix1 = (int)Math.Round(xb);
		int iy1 = (int)Math.Round(yb);

		int dx = Math.Abs(ix1 - ix0), sx = ix0 < ix1 ? 1 : -1;
		int dy = -Math.Abs(iy1 - iy0), sy = iy0 < iy1 ? 1 : -1;
		int err = dx + dy, e2;
		int patternIndex = 0;

		int[] solidPattern = { 1 };      
		int[] dashedPattern = { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0};
		int[] dottedPattern = { 1, 0, 0, 0, 0, 0}; 
		int[] dashDotPattern = { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0};

		int[] selectedPattern = solidPattern;
		switch (style)
		{
			case "dashed":
				selectedPattern = dashedPattern;
				break;
			case "dotted":
				selectedPattern = dottedPattern;
				break;
			case "dash-dot":
				selectedPattern = dashDotPattern;
				break;
		}

		while (true)
		{
			if (selectedPattern[patternIndex % selectedPattern.Length] == 1)
			{
				points.Add(new Vector2(ix0, iy0));
			}
			
			patternIndex++;
			
			if (ix0 == ix1 && iy0 == iy1) break;
			e2 = 2 * err;
			if (e2 >= dy) { err += dy; ix0 += sx; }
			if (e2 <= dx) { err += dx; iy0 += sy; }
		}
		return points;
	}
	
	// Menghasilkan grafik eksponensial dengan fungsi y = e^(x/scale).
	// Param: startX - titik awal x; endX - titik akhir x; step - interval x; scale - faktor skala.
	// Return: List dari titik-titik (Vector2) yang membentuk grafik.
	public List<Vector2> ExponentialGraph(float startX, float endX, float step, float scale)
	{
		List<Vector2> points = new List<Vector2>();
	
		for (float x = startX; x <= endX; x += step)
		{
			float y = Mathf.Exp(x / scale);
			points.Add(new Vector2(x, y));
		}
	
		return points;
	}
}
