using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;
using System.Numerics;
namespace Godot;

public partial class FilledBentukDasar : BaseKarya
{
	public List<(int offsetX, int batangTinggi, List<int> simpulY)> bambuData = null;
	
	// Helper method untuk transformasi titik tunggal
	public Vector2 TransformPoint(Matrix4x4 matrix, Vector2 point)
	{
		// Convert Vector2 to Vector3 with Z=1
		Vector3 tempPoint = new Vector3(point.X, point.Y, 1);

		// Manually perform matrix-vector multiplication
		Vector3 transformedPoint3D = new Vector3(
			matrix.M11 * tempPoint.X + matrix.M12 * tempPoint.Y + matrix.M13 * tempPoint.Z + matrix.M14,
			matrix.M21 * tempPoint.X + matrix.M22 * tempPoint.Y + matrix.M23 * tempPoint.Z + matrix.M24,
			matrix.M31 * tempPoint.X + matrix.M32 * tempPoint.Y + matrix.M33 * tempPoint.Z + matrix.M34
		);

		// Convert back to Vector2
		return new Vector2(transformedPoint3D.X, transformedPoint3D.Y);
	}
	
	// Helper methods untuk drawing yang optimal
	public void DrawRectOptimized(int x, int y, int width, int height, Color color, Matrix4x4? transform = null)
	{
		if (transform.HasValue && transform.Value != Matrix4x4.Identity)
		{
			Vector2[] rectPoints = new Vector2[] {
				new Vector2(x, y),
				new Vector2(x + width, y),
				new Vector2(x + width, y + height),
				new Vector2(x, y + height)
			};
			
			List<Vector2> transformedPoints = new List<Vector2>();
			foreach (var point in rectPoints)
			{
				transformedPoints.Add(TransformPoint(transform.Value, point));
			}
			DrawPolygon(transformedPoints.ToArray(), new Color[] { color });
		}
		else
		{
			DrawRect(new Rect2(x, y, width, height), color, true);
		}
	}
	
	public void DrawCircleOptimized(int x, int y, int radius, Color color, Matrix4x4? transform = null)
	{
		if (!transform.HasValue || transform.Value == Matrix4x4.Identity)
		{
			DrawCircle(new Vector2(x, y), radius, color);
		}
		else
		{
			int segments = 24; // Jumlah segmen untuk lingkaran
			Vector2[] circlePoints = new Vector2[segments];
			
			for (int i = 0; i < segments; i++)
			{
				float angle = i * 2 * MathF.PI / segments;
				circlePoints[i] = new Vector2(
					x + radius * MathF.Cos(angle),
					y + radius * MathF.Sin(angle)
				);
			}
			
			List<Vector2> transformedPoints = new List<Vector2>();
			foreach (var point in circlePoints)
			{
				transformedPoints.Add(TransformPoint(transform.Value, point));
			}
			
			DrawPolygon(transformedPoints.ToArray(), new Color[] { color });
		}
	}
	
	public void DrawIsoscelesTrapezoidOptimized(int x, int y, int halfWidthBottom, int halfWidthTop, int height, Color color, Matrix4x4? transform = null)
	{
		Vector2[] points = new Vector2[] {
			new Vector2(x - halfWidthBottom, y),
			new Vector2(x - halfWidthTop, y - height),
			new Vector2(x + halfWidthTop, y - height),
			new Vector2(x + halfWidthBottom, y)
		};
		
		if (transform.HasValue && transform.Value != Matrix4x4.Identity)
		{
			List<Vector2> transformedPoints = new List<Vector2>();
			foreach (var point in points)
			{
				transformedPoints.Add(TransformPoint(transform.Value, point));
			}
			DrawPolygon(transformedPoints.ToArray(), new Color[] { color });
		}
		else
		{
			DrawPolygon(points, new Color[] { color });
		}
	}
	
	public void DrawIsoscelesTriangleOptimized(int x, int y, int baseWidth, int height, string orientation, Color color, Matrix4x4? transform = null)
	{
		Vector2[] points;
		
		if (orientation == "up")
		{
			points = new Vector2[] {
				new Vector2(x - baseWidth/2, y),
				new Vector2(x, y - height),
				new Vector2(x + baseWidth/2, y)
			};
		}
		else if (orientation == "down")
		{
			points = new Vector2[] {
				new Vector2(x - baseWidth/2, y - height),
				new Vector2(x + baseWidth/2, y - height),
				new Vector2(x, y)
			};
		}
		else
		{
			// Orientasi default
			points = new Vector2[] {
				new Vector2(x - baseWidth/2, y),
				new Vector2(x, y - height),
				new Vector2(x + baseWidth/2, y)
			};
		}
		
		if (transform.HasValue && transform.Value != Matrix4x4.Identity)
		{
			List<Vector2> transformedPoints = new List<Vector2>();
			foreach (var point in points)
			{
				transformedPoints.Add(TransformPoint(transform.Value, point));
			}
			DrawPolygon(transformedPoints.ToArray(), new Color[] { color });
		}
		else
		{
			DrawPolygon(points, new Color[] { color });
		}
	}
	
	public void DrawRightTriangleOptimized(int x, int y, int baseWidth, int height, string orientation, Color color, Matrix4x4? transform = null)
	{
		Vector2[] points;
		
		if (orientation.ToLower() == "left")
		{
			points = new Vector2[] {
				new Vector2(x, y),
				new Vector2(x, y - height),
				new Vector2(x - baseWidth, y)
			};
		}
		else if (orientation.ToLower() == "right")
		{
			points = new Vector2[] {
				new Vector2(x, y),
				new Vector2(x, y - height),
				new Vector2(x + baseWidth, y)
			};
		}
		else
		{
			// Orientasi default
			points = new Vector2[] {
				new Vector2(x, y),
				new Vector2(x + baseWidth, y),
				new Vector2(x, y - height)
			};
		}
		
		if (transform.HasValue && transform.Value != Matrix4x4.Identity)
		{
			List<Vector2> transformedPoints = new List<Vector2>();
			foreach (var point in points)
			{
				transformedPoints.Add(TransformPoint(transform.Value, point));
			}
			DrawPolygon(transformedPoints.ToArray(), new Color[] { color });
		}
		else
		{
			DrawPolygon(points, new Color[] { color });
		}
	}
}
