using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using System.Numerics;
namespace Godot;

public partial class Karya4 : BaseKarya3n4
{
	private float time = 0; // Waktu untuk animasi
	private float amplitude = 2; // Amplitudo getaran (dalam piksel)
	private float frequency = 45; // Frekuensi getaran
	protected bool isActive; // Status animasi

	// Variabel untuk menyimpan hasil perhitungan
	private List<Vector2> angklungPositions = new List<Vector2>(8); // Prealokasi ukuran
	private List<Matrix4x4> angklungTransforms = new List<Matrix4x4>(8); // Prealokasi ukuran
	private Matrix4x4 motifMatrix;
	private Vector2 bunga1Position;
	private Vector2 bunga2Position;
	private Matrix4x4 bunga1Transform;
	private Matrix4x4 bunga2Transform;


	// Variabel untuk animasi getaran dan suara
	private bool[] angklungVibrating = new bool[8]; // Status getaran untuk setiap angklung
	private AudioStreamPlayer[] angklungSounds = new AudioStreamPlayer[8]; // Suara untuk setiap angklung

	// Variabel untuk motif angklung
	private bool motifVibrating = false; // Status getaran untuk motif angklung

	// Cache untuk nilai-nilai yang sering dihitung
	private float cachedVibrationSin = 0;
	private float prevCachedVibrationSin = 0; // Untuk interpolasi
	private Vector2[] staticPositions = new Vector2[8];

	// Pre-alokasi untuk menghindari alokasi memori berulang
	private Vector2 tempPosition = new Vector2();
	private float smoothDelta = 0;
	private const float SMOOTHING_FACTOR = 0.1f;

	// Mapping tombol ke indeks angklung
	private Dictionary<Key, int> keyToAngklungIndex = new Dictionary<Key, int>
	{
		{ Key.A, 0 }, // Do
		{ Key.S, 1 }, // Re
		{ Key.D, 2 }, // Mi
		{ Key.F, 3 }, // Fa
		{ Key.G, 4 }, // So
		{ Key.H, 5 }, // La
		{ Key.J, 6 }, // Si
		{ Key.K, 7 }  // Do
	};

	public override void _EnterTree()
	{
		isActive = true; // Aktifkan animasi ketika scene dimasukkan ke tree
		GD.Print("Karya4: Animasi diaktifkan");

		// Prealokasi dan inisialisasi collections untuk menghindari realokasi terus-menerus
		for (int i = 0; i < 8; i++)
		{
			if (angklungPositions.Count < 8) angklungPositions.Add(new Vector2());
			if (angklungTransforms.Count < 8) angklungTransforms.Add(TransformasiFast.Identity());

			// Inisialisasi posisi statis (tidak berubah)
			staticPositions[i] = new Vector2(120 + i * 120, 450);
		}
	}

	// Tambahkan metode Ready untuk memuat resource audio
	public override void _Ready()
	{
		// Inisialisasi suara untuk setiap angklung
		for (int i = 0; i < 8; i++)
		{
			// Cek apakah node sudah ada
			string nodeName = $"AngklungSound{i + 1}";
			AudioStreamPlayer existingPlayer = GetNodeOrNull<AudioStreamPlayer>(nodeName);

			if (existingPlayer != null)
			{
				// Gunakan node yang sudah ada
				angklungSounds[i] = existingPlayer;
				GD.Print($"Ditemukan node audio yang sudah ada: {nodeName}");
			}
			else
			{
				// Buat node AudioStreamPlayer baru
				AudioStreamPlayer player = new AudioStreamPlayer();
				player.Name = nodeName;

				// Load audio file dari Assets\Audio\Karya4
				string audioPath = "";

				// Coba beberapa ekstensi file yang mungkin
				foreach (string ext in new[] { ".ogg", ".wav", ".mp3" })
				{
					string pathWithExt = $"res://Assets/Audio/Karya4/AngklungSound{i + 1}{ext}";
					if (ResourceLoader.Exists(pathWithExt))
					{
						audioPath = pathWithExt;
						break;
					}
				}

				if (string.IsNullOrEmpty(audioPath))
				{
					GD.PrintErr($"File audio tidak ditemukan untuk: AngklungSound{i + 1}");
					continue;
				}

				// Load audio stream
				AudioStream stream = ResourceLoader.Load<AudioStream>(audioPath);

				if (stream != null)
				{
					player.Stream = stream;
					AddChild(player);
					angklungSounds[i] = player;
					GD.Print($"Berhasil memuat audio: {audioPath}");
				}
				else
				{
					GD.PrintErr($"Tidak dapat memuat file audio: {audioPath}");
				}
			}
		}
	}

	public override void _ExitTree()
	{
		isActive = false; // Nonaktifkan animasi ketika scene dihapus dari tree
		GD.Print("Karya4: Animasi dinon-aktifkan");
	}

	// Konversi ke PhysicsProcess untuk timing yang lebih konsisten
	public override void _PhysicsProcess(double delta)
	{
		if (!isActive) return;
		float deltaFloat = (float)delta;
		smoothDelta = Mathf.Lerp(smoothDelta, deltaFloat, SMOOTHING_FACTOR);
		time += smoothDelta;
		// Simpan nilai vibration sebelumnya untuk interpolasi
		prevCachedVibrationSin = cachedVibrationSin;
		UpdateAnimasiKarya4(smoothDelta);
		QueueRedraw();
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (keyToAngklungIndex.TryGetValue(eventKey.Keycode, out int index))
			{
				if (eventKey.Pressed && !eventKey.IsEcho())
				{
					// Logika untuk menyalakan angklung saat tombol ditekan saja
					angklungVibrating[index] = true;
					if (angklungSounds[index] != null)
					{
						// Jika sound sedang dimainkan, hentikan sebelum memulai lagi
						if (angklungSounds[index].Playing)
						{
							angklungSounds[index].Stop();
						}
						angklungSounds[index].Play();
					}
				}
				else if (!eventKey.Pressed)
				{
					// Logika untuk mematikan angklung saat tombol dilepas
					angklungVibrating[index] = false;
					if (angklungSounds[index] != null)
					{
						angklungSounds[index].Stop();
					}
				}
			}
			else if (eventKey.Keycode == Key.M && eventKey.Pressed && !eventKey.IsEcho())
			{
				motifVibrating = !motifVibrating;
			}
		}
	}

	public override void _Draw()
	{
		if (!isActive) return;

		float t = (float)Engine.GetPhysicsInterpolationFraction();
		float interpolatedVibration = Mathf.Lerp(prevCachedVibrationSin, cachedVibrationSin, t);

		GambarPohonBambu(new Vector2(100, 410), 1.0f, time);
		GambarPohonBambu(new Vector2(1052, 410), 1.0f, time);

		// Gambar angklung dengan animasi getaran
		for (int i = 0; i < angklungPositions.Count; i++)
		{
			tempPosition.X = angklungPositions[i].X;
			tempPosition.Y = angklungPositions[i].Y;

			if (angklungVibrating[i])
			{
				tempPosition.X += interpolatedVibration;
			}
			GambarAngklung(tempPosition, 0.7f, angklungTransforms[i]);
		}

		// Gambar motif
		MotifAngklung(new Vector2(576, 380), 0.7f, motifMatrix);
		// Gambar bunga
		GambarBunga(bunga1Position, 1.0f, bunga1Transform);
		GambarPendopo(new Vector2(576, 420), 0.9f);
		GambarBunga(bunga2Position, 1.0f, bunga2Transform);
	}

	public void MotifAngklung(Vector2 posisi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
	{
		scaleFactor = Math.Max(scaleFactor, 0.5f);
		int S(int original) => (int)Math.Round(original * scaleFactor);
		Matrix4x4 matrix = transform ?? Matrix4x4.Identity;

		int tinggiAngklung = S(130);
		int jarakAntarAngklung = S(45);
		int tinggiPenyangga = S(200);
		float penyanggaY = tinggiPenyangga / 6;
		int positionOffset = S(20);

		// Penyangga Angklung
		GambarPenyanggaAngklung(posisi, scaleFactor);

		// Sisi Angklung di beberapa posisi
		for (int i = 0; i < 8; i++)
		{
			int posIndex = i - 4;
			float offsetX = posIndex * jarakAntarAngklung + positionOffset;
			float tinggi = tinggiAngklung - posIndex * S(10);

			tempPosition.X = posisi.X + offsetX;
			tempPosition.Y = posisi.Y - penyanggaY;
			Matrix4x4 sisiMatrix = matrix;

			float t = (float)Engine.GetPhysicsInterpolationFraction();
			float interpolatedVibration = Mathf.Lerp(prevCachedVibrationSin, cachedVibrationSin, t);

			// Tambahkan animasi getaran hanya jika aktif
			if (angklungVibrating[i])
			{
				tempPosition.Y += interpolatedVibration;
				transformasi.Translation(ref sisiMatrix, 0, interpolatedVibration);
			}

			// Gambar sisi angklung
			SisiAngklung(tempPosition, tinggi, scaleFactor, sisiMatrix);
		}
	}

	// Memisahkan logika animasi ke metode terpisah
	private void UpdateAnimasiKarya4(float delta)
	{
		float timeFreq = time * frequency;
		cachedVibrationSin = MathF.Sin(time * 20) * 5;

		float normalizedSin = 0.5f;
		if (isFlowerAnimationActive)
		{
			float timeFloatSin = MathF.Sin(time * floatingSpeed);
			normalizedSin = (timeFloatSin + 1) / 2;
		}

		bunga1Position = new Vector2(164, 125).Lerp(new Vector2(988, 125), normalizedSin);
		bunga2Position = new Vector2(988, 125).Lerp(new Vector2(164, 125), normalizedSin);

		// Update transformasi angklung tanpa mengalokasikan memori baru
		for (int i = 0; i < 8; i++)
		{
			// Gunakan posisi statis yang telah dihitung
			angklungPositions[i] = staticPositions[i];

			// Reset transformasi ke identity
			angklungTransforms[i] = TransformasiFast.Identity();

			// Hanya terapkan getaran jika angklung sedang aktif
			if (angklungVibrating[i])
			{
				float offsetX = amplitude * MathF.Sin(timeFreq + i);
				var matrix = angklungTransforms[i];
				transformasi.Translation(ref matrix, offsetX, 0);
				angklungTransforms[i] = matrix;
			}
		}

		// Hitung transformasi motif angklung
		motifMatrix = TransformasiFast.Identity();
		if (motifVibrating)
		{
			float offsetY = amplitude * MathF.Sin(timeFreq);
			transformasi.Translation(ref motifMatrix, 0, offsetY);
		}

		if (isFlowerAnimationActive)
		{
			float angle1 = time * rotationSpeed;
			bunga1Transform = TransformasiFast.Identity();
			transformasi.RotationClockwise(ref bunga1Transform, angle1, bunga1Position);

			float angle2 = time * rotationSpeed;
			bunga2Transform = TransformasiFast.Identity();
			transformasi.RotationClockwise(ref bunga2Transform, angle2, bunga2Position);
		}
		else
		{
			bunga1Transform = TransformasiFast.Identity();
			bunga2Transform = TransformasiFast.Identity();
		}
	}
}
