
using Godot;
using System;
using System.Collections.Generic;

public partial class BentukDasar : Node2D
{
	private Primitif primitif = new Primitif();

	public List<Vector2> DrawSquare(int x, int y, int size, string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();
		points.AddRange(primitif.LineBresenham(x, y, x + size, y, style));
		points.AddRange(primitif.LineBresenham(x, y, x, y + size, style));
		points.AddRange(primitif.LineBresenham(x + size, y, x + size, y + size, style));
		points.AddRange(primitif.LineBresenham(x, y + size, x + size, y + size, style));
		return points;
	}

	public List<Vector2> DrawRectangle(int x, int y, int width, int height, string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();
		points.AddRange(primitif.LineBresenham(x, y, x + width, y, style));
		points.AddRange(primitif.LineBresenham(x, y, x, y + height, style));
		points.AddRange(primitif.LineBresenham(x + width, y, x + width, y + height, style));
		points.AddRange(primitif.LineBresenham(x, y + height, x + width, y + height, style));
		return points;
	}

	public List<Vector2> DrawRightTriangle(int x, int y, int baseLength, int height, string arah = "right", string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();
		if (arah.ToLower() == "right")
		{
			// Siku di kiri bawah
			points.AddRange(primitif.LineBresenham(x, y, x + baseLength, y, style));           // Alas (kanan)
			points.AddRange(primitif.LineBresenham(x, y, x, y - height, style));               // Tinggi (atas)
			points.AddRange(primitif.LineBresenham(x, y - height, x + baseLength, y, style));  // Sisi miring
		}
		else if (arah.ToLower() == "left")
		{
			// Siku di kanan bawah
			points.AddRange(primitif.LineBresenham(x, y, x - baseLength, y, style));           // Alas (kiri)
			points.AddRange(primitif.LineBresenham(x, y, x, y - height, style));               // Tinggi (atas)
			points.AddRange(primitif.LineBresenham(x, y - height, x - baseLength, y, style));  // Sisi miring
		}
		return points;
	}


	public List<Vector2> DrawIsoscelesTriangle(int x, int y, int baseWidth, int height, string orientation = "up", string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();
		Vector2 p1, p2, p3;

		switch (orientation.ToLower())
		{
			case "up":
				p1 = new Vector2(x, y); // Bawah kiri
				p2 = new Vector2(x + baseWidth, y); // Bawah kanan
				p3 = new Vector2(x + baseWidth / 2, y - height); // Puncak atas
				break;

			case "down":
				p1 = new Vector2(x, y); // Atas kiri
				p2 = new Vector2(x + baseWidth, y); // Atas kanan
				p3 = new Vector2(x + baseWidth / 2, y + height); // Puncak bawah
				break;

			case "left":
				p1 = new Vector2(x, y); // Kanan atas
				p2 = new Vector2(x, y + baseWidth); // Kanan bawah
				p3 = new Vector2(x - height, y + baseWidth / 2); // Puncak kiri
				break;

			case "right":
				p1 = new Vector2(x, y); // Kiri atas
				p2 = new Vector2(x, y + baseWidth); // Kiri bawah
				p3 = new Vector2(x + height, y + baseWidth / 2); // Puncak kanan
				break;

			default:
				// Default ke atas
				p1 = new Vector2(x, y);
				p2 = new Vector2(x + baseWidth, y);
				p3 = new Vector2(x + baseWidth / 2, y - height);
				break;
		}

		points.AddRange(primitif.LineBresenham(p1.X, p1.Y, p2.X, p2.Y, style));
		points.AddRange(primitif.LineBresenham(p2.X, p2.Y, p3.X, p3.Y, style));
		points.AddRange(primitif.LineBresenham(p3.X, p3.Y, p1.X, p1.Y, style));

		return points;
	}


	public List<Vector2> DrawTrapezoid(int x, int y, int width, int height, int topOffset, string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();
		// Titik-titik sudut trapesium
		Vector2 p1 = new Vector2(x, y); // kiri bawah
		Vector2 p2 = new Vector2(x, y - height); // kiri atas (sebelum offset)
		Vector2 p3 = new Vector2(x + width, y - height); // kanan atas
		Vector2 p4 = new Vector2(x + width, y); // kanan bawah
												// Ubah bagian di bawah ini untuk mementukan titik yang membeuat kemiringan
		p2.Y += topOffset;

		// Gambar sisi-sisi trapesium
		points.AddRange(primitif.LineBresenham(p1.X, p1.Y, p2.X, p2.Y, style)); // sisi kiri miring
		points.AddRange(primitif.LineBresenham(p2.X, p2.Y, p3.X, p3.Y, style)); // sisi atas
		points.AddRange(primitif.LineBresenham(p3.X, p3.Y, p4.X, p4.Y, style)); // sisi kanan
		points.AddRange(primitif.LineBresenham(p4.X, p4.Y, p1.X, p1.Y, style)); // sisi bawah

		return points;
	}

	public List<Vector2> DrawIsoscelesTrapezoid(int x, int y, int baseWidth, int topWidth, int height, string style = "solid")
	{
		List<Vector2> points = new List<Vector2>();

		int halfBase = baseWidth / 2;
		int halfTop = topWidth / 2;

		// Titik sudut:
		Vector2 p1 = new Vector2(x - halfBase, y);             // kiri bawah
		Vector2 p2 = new Vector2(x - halfTop, y - height);     // kiri atas
		Vector2 p3 = new Vector2(x + halfTop, y - height);     // kanan atas
		Vector2 p4 = new Vector2(x + halfBase, y);             // kanan bawah

		points.AddRange(primitif.LineBresenham(p1.X, p1.Y, p2.X, p2.Y, style)); // kiri miring
		points.AddRange(primitif.LineBresenham(p2.X, p2.Y, p3.X, p3.Y, style)); // atas
		points.AddRange(primitif.LineBresenham(p3.X, p3.Y, p4.X, p4.Y, style)); // kanan miring
		points.AddRange(primitif.LineBresenham(p4.X, p4.Y, p1.X, p1.Y, style)); // bawah

		return points;
	}

	//Modul 3 Midpoint Ellips
	public List<Vector2> DrawMidEllipse(int centerX, int centerY, int Rx, int Ry)
	{
		List<Vector2> points = new List<Vector2>();
		int Rx2 = Rx * Rx;
		int Ry2 = Ry * Ry;
		int twoRx2 = 2 * Rx2;
		int twoRy2 = 2 * Ry2;
		int x = 0;
		int y = Ry;
		int px = 0;
		int py = twoRx2 * y;
		float p;

		p = Ry2 - (Rx2 * Ry) + (0.25f * Rx2);
		while (px < py)
		{
			x++;
			px += twoRy2;
			if (p < 0)
			{
				p += Ry2 + px;
			}
			else
			{
				y--;
				py -= twoRx2;
				p += Ry2 + px - py;
			}
			points.AddRange(PlotEllipsePoints(centerX, centerY, x, y));
		}

		p = (int)(Ry2 * (x + 0.5) * (x + 0.5) + Rx2 * (y - 1) * (y - 1) - Rx2 * Ry2);
		while (y > 0)
		{
			y--;
			py -= twoRx2;
			if (p > 0)
			{
				p += Rx2 - py;
			}
			else
			{
				x++;
				px += twoRy2;
				p += Rx2 - py + px;
			}
			points.AddRange(PlotEllipsePoints(centerX, centerY, x, y));
		}
		return points;
	}

	private List<Vector2> PlotEllipsePoints(int centerX, int centerY, int x, int y)
	{
		return new List<Vector2>
		{
			new Vector2(centerX + x, centerY + y),
			new Vector2(centerX - x, centerY + y),
			new Vector2(centerX + x, centerY - y),
			new Vector2(centerX - x, centerY - y)
		};
	}

	// Lingkaran
	public List<Vector2> DrawMidCircle(int centerX, int centerY, int radius)
	{
		List<Vector2> circlePoints = new List<Vector2>();

		int x = 0;
		int y = radius;
		int d = 1 - radius; // Decision parameter

		while (x <= y)
		{
			circlePoints.AddRange(PlotPoints(centerX, centerY, x, y));
			x++;

			if (d < 0)
			{
				d += 2 * x + 1;
			}
			else
			{
				y--;
				d += 2 * (x - y) + 1;
			}
		}

		return circlePoints;
	}

	private List<Vector2> PlotPoints(int centerX, int centerY, int x, int y)
	{
		return new List<Vector2>
	{
		new Vector2(centerX + x, centerY + y),
		new Vector2(centerX - x, centerY + y),
		new Vector2(centerX + x, centerY - y),
		new Vector2(centerX - x, centerY - y),
		new Vector2(centerX + y, centerY + x),
		new Vector2(centerX - y, centerY + x),
		new Vector2(centerX + y, centerY - x),
		new Vector2(centerX - y, centerY - x)
	};
	}
}
