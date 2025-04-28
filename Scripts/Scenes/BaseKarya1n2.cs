using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using System.Numerics;
namespace Godot;
public partial class BaseKarya1n2 : BaseKarya
{
    private List<(int offsetX, int batangTinggi, List<int> simpulY)> bambuData = null;

    //Gambar Pohon
    public void GambarPohonBambu(Vector2 posisi, float scaleFactor = 1.0f)
{
    scaleFactor = Math.Max(scaleFactor, 0.5f);
    int S(int original) => (int)Math.Round(original * scaleFactor);

    // Komponen dasar
    int batangLebar = S(20);
    int simpulLebar = S(25), simpulTinggi = S(10);
    int jarakAntarBatang = S(40); // Jarak antar batang lebih besar
    int akarLebar = S(50), akarTinggi = S(25);
    int batangTinggiMin = S(150), batangTinggiMax = S(220);
    Vector2 basePos = posisi;

    // Randomisasi hanya dilakukan sekali
    if (bambuData == null)
    {
        bambuData = new List<(int offsetX, int batangTinggi, List<int> simpulY)>();
        Random random = new Random();

        for (int i = -1; i <= 1; i++)
        {
            int offsetX = i * jarakAntarBatang;
            int batangTinggi = batangTinggiMin + random.Next(batangTinggiMax - batangTinggiMin);

            // Simpul bambu di sepanjang batang
            List<int> simpulY = new List<int>();
            for (int j = 1; j <= 4; j++) // Jumlah simpul
            {
                int simpulPosY = (int)(basePos.Y - j * batangTinggi / 4 + random.Next(-S(5), S(5)));
                simpulY.Add(simpulPosY);
            }

            bambuData.Add((offsetX, batangTinggi, simpulY));
        }
    }

    // Gambar Batang Bambu dengan Tinggi Berbeda
    foreach (var data in bambuData)
    {
        int offsetX = data.offsetX;
        int batangTinggi = data.batangTinggi;

        // Gambar batang
        var batang = bentuk.DrawRectangle((int)(basePos.X + offsetX - batangLebar / 2), (int)(basePos.Y - batangTinggi), batangLebar, batangTinggi);
        PutPixelAll(batang, Colors.Green);

        // Gambar simpul
        foreach (int simpulY in data.simpulY)
        {
            var simpul = bentuk.DrawRectangle((int)(basePos.X + offsetX - simpulLebar / 2), simpulY, simpulLebar, simpulTinggi);
            PutPixelAll(simpul, Colors.DarkGreen);
        }

        // Gambar akar
        var akarUtama = bentuk.DrawIsoscelesTrapezoid((int)(basePos.X + offsetX), (int)(basePos.Y + akarTinggi), akarLebar, akarLebar / 2, akarTinggi);
        PutPixelAll(akarUtama, Colors.Chocolate);
        var akarKiri = bentuk.DrawIsoscelesTrapezoid((int)(basePos.X + offsetX - akarLebar / 2), (int)(basePos.Y + akarTinggi), akarLebar / 2, akarLebar / 4, akarTinggi / 2);
        PutPixelAll(akarKiri, Colors.Chocolate);
        var akarKanan = bentuk.DrawIsoscelesTrapezoid((int)(basePos.X + offsetX + akarLebar / 2), (int)(basePos.Y + akarTinggi), akarLebar / 2, akarLebar / 4, akarTinggi / 2);
        PutPixelAll(akarKanan, Colors.Chocolate);
    }

    // Rumput di Dasar Pohon
    Random randomRumput = new Random();
    for (int i = -4; i <= 3; i++) // jumlah rumput
    {
        int rumputX = (int)(basePos.X + i * S(20));
        int rumputY = (int)(basePos.Y + akarTinggi);
        var rumput = bentuk.DrawIsoscelesTriangle(rumputX, rumputY, S(15 + randomRumput.Next(5)), S(10 + randomRumput.Next(5)), "up");
        PutPixelAll(rumput, Colors.LightGreen);
    }

    // Daun Berterbangan di Sekitar Bambu
    Random randomDaun = new Random();
    for (int i = 0; i < 7; i++) // jumlah daun
    {
        int daunX = (int)(basePos.X + randomDaun.Next(-S(100), S(100))); // Posisi acak di sekitar bambu
        int daunY = (int)(basePos.Y - randomDaun.Next(S(50), S(200))); // Posisi acak di atas bambu
        var daun = bentuk.DrawIsoscelesTriangle(daunX, daunY, S(10), S(15), "up");
        PutPixelAll(daun, Colors.DarkGreen);
    }
}

    // Gambar Bunga
    public void GambarBunga(Vector2 posisi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
{
    scaleFactor = Math.Max(scaleFactor, 0.5f);
    int S(int original) => (int)Math.Round(original * scaleFactor);
    Matrix4x4 matrix = transform ?? Matrix4x4.Identity; 

    int radiusLingkaranInti = S(10);
    int radiusLingkaranKelopak = S(15);
    int panjangSegitiga = S(20);
    int tinggiSegitiga = S(20);
    Vector2 basePos = posisi;

    // Lingkaran Inti
    var lingkaranInti = bentuk.DrawMidCircle((int)basePos.X, (int)basePos.Y, radiusLingkaranInti);
    var transformedLingkaranInti = transformasi.GetTransformPoint(matrix, lingkaranInti);
    PutPixelAll(transformedLingkaranInti, Colors.Brown);

    // Lingkaran Kelopak
    var lingkaranKiri = bentuk.DrawMidCircle((int)(basePos.X - radiusLingkaranKelopak), (int)basePos.Y, radiusLingkaranKelopak);
    var transformedLingkaranKiri = transformasi.GetTransformPoint(matrix, lingkaranKiri);
    PutPixelAll(transformedLingkaranKiri, Colors.Yellow);

    var lingkaranKanan = bentuk.DrawMidCircle((int)(basePos.X + radiusLingkaranKelopak), (int)basePos.Y, radiusLingkaranKelopak);
    var transformedLingkaranKanan = transformasi.GetTransformPoint(matrix, lingkaranKanan);
    PutPixelAll(transformedLingkaranKanan, Colors.Yellow);

    var lingkaranAtas = bentuk.DrawMidCircle((int)basePos.X, (int)(basePos.Y - radiusLingkaranKelopak), radiusLingkaranKelopak);
    var transformedLingkaranAtas = transformasi.GetTransformPoint(matrix, lingkaranAtas);
    PutPixelAll(transformedLingkaranAtas, Colors.Yellow);

    var lingkaranBawah = bentuk.DrawMidCircle((int)basePos.X, (int)(basePos.Y + radiusLingkaranKelopak), radiusLingkaranKelopak);
    var transformedLingkaranBawah = transformasi.GetTransformPoint(matrix, lingkaranBawah);
    PutPixelAll(transformedLingkaranBawah, Colors.Yellow);

    // Gambar Segitiga Utama
    var segitigaUtama = bentuk.DrawIsoscelesTriangle((int)basePos.X - radiusLingkaranKelopak * 2 / 3, (int)(basePos.Y - radiusLingkaranKelopak), panjangSegitiga, tinggiSegitiga, "up");

    // Rotasi Segitiga
    for (int i = 0; i < 4; i++)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.Identity;
        transformasi.RotationClockwise(ref rotationMatrix, MathF.PI / 2 * i + MathF.PI / 4, basePos);
        var rotatedSegitiga = transformasi.GetTransformPoint(rotationMatrix * matrix, segitigaUtama);
        PutPixelAll(rotatedSegitiga, Colors.DarkOrange);
    }
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
        float offsetY = S(2 / 3 * tinggiPenyangga);
        float tinggi = tinggiAngklung - i * S(10);
        SisiAngklung(new Vector2(basePos.X + offsetX, basePos.Y - tinggiPenyangga * 1 / 6), tinggi, scaleFactor, matrix);
    }
}

    //Gambar Sisi Angklung
    public void SisiAngklung(Vector2 posisi, float tinggi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
{
    scaleFactor = Math.Max(scaleFactor, 0.5f);
    int S(int original) => (int)Math.Round(original * scaleFactor);
    Matrix4x4 matrix = transform ?? Matrix4x4.Identity; 

    // Komponen dasar
    int radiusLingkaran = S(10);
    int lebarPersegiPanjang1 = S(20), tinggiPersegiPanjang1 = S((int)tinggi);
    int lebarPersegiPanjang2 = S(5), tinggiPersegiPanjang2 = S((int)(tinggi - 2 * tinggi - tinggi / 3));
    Vector2 basePos = posisi;

    // Lingkaran
    var lingkaran = bentuk.DrawMidCircle((int)basePos.X, (int)basePos.Y, radiusLingkaran);
    var transformedLingkaran = transformasi.GetTransformPoint(matrix, lingkaran);
    PutPixelAll(transformedLingkaran, Colors.DarkGoldenrod);

    // Persegi Panjang 1
    var persegiPanjang1 = bentuk.DrawRectangle((int)(basePos.X - lebarPersegiPanjang1 / 2), (int)(basePos.Y - radiusLingkaran - tinggiPersegiPanjang1), lebarPersegiPanjang1, tinggiPersegiPanjang1);
    var transformedPersegiPanjang1 = transformasi.GetTransformPoint(matrix, persegiPanjang1);
    PutPixelAll(transformedPersegiPanjang1, Colors.DarkGoldenrod);

    // Persegi Panjang 2
    var persegiPanjang2 = bentuk.DrawRectangle((int)(basePos.X - lebarPersegiPanjang2 / 2), (int)(basePos.Y - radiusLingkaran / 4), lebarPersegiPanjang2, tinggiPersegiPanjang2);
    var transformedPersegiPanjang2 = transformasi.GetTransformPoint(matrix, persegiPanjang2);
    PutPixelAll(transformedPersegiPanjang2, Colors.SaddleBrown);
}

    //Gambar Penyangga Angklung
    public void GambarPenyanggaAngklung(Vector2 posisi, float scaleFactor = 1.0f)
    {
        scaleFactor = Math.Max(scaleFactor, 0.5f);
        int S(int original) => (int)Math.Round(original * scaleFactor);

        int lebarPersegiBawah = S(500), tinggiPersegiBawah = S(10);
        int tinggiPenyangga = S(200), lebarPenyangga = S(20);
        int radiusLingkaran = S(12);
        int lebarPersegiPenyimpan = S(420), tinggiPersegiPenyimpan = S(20);
        float sudutRotasi = -10 * MathF.PI / 180.0f;
        Vector2 basePos = posisi;

        // Persegi Bawah
        var persegiBawah = bentuk.DrawRectangle((int)(basePos.X - lebarPersegiBawah / 2), (int)basePos.Y - radiusLingkaran, lebarPersegiBawah, tinggiPersegiBawah);
        PutPixelAll(persegiBawah, Colors.DarkGreen);
        // Penyangga Kiri
        List<Vector2> penyanggaKiri = new List<Vector2>();
        var persegiPenyanggaKiri = bentuk.DrawRectangle((int)(basePos.X - lebarPersegiBawah / 2 + lebarPenyangga), (int)(basePos.Y - tinggiPenyangga), lebarPenyangga, tinggiPenyangga);
        penyanggaKiri.AddRange(persegiPenyanggaKiri);
        PutPixelAll(persegiPenyanggaKiri, Colors.DarkGreen);
        var lingkaranPenyanggaKiri = bentuk.DrawMidCircle((int)(basePos.X - lebarPersegiBawah / 2 + lebarPenyangga * 2 - lebarPenyangga * 1 / 2), (int)(basePos.Y + radiusLingkaran), radiusLingkaran);
        penyanggaKiri.AddRange(lingkaranPenyanggaKiri);
        PutPixelAll(lingkaranPenyanggaKiri, Colors.DarkGreen);
        // Penyangga Kanan (Mirror dari Penyangga Kiri)
        var penyanggaKanan = transformasi.Mirror(penyanggaKiri, 1, basePos);
        PutPixelAll(penyanggaKanan, Colors.DarkGreen);
        // Persegi Penyimpan (Rotasi)
        Matrix4x4 matrix = Matrix4x4.Identity;
        transformasi.RotationClockwise(ref matrix, sudutRotasi, new Vector2(basePos.X, basePos.Y - tinggiPenyangga / 2));
        var persegiPenyimpan = bentuk.DrawRectangle((int)(basePos.X - lebarPersegiPenyimpan / 2), (int)(basePos.Y - tinggiPenyangga / 2), lebarPersegiPenyimpan, tinggiPersegiPenyimpan);
        var persegiPenyimpanRotated = transformasi.GetTransformPoint(matrix, persegiPenyimpan);
        PutPixelAll(persegiPenyimpanRotated, Colors.DarkOliveGreen);
    }

    //Gambar Pendopo
    public void GambarPendopo(Vector2 posisi, float scaleFactor = 1.0f)
    {
        scaleFactor = Math.Max(scaleFactor, 0.5f);
        int S(int original) => (int)Math.Round(original * scaleFactor);

        // Komponen dasar
        int lebarFondasi = S(760), tinggiFondasi = S(20);
        int lebarTrapesiumFondasiBawah = S(760), lebarTrapesiumFondasiAtas = S(700), tinggiTrapesiumFondasi = S(17);
        int tinggiPilar = S(270), lebarPilar = S(40);
        int lebarSegitigaPilar = S(50), tinggiSegitigaPilar = S(60);
        int lebarFondasiAtap = S(825), tinggiFondasiAtap = S(10);
        int lebarSegitigaAtap = S(300), tinggiSegitigaAtap = S(70);
        int lebarTrapesiumAtapBawah = S(175), lebarTrapesiumAtapAtas = S(65), tinggiTrapesiumAtap = S(150);
        Vector2 basePos = posisi;

        // Fondasi Pendopo
        var fondasi = bentuk.DrawRectangle((int)(basePos.X - lebarFondasi / 2), (int)basePos.Y, lebarFondasi, tinggiFondasi);
        PutPixelAll(fondasi, Colors.Black);
        var trapesiumFondasi = bentuk.DrawIsoscelesTrapezoid((int)(basePos.X), (int)(basePos.Y), lebarTrapesiumFondasiBawah, lebarTrapesiumFondasiAtas, tinggiTrapesiumFondasi);
        PutPixelAll(trapesiumFondasi, Colors.Brown);

        // Pilar Kiri
        var pilarKiri = bentuk.DrawRectangle((int)(basePos.X - lebarFondasi / 3 - lebarPilar), (int)(basePos.Y - tinggiFondasi - tinggiPilar), lebarPilar, tinggiPilar);
        PutPixelAll(pilarKiri, Colors.DarkGoldenrod);
        var segitigaBawahKiri = bentuk.DrawIsoscelesTriangle((int)(basePos.X - lebarFondasi / 3 - lebarPilar * 9 / 8), (int)(basePos.Y - tinggiFondasi), lebarSegitigaPilar, tinggiSegitigaPilar, "up");
        PutPixelAll(segitigaBawahKiri, Colors.Chocolate);
        var segitigaAtasKiri = bentuk.DrawIsoscelesTriangle((int)(basePos.X - lebarFondasi / 3 - lebarPilar * 9 / 8), (int)(basePos.Y - tinggiFondasi - tinggiPilar), lebarSegitigaPilar, tinggiSegitigaPilar, "down");
        PutPixelAll(segitigaAtasKiri, Colors.Chocolate);

        // Pilar Kanan
        var pilarKanan = transformasi.Mirror(pilarKiri, 1, new Vector2(basePos.X, basePos.Y));
        PutPixelAll(pilarKanan, Colors.DarkGoldenrod);
        var segitigaBawahKanan = transformasi.Mirror(segitigaBawahKiri, 1, new Vector2(basePos.X, basePos.Y));
        PutPixelAll(segitigaBawahKanan, Colors.Chocolate);
        var segitigaAtasKanan = transformasi.Mirror(segitigaAtasKiri, 1, new Vector2(basePos.X, basePos.Y));
        PutPixelAll(segitigaAtasKanan, Colors.Chocolate);

        // Fondasi Atap
        var fondasiAtap = bentuk.DrawRectangle((int)(basePos.X - lebarFondasiAtap / 2), (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap), lebarFondasiAtap, tinggiFondasiAtap);
        PutPixelAll(fondasiAtap, Colors.Brown);

        // Segitiga Atap
        var segitigaKiriAtap = bentuk.DrawRightTriangle((int)(basePos.X - lebarFondasiAtap / 10), (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap), lebarSegitigaAtap, tinggiSegitigaAtap, "left");
        PutPixelAll(segitigaKiriAtap, Colors.Chocolate);
        var segitigaKananAtap = bentuk.DrawRightTriangle((int)(basePos.X + lebarFondasiAtap / 10), (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap), lebarSegitigaAtap, tinggiSegitigaAtap, "right");
        PutPixelAll(segitigaKananAtap, Colors.Chocolate);

        // Trapesium Atap
        var trapesiumAtap = bentuk.DrawIsoscelesTrapezoid((int)(basePos.X), (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap), lebarTrapesiumAtapBawah*2 , lebarTrapesiumAtapAtas*2, tinggiTrapesiumAtap);
        PutPixelAll(trapesiumAtap, Colors.Brown);
    }

    //Gambar Angklung
    public void GambarAngklung(Vector2 posisi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
{
    scaleFactor = Math.Max(scaleFactor, 0.5f);
    int S(int original) => (int)Math.Round(original * scaleFactor);
    Matrix4x4 matrix = transform ?? Matrix4x4.Identity; 

    // Komponen dasar
    int batangTinggi = S(200), batangLebar = S(5), jarakAntarBatang = S(40), lebarBatangHorizontal = S(14), panjangBatangHorizontalTengah = S(50), radiusLingkaran = S(7), ukuranPersegiBawah = S(6);
    Vector2 basePos = posisi;

    // Batang Vertikal
    for (int i = 0; i < 3; i++)
    {
        var batangVertikal = bentuk.DrawRectangle((int)(basePos.X + i * jarakAntarBatang), (int)basePos.Y, batangLebar, batangTinggi);
        var transformedBatangVertikal = transformasi.GetTransformPoint(matrix, batangVertikal);
        PutPixelAll(transformedBatangVertikal, Colors.SaddleBrown);
    }

    // Batang Horizontal Bawah, Tengah, Atas & Lingkaran
    int yLingkaran = (int)(basePos.Y + batangTinggi - S(20));
    int xLingkaranKiri = (int)(basePos.X - radiusLingkaran);
    int xLingkaranKanan = (int)(basePos.X + 2 * jarakAntarBatang + batangLebar + radiusLingkaran);

    var lingkaranKiri = bentuk.DrawMidCircle(xLingkaranKiri, yLingkaran, radiusLingkaran);
    var transformedLingkaranKiri = transformasi.GetTransformPoint(matrix, lingkaranKiri);
    PutPixelAll(transformedLingkaranKiri, Colors.DarkGoldenrod);

    var lingkaranKanan = bentuk.DrawMidCircle(xLingkaranKanan, yLingkaran, radiusLingkaran);
    var transformedLingkaranKanan = transformasi.GetTransformPoint(matrix, lingkaranKanan);
    PutPixelAll(transformedLingkaranKanan, Colors.DarkGoldenrod);

    var batangHorizontalBawah = bentuk.DrawRectangle(xLingkaranKiri, yLingkaran - radiusLingkaran, xLingkaranKanan - xLingkaranKiri, lebarBatangHorizontal);
    var transformedBatangHorizontalBawah = transformasi.GetTransformPoint(matrix, batangHorizontalBawah);
    PutPixelAll(transformedBatangHorizontalBawah, Colors.DarkRed);

    var batangHorizontalTengah = bentuk.DrawRectangle((int)(basePos.X + jarakAntarBatang), (int)(basePos.Y + batangTinggi - S(100)), panjangBatangHorizontalTengah, lebarBatangHorizontal / 2);
    var transformedBatangHorizontalTengah = transformasi.GetTransformPoint(matrix, batangHorizontalTengah);
    PutPixelAll(transformedBatangHorizontalTengah, Colors.SaddleBrown);

    var batangHorizontalAtas = bentuk.DrawRectangle((int)(basePos.X - radiusLingkaran), (int)(basePos.Y + batangTinggi - S(140)), xLingkaranKanan - xLingkaranKiri, lebarBatangHorizontal / 2);
    var transformedBatangHorizontalAtas = transformasi.GetTransformPoint(matrix, batangHorizontalAtas);
    PutPixelAll(transformedBatangHorizontalAtas, Colors.SaddleBrown);

    // Persegi Penopang
    int yPersegi = yLingkaran - radiusLingkaran + 1 - ukuranPersegiBawah;
    int xKiri = (int)(basePos.X + jarakAntarBatang / 2 - ukuranPersegiBawah / 2);
    var persegiKiri = bentuk.DrawRectangle(xKiri, yPersegi, ukuranPersegiBawah, ukuranPersegiBawah);
    var transformedPersegiKiri = transformasi.GetTransformPoint(matrix, persegiKiri);
    PutPixelAll(transformedPersegiKiri, Colors.SaddleBrown);

    int xKanan = (int)(basePos.X + jarakAntarBatang * 3 / 2 - ukuranPersegiBawah / 2);
    var persegiKanan = bentuk.DrawRectangle(xKanan, yPersegi, ukuranPersegiBawah, ukuranPersegiBawah);
    var transformedPersegiKanan = transformasi.GetTransformPoint(matrix, persegiKanan);
    PutPixelAll(transformedPersegiKanan, Colors.SaddleBrown);

    // Pipa Angklung
    int tinggiTrapesiumBawah = S(60), tinggiTrapesiumAtas = S(100), offsetTrapesiumBawah = S(6), offsetTrapesiumAtas = S(6);
    int yPipa = yPersegi;
    var trapesiumBawahKiri = bentuk.DrawTrapezoid(xKiri, yPipa, S(12), tinggiTrapesiumBawah, offsetTrapesiumBawah);
    var transformedTrapesiumBawahKiri = transformasi.GetTransformPoint(matrix, trapesiumBawahKiri);
    PutPixelAll(transformedTrapesiumBawahKiri, Colors.DarkGoldenrod);

    var trapesiumAtasKiri = bentuk.DrawTrapezoid(xKiri, yPipa - tinggiTrapesiumBawah * 3 / 4, S(7), tinggiTrapesiumAtas, offsetTrapesiumAtas);
    var transformedTrapesiumAtasKiri = transformasi.GetTransformPoint(matrix, trapesiumAtasKiri);
    PutPixelAll(transformedTrapesiumAtasKiri, Colors.DarkGoldenrod);

    var trapesiumBawahKanan = bentuk.DrawTrapezoid(xKanan, yPipa, S(12), tinggiTrapesiumBawah * 4 / 5, offsetTrapesiumBawah);
    var transformedTrapesiumBawahKanan = transformasi.GetTransformPoint(matrix, trapesiumBawahKanan);
    PutPixelAll(transformedTrapesiumBawahKanan, Colors.DarkGoldenrod);

    var trapesiumAtasKanan = bentuk.DrawTrapezoid(xKanan, yPipa - tinggiTrapesiumBawah * 1 / 2, S(7), tinggiTrapesiumAtas * 3 / 5, offsetTrapesiumAtas);
    var transformedTrapesiumAtasKanan = transformasi.GetTransformPoint(matrix, trapesiumAtasKanan);
    PutPixelAll(transformedTrapesiumAtasKanan, Colors.DarkGoldenrod);
}

}