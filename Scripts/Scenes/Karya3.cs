
using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using System.Numerics;

public partial class Karya3 : BaseKarya3n4
{
	private float time = 0; // Waktu untuk animasi
	private float amplitude = 2; // Amplitudo getaran (dalam piksel)
	private float frequency = 45; // Frekuensi getaran
	private float rotationSpeed = 4.0f; // Kecepatan rotasi bunga (radian per detik)
	private float floatingSpeed = 1.5f; // Kecepatan perpindahan bunga
	protected bool isActive; // Status animasi

	// Variabel untuk menyimpan status aktif
	public override void _EnterTree()
	{
		isActive = true; // Aktifkan animasi ketika scene dimasukkan ke tree
		GD.Print("Karya3: Animasi diaktifkan");

	}

	public override void _ExitTree()
	{
		isActive = false; // Nonaktifkan animasi ketika scene dihapus dari tree
		GD.Print("Karya3: Animasi dinon-aktifkan");

	}
	public override void _Process(double delta)
	{
		if (!isActive) return;
		time += (float)delta; // Perbarui waktu setiap frame

		QueueRedraw(); // Memanggil ulang _Draw untuk menggambar ulang setiap frame
	}

	public override void _Draw()
	{
		if (!isActive) return;

		if (isDrawAxis)
			DrawAxis(Colors.Green);

		if (isDrawMarginBox)
			DrawMarginBox(Colors.Yellow);

		// Animasi getaran untuk setiap angklung
		for (int i = 0; i < 8; i++)
		{
			// Posisi dasar angklung
			Vector2 posisi = new Vector2(120 + i * 120, 450);

			// Buat transformasi getaran
			Matrix4x4 matrix = TransformasiFast.Identity();
			float offsetX = amplitude * MathF.Sin(time * frequency + i); // Gerakan sinusoidal dengan offset berbeda untuk setiap angklung
			transformasi.Translation(ref matrix, offsetX, 0);
			GambarAngklung(posisi, 0.7f, matrix);
		}

		// Animasi getaran untuk motif angklung
		Matrix4x4 motifMatrix = TransformasiFast.Identity();
		float offsetY = amplitude * MathF.Sin(time * frequency); // Gerakan sinusoidal ke atas dan ke bawah
		transformasi.Translation(ref motifMatrix, 0, offsetY);
		MotifAngklung(new Vector2(576, 380), 0.7f, motifMatrix);

		// Animasi rotasi untuk bunga
		// Titik awal dan akhir perpindahan bunga
		Vector2 start1 = new Vector2(164, 125);
		Vector2 end1 = new Vector2(988, 125); // Titik akhir bunga pertama
		Vector2 start2 = new Vector2(988, 125);
		Vector2 end2 = new Vector2(164, 125); // Titik akhir bunga kedua

		// Perpindahan bunga pertama
		float t1 = (MathF.Sin(time * floatingSpeed) + 1) / 2; // Nilai t antara 0 dan 1
		Vector2 interpolatedPos1 = start1.Lerp(end1, t1);

		// Perpindahan bunga kedua
		float t2 = (MathF.Sin(time * floatingSpeed) + 1) / 2; // Offset fase untuk bunga kedua
		Vector2 interpolatedPos2 = start2.Lerp(end2, t2);

		// Rotasi bunga pertama
		float angle1 = time * rotationSpeed; // Sudut rotasi berdasarkan waktu
		Matrix4x4 matrix1 = TransformasiFast.Identity();
		transformasi.RotationClockwise(ref matrix1, angle1, interpolatedPos1);
		GambarBunga(interpolatedPos1, 1.0f, matrix1);

		// Gambar pendopo
		GambarPendopo(new Vector2(576, 420), 0.9f);

		// Rotasi bunga kedua
		float angle2 = time * rotationSpeed; // Sudut rotasi berdasarkan waktu
		Matrix4x4 matrix2 = TransformasiFast.Identity();
		transformasi.RotationClockwise(ref matrix2, angle2, interpolatedPos2);
		GambarBunga(interpolatedPos2, 1.0f, matrix2);

		GambarPohonBambu(new Vector2(100, 410), 1.0f, time);
		GambarPohonBambu(new Vector2(1052, 410), 1.0f, time);

	}

	// Gambar Motif Angklung
	public void MotifAngklung(Vector2 posisi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
	{
		scaleFactor = Math.Max(scaleFactor, 0.5f);
		int S(int original) => (int)Math.Round(original * scaleFactor);
		Matrix4x4 matrix = transform ?? Matrix4x4.Identity; 

		// Komponen dasar
		int tinggiAngklung = S(130);
		int jarakAntarAngklung = S(45);
		int tinggiPenyangga = S(200);
		Vector2 basePos = posisi;

		// Penyangga Angklung
		GambarPenyanggaAngklung(basePos, scaleFactor);

		// Sisi Angklung di beberapa posisi
		for (int i = -4; i < 4; i++)
		{
			float offsetX = i * jarakAntarAngklung + S(20);
			float tinggi = tinggiAngklung - i * S(10);

			// Terapkan transformasi pada setiap sisi angklung
			Vector2 posisiSisi = new Vector2(basePos.X + offsetX, basePos.Y - tinggiPenyangga * 1 / 6);
			SisiAngklung(posisiSisi, tinggi, scaleFactor, matrix);
		}
	}
}
