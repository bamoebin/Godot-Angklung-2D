using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using System.Numerics;

public partial class Karya2 : BaseKarya1n2
{
	private float time = 0; // Waktu untuk animasi
	private float amplitude = 1; // Amplitudo getaran (dalam piksel)
	private float frequency = 43; // Frekuensi getaran
	private float rotationSpeed = 4.0f; // Kecepatan rotasi bunga (radian per detik)
	private float floatingSpeed = 1.5f; // Kecepatan perpindahan bunga
	private float orbitRadius = 20; // Radius rotasi bunga di tempat
	protected bool isActive; // Status animasi

	// Variabel untuk menyimpan hasil perhitungan
	private List<Vector2> angklungPositions = new List<Vector2>();
	private List<Matrix4x4> angklungTransforms = new List<Matrix4x4>();
	private Matrix4x4 motifMatrix;
	private Vector2 bunga1Position;
	private Vector2 bunga2Position;
	private Matrix4x4 bunga1Transform;
	private Matrix4x4 bunga2Transform;

	// Variabel untuk membatasi pembaruan frame
	private float redrawAccumulator = 0;
	private const float redrawInterval = 1.0f / 60.0f; // 60 FPS

	public override void _EnterTree()
	{
		isActive = true; // Aktifkan animasi ketika scene dimasukkan ke tree
		GD.Print("Karya2: Animasi diaktifkan");
	}

	public override void _ExitTree()
	{
		isActive = false; // Nonaktifkan animasi ketika scene dihapus dari tree
		GD.Print("Karya2: Animasi dinon-aktifkan");
	}

	public override void _Process(double delta)
	{
		if (!isActive) return;

		time += (float)delta;
		redrawAccumulator += (float)delta;

		// Hitung transformasi angklung
		angklungPositions.Clear();
		angklungTransforms.Clear();
		for (int i = 0; i < 8; i++)
		{
			Vector2 posisi = new Vector2(120 + i * 120, 450);
			float offsetX = amplitude * MathF.Sin(time * frequency + i);
			Matrix4x4 matrix = TransformasiFast.Identity();
			transformasi.Translation(ref matrix, offsetX, 0);
			angklungPositions.Add(posisi);
			angklungTransforms.Add(matrix);
		}

		// Hitung transformasi motif angklung
		motifMatrix = TransformasiFast.Identity();
		float offsetY = amplitude * MathF.Sin(time * frequency);
		transformasi.Translation(ref motifMatrix, 0, offsetY);

		// Hitung transformasi bunga
		float t1 = (MathF.Sin(time * floatingSpeed) + 1) / 2;
		bunga1Position = new Vector2(164, 125).Lerp(new Vector2(988, 125), t1);
		float t2 = (MathF.Sin(time * floatingSpeed) + 1) / 2;
		bunga2Position = new Vector2(988, 125).Lerp(new Vector2(164, 125), t2);

		float angle1 = time * rotationSpeed;
		bunga1Transform = TransformasiFast.Identity();
		transformasi.RotationClockwise(ref bunga1Transform, angle1, bunga1Position);

		float angle2 = time * rotationSpeed;
		bunga2Transform = TransformasiFast.Identity();
		transformasi.RotationClockwise(ref bunga2Transform, angle2, bunga2Position);

		// Batasi pembaruan frame
		if (redrawAccumulator >= redrawInterval)
		{
			redrawAccumulator -= redrawInterval;
			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		if (!isActive) return;

		if (isDrawAxis)
			DrawAxis(Colors.Green);

		if (isDrawMarginBox)
			DrawMarginBox(Colors.Yellow);

		// Gambar angklung
		for (int i = 0; i < angklungPositions.Count; i++)
		{
			GambarAngklung(angklungPositions[i], 0.7f, angklungTransforms[i]);
		}

		// Gambar motif angklung
		MotifAngklung(new Vector2(576, 380), 0.7f, motifMatrix);

		// Gambar bunga
		GambarBunga(bunga1Position, 1.0f, bunga1Transform);
		GambarBunga(bunga2Position, 1.0f, bunga2Transform);

		// Gambar elemen statis
		GambarPendopo(new Vector2(576, 420), 0.9f);
		GambarPohonBambu(new Vector2(100, 410), 1.0f);
		GambarPohonBambu(new Vector2(1052, 410), 1.0f);
	}
}
