namespace Godot;
using Godot;
using System.Collections.Generic;
using Num = System.Numerics; 


// Kelas dasar untuk Karya1 dan Karya2
public partial class BaseKarya : Node2D
{
	protected BentukDasar bentuk = new BentukDasar();
	protected TransformasiFast transformasi = new TransformasiFast();
	protected Primitif primitif = new Primitif();
	protected int screenWidth = ScreenHelper.ScreenWidth;
	protected int screenHeight = ScreenHelper.ScreenHeight;
	protected int MarginRight = ScreenHelper.MarginRight;
	protected int MarginBottom = ScreenHelper.MarginBottom;
	protected int MarginLeft = ScreenHelper.MarginLeft;
	protected int MarginTop = ScreenHelper.MarginTop;
	protected int centerX = ScreenHelper.ScreenWidth / 2;
	protected int centerY = ScreenHelper.ScreenHeight / 2;
	
	// Variabel untuk Slider
	public float rotationSpeed = 4.0f; // Kecepatan rotasi bunga (radian per detik)
	public float floatingSpeed = 1.5f; // Kecepatan perpindahan bunga

	// Variabel untuk CheckButton
	public bool isDrawAxis;
	public bool isDrawMarginBox;
	public bool isSimpleAnimation = false;//Karya3
	public bool isFlowerAnimationActive = true; //Karya4

	// Menggambar sumbu koordinat

	protected void DrawAxis(Color axisColor)
	{

		List<Vector2> axisLines = new List<Vector2>();
		axisLines.AddRange(primitif.LineDDA(50, centerY, screenWidth - 50, centerY)); // Sumbu X
		axisLines.AddRange(primitif.LineDDA(centerX, 50, centerX, screenHeight - 50)); // Sumbu Y
		PutPixelAll(axisLines, axisColor);
	}

	// Konversi koordinat kartesian ke pixel layar
	protected Vector2 ConvertToPixel(Vector2 point, float centerX, float centerY)
	{
		return new Vector2(centerX + point.X, centerY - point.Y);
	}

	protected List<Vector2> ConvertToPixelList(List<Vector2> points, float centerX, float centerY)
	{
		List<Vector2> pixelPoints = new List<Vector2>();
		foreach (var point in points)
		{
			pixelPoints.Add(ConvertToPixel(point, centerX, centerY));
		}
		return pixelPoints;
	}

	// Konversi dari pixel ke koordinat kartesian
	protected Vector2 ConvertToCartesian(Vector2 pixel, float centerX, float centerY)
	{
		return new Vector2(pixel.X - centerX, centerY - pixel.Y);
	}

	//Menggambar kotak dalam margin
	protected void DrawMarginBox(Color color)
	{
		List<Vector2> box = new List<Vector2>();

		box.AddRange(primitif.LineDDA(MarginLeft, MarginTop, screenWidth - MarginLeft, MarginTop));
		box.AddRange(primitif.LineDDA(MarginLeft, screenHeight - MarginTop, screenWidth - MarginLeft, screenHeight - MarginTop));
		box.AddRange(primitif.LineDDA(MarginLeft, MarginTop, MarginLeft, screenHeight - MarginTop));
		box.AddRange(primitif.LineDDA(screenWidth - MarginLeft, MarginTop, screenWidth - MarginLeft, screenHeight - MarginTop));
		PutPixelAll(box, color);
	}


	public void PutPixel(float x, float y, Color color)
	{
		DrawRect(new Rect2(new Vector2(x, y), new Vector2(1, 1)), color);
	}

	public void PutPixelAll(List<Vector2> dots, Color color)
	{
		foreach (var dot in dots)
		{
			PutPixel(dot.X, dot.Y, color);
		}
	}
}
