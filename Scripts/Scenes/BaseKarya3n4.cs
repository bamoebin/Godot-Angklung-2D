using Godot;
using System;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;
using System.Numerics;
namespace Godot;

public partial class BaseKarya3n4 : FilledBentukDasar
{
    
    
    // Gambar Pohon Bambu 
    public void GambarPohonBambu(Vector2 posisi, float scaleFactor = 1.0f, float time = 0)
    {
        scaleFactor = Math.Max(scaleFactor, 0.5f);
        int S(int original) => (int)Math.Round(original * scaleFactor);

        // Komponen dasar
        int batangLebar = S(20);
        int simpulLebar = S(25), simpulTinggi = S(10);
        int jarakAntarBatang = S(40);
        int akarLebar = S(50), akarTinggi = S(25);
        int batangTinggiMin = S(150), batangTinggiMax = S(220);
        Vector2 basePos = posisi;

        // Inisialisasi data bambu
        if (bambuData == null)
        {
            bambuData = new List<(int offsetX, int batangTinggi, List<int> simpulY)>();
            for (int i = -1; i <= 1; i++)
            {
                int offsetX = i * jarakAntarBatang;
                int batangTinggi = batangTinggiMin + (batangTinggiMax - batangTinggiMin) / 2 + i * S(10);

                List<int> simpulY = new List<int>();
                for (int j = 1; j <= 4; j++)
                {
                    int simpulPosY = (int)(basePos.Y - j * batangTinggi / 4);
                    simpulY.Add(simpulPosY);
                }

                bambuData.Add((offsetX, batangTinggi, simpulY));
            }
        }

        // Gambar batang bambu dengan transformasi meninggi dan merendah
        foreach (var data in bambuData)
        {
            int offsetX = data.offsetX;
            int batangTinggi = data.batangTinggi + (int)(MathF.Sin(time + offsetX) * S(10));

            // Gambar batang
            DrawRectOptimized(
                (int)(basePos.X + offsetX - batangLebar / 2),
                (int)(basePos.Y - batangTinggi),
                batangLebar,
                batangTinggi,
                Colors.Green
            );

            // Gambar simpul
            foreach (int simpulY in data.simpulY)
            {
                DrawRectOptimized(
                    (int)(basePos.X + offsetX - simpulLebar / 2),
                    simpulY,
                    simpulLebar,
                    simpulTinggi,
                    Colors.DarkGreen
                );
            }

            // Gambar akar
            DrawIsoscelesTrapezoidOptimized(
                (int)(basePos.X + offsetX),
                (int)(basePos.Y + akarTinggi),
                akarLebar/2,  
                akarLebar/4,  
                akarTinggi,
                Colors.Chocolate
            );
            
            DrawIsoscelesTrapezoidOptimized(
                (int)(basePos.X + offsetX - akarLebar / 2),
                (int)(basePos.Y + akarTinggi),
                akarLebar/4,  
                akarLebar/8,  
                akarTinggi / 2,
                Colors.Chocolate
            );
            
            DrawIsoscelesTrapezoidOptimized(
                (int)(basePos.X + offsetX + akarLebar / 2),
                (int)(basePos.Y + akarTinggi),
                akarLebar/4,  
                akarLebar/8,  
                akarTinggi / 2,
                Colors.Chocolate
            );
        }

        // Gambar rumput
        for (int i = -4; i <= 3; i++)
        {
            int rumputX = (int)(basePos.X + i * S(20));
            int rumputYAtas = (int)(basePos.Y + akarTinggi - S(20) + MathF.Sin(time + i) * S(5));
            int rumputYBawah = (int)(basePos.Y + akarTinggi);

            DrawIsoscelesTriangleOptimized(
                rumputX,
                rumputYBawah,
                S(15),
                rumputYBawah - rumputYAtas,
                "up",
                Colors.LightGreen
            );
        }

        // Gambar daun
        for (int i = 0; i < 7; i++)
        {
            int daunX = (int)(basePos.X + MathF.Sin(time * 2 + i) * S(50));
            int daunY = (int)(basePos.Y - S(200) + (time * 30 % S(100)) + i * S(20));
            
            DrawIsoscelesTriangleOptimized(
                daunX,
                daunY,
                S(10),
                S(15),
                "up",
                Colors.DarkGreen
            );
        }
    }

    // Gambar Motif Bunga
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

        // Gambar Segitiga Utama dengan rotasi
        Vector2[] segitigaPoints = new Vector2[] {
            new Vector2(basePos.X - panjangSegitiga/2, basePos.Y),
            new Vector2(basePos.X, basePos.Y - tinggiSegitiga), 
            new Vector2(basePos.X + panjangSegitiga/2, basePos.Y)
        };
        
        // Rotasi Segitiga
        for (int i = 0; i < 4; i++)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.Identity;
            transformasi.RotationClockwise(ref rotationMatrix, MathF.PI / 2 * i + MathF.PI / 4, basePos);
            
            List<Vector2> transformedPoints = new List<Vector2>();
            foreach (var point in segitigaPoints)
            {
                // Gabungkan rotasi dan transformasi lainnya
                Matrix4x4 combinedMatrix = rotationMatrix * matrix;
                transformedPoints.Add(TransformPoint(combinedMatrix, point));
            }
            DrawPolygon(transformedPoints.ToArray(), new Color[] { Colors.DarkOrange });
        }

        // Lingkaran Kelopak
        DrawCircleOptimized((int)(basePos.X - radiusLingkaranKelopak), (int)basePos.Y, radiusLingkaranKelopak, Colors.Yellow, matrix);
        DrawCircleOptimized((int)(basePos.X + radiusLingkaranKelopak), (int)basePos.Y, radiusLingkaranKelopak, Colors.Yellow, matrix);
        DrawCircleOptimized((int)basePos.X, (int)(basePos.Y - radiusLingkaranKelopak), radiusLingkaranKelopak, Colors.Yellow, matrix);
        DrawCircleOptimized((int)basePos.X, (int)(basePos.Y + radiusLingkaranKelopak), radiusLingkaranKelopak, Colors.Yellow, matrix);

        // Lingkaran Inti
        DrawCircleOptimized((int)basePos.X, (int)basePos.Y, radiusLingkaranInti, Colors.Brown, matrix);
    }

    // Gambar Sisi Angklung
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
    DrawCircleOptimized((int)basePos.X, (int)basePos.Y, radiusLingkaran, Colors.DarkGoldenrod, matrix);

    // Persegi Panjang 1
    DrawRectOptimized(
        (int)(basePos.X - lebarPersegiPanjang1 / 2),
        (int)(basePos.Y - radiusLingkaran - tinggiPersegiPanjang1),
        lebarPersegiPanjang1,
        tinggiPersegiPanjang1,
        Colors.DarkGoldenrod,
        matrix
    );

    // Persegi Panjang 2
    DrawRectOptimized(
        (int)(basePos.X - lebarPersegiPanjang2 / 2),
        (int)(basePos.Y - radiusLingkaran / 4),
        lebarPersegiPanjang2,
        tinggiPersegiPanjang2,
        Colors.SaddleBrown,
        matrix
    );
}

    // Gambar Penyangga Angklung 
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
        DrawRectOptimized(
            (int)(basePos.X - lebarPersegiBawah / 2),
            (int)basePos.Y - radiusLingkaran,
            lebarPersegiBawah,
            tinggiPersegiBawah,
            Colors.DarkGreen
        );

        // Penyangga Kiri
        DrawRectOptimized(
            (int)(basePos.X - lebarPersegiBawah / 2 + lebarPenyangga),
            (int)(basePos.Y - tinggiPenyangga),
            lebarPenyangga,
            tinggiPenyangga,
            Colors.DarkGreen
        );
        
        DrawCircleOptimized(
            (int)(basePos.X - lebarPersegiBawah / 2 + lebarPenyangga * 2 - lebarPenyangga * 1 / 2),
            (int)(basePos.Y + radiusLingkaran),
            radiusLingkaran,
            Colors.DarkGreen,
            null
        );

        // Penyangga Kanan
        DrawRectOptimized(
            (int)(basePos.X + lebarPersegiBawah / 2 - lebarPenyangga * 2),
            (int)(basePos.Y - tinggiPenyangga),
            lebarPenyangga,
            tinggiPenyangga,
            Colors.DarkGreen
        );
        
        DrawCircleOptimized(
            (int)(basePos.X + lebarPersegiBawah / 2 - lebarPenyangga * 2 + lebarPenyangga * 1 / 2),
            (int)(basePos.Y + radiusLingkaran),
            radiusLingkaran,
            Colors.DarkGreen,
            null
        );

        // Persegi Penyimpan (Rotasi)
        Vector2[] persegiPoints = new Vector2[] {
            new Vector2(basePos.X - lebarPersegiPenyimpan/2, basePos.Y - tinggiPenyangga/2),
            new Vector2(basePos.X + lebarPersegiPenyimpan/2, basePos.Y - tinggiPenyangga/2),
            new Vector2(basePos.X + lebarPersegiPenyimpan/2, basePos.Y - tinggiPenyangga/2 + tinggiPersegiPenyimpan),
            new Vector2(basePos.X - lebarPersegiPenyimpan/2, basePos.Y - tinggiPenyangga/2 + tinggiPersegiPenyimpan)
        };
        
        Matrix4x4 rotationMatrix = Matrix4x4.Identity;
        transformasi.RotationClockwise(ref rotationMatrix, sudutRotasi, new Vector2(basePos.X, basePos.Y - tinggiPenyangga / 2));
        
        List<Vector2> transformedPersegiPoints = new List<Vector2>();
        foreach (var point in persegiPoints)
        {
            transformedPersegiPoints.Add(TransformPoint(rotationMatrix, point));
        }
        DrawPolygon(transformedPersegiPoints.ToArray(), new Color[] { Colors.DarkOliveGreen });
    }

    // Gambar Pendopo 
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
    DrawRectOptimized(
        (int)(basePos.X - lebarFondasi / 2),
        (int)basePos.Y,
        lebarFondasi,
        tinggiFondasi,
        Colors.Black
    );
    
    DrawIsoscelesTrapezoidOptimized(
        (int)(basePos.X),
        (int)(basePos.Y),
        lebarTrapesiumFondasiBawah/2,
        lebarTrapesiumFondasiAtas/2,
        tinggiTrapesiumFondasi,
        Colors.Brown
    );

    // Pilar Kiri
    DrawRectOptimized(
        (int)(basePos.X - lebarFondasi / 3 - lebarPilar * 13 / 8),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar),
        lebarPilar,
        tinggiPilar,
        Colors.DarkGoldenrod
    );
    
    DrawIsoscelesTriangleOptimized(
        (int)(basePos.X - lebarFondasi / 3 - lebarPilar * 9 / 8),
        (int)(basePos.Y - tinggiFondasi * 3 / 4),
        lebarSegitigaPilar,
        tinggiSegitigaPilar,
        "up",
        Colors.Chocolate
    );
    
    DrawIsoscelesTriangleOptimized(
        (int)(basePos.X - lebarFondasi / 3 - lebarPilar * 9 / 8),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar * 4 / 5),
        lebarSegitigaPilar,
        tinggiSegitigaPilar,
        "down",
        Colors.Chocolate
    );

    // Pilar Kanan
    DrawRectOptimized(
        (int)(basePos.X + lebarFondasi / 3 + lebarPilar * 5 / 8),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar),
        lebarPilar,
        tinggiPilar,
        Colors.DarkGoldenrod
    );
    
    DrawIsoscelesTriangleOptimized(
        (int)(basePos.X + lebarFondasi / 3 + lebarPilar * 9 / 8),
        (int)(basePos.Y - tinggiFondasi * 3 / 4),
        lebarSegitigaPilar,
        tinggiSegitigaPilar,
        "up",
        Colors.Chocolate
    );
    
    DrawIsoscelesTriangleOptimized(
        (int)(basePos.X + lebarFondasi / 3 + lebarPilar * 9 / 8),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar * 4 / 5),
        lebarSegitigaPilar,
        tinggiSegitigaPilar,
        "down",
        Colors.Chocolate
    );

    // Fondasi Atap
    DrawRectOptimized(
        (int)(basePos.X - lebarFondasiAtap / 2),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap),
        lebarFondasiAtap,
        tinggiFondasiAtap,
        Colors.Brown
    );
    
    // Segitiga Atap
    DrawRightTriangleOptimized(
        (int)(basePos.X - lebarFondasiAtap / 12),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap),
        lebarSegitigaAtap,
        tinggiSegitigaAtap,
        "left",
        Colors.Chocolate
    );
    
    DrawRightTriangleOptimized(
        (int)(basePos.X + lebarFondasiAtap / 12),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap),
        lebarSegitigaAtap,
        tinggiSegitigaAtap,
        "right",
        Colors.Chocolate
    );

    // Trapesium Atap
    DrawIsoscelesTrapezoidOptimized(
        (int)(basePos.X),
        (int)(basePos.Y - tinggiFondasi - tinggiPilar - tinggiFondasiAtap),
        lebarTrapesiumAtapBawah,  
        lebarTrapesiumAtapAtas,   
        tinggiTrapesiumAtap,
        Colors.Brown
    );
}

    // Gambar Angklung 
    public void GambarAngklung(Vector2 posisi, float scaleFactor = 1.0f, Matrix4x4? transform = null)
    {
        scaleFactor = Math.Max(scaleFactor, 0.5f);
        int S(int original) => (int)Math.Round(original * scaleFactor);
        Matrix4x4 matrix = transform ?? Matrix4x4.Identity;

        // Komponen dasar
        int batangTinggi = S(200), batangLebar = S(5), jarakAntarBatang = S(40), lebarBatangHorizontal = S(14);
        int panjangBatangHorizontalTengah = S(50), radiusLingkaran = S(7), ukuranPersegiBawah = S(6);
        Vector2 basePos = posisi;

        // Batang Vertikal
        for (int i = 0; i < 3; i++)
        {
            DrawRectOptimized(
                (int)(basePos.X + i * jarakAntarBatang),
                (int)basePos.Y,
                batangLebar,
                batangTinggi,
                Colors.SaddleBrown,
                matrix
            );
        }

        // Batang Horizontal Bawah, Tengah, Atas & Lingkaran
        int yLingkaran = (int)(basePos.Y + batangTinggi - S(20));
        int xLingkaranKiri = (int)(basePos.X - radiusLingkaran);
        int xLingkaranKanan = (int)(basePos.X + 2 * jarakAntarBatang + batangLebar + radiusLingkaran);

        DrawCircleOptimized(xLingkaranKiri, yLingkaran, radiusLingkaran, Colors.DarkGoldenrod, matrix);
        DrawCircleOptimized(xLingkaranKanan, yLingkaran, radiusLingkaran, Colors.DarkGoldenrod, matrix);
        
        DrawRectOptimized(
            xLingkaranKiri,
            yLingkaran - radiusLingkaran,
            xLingkaranKanan - xLingkaranKiri,
            lebarBatangHorizontal,
            Colors.DarkRed,
            matrix
        );
        
        DrawRectOptimized(
            (int)(basePos.X + jarakAntarBatang),
            (int)(basePos.Y + batangTinggi - S(100)),
            panjangBatangHorizontalTengah,
            lebarBatangHorizontal / 2,
            Colors.SaddleBrown,
            matrix
        );
        
        DrawRectOptimized(
            (int)(basePos.X - radiusLingkaran),
            (int)(basePos.Y + batangTinggi - S(140)),
            xLingkaranKanan - xLingkaranKiri,
            lebarBatangHorizontal / 2,
            Colors.SaddleBrown,
            matrix
        );

        // Persegi Penopang
        int yPersegi = yLingkaran - radiusLingkaran + 1 - ukuranPersegiBawah;
        int xKiri = (int)(basePos.X + jarakAntarBatang / 2 - ukuranPersegiBawah / 2);
        
        DrawRectOptimized(
            xKiri,
            yPersegi,
            ukuranPersegiBawah,
            ukuranPersegiBawah,
            Colors.SaddleBrown,
            matrix
        );
        
        int xKanan = (int)(basePos.X + jarakAntarBatang * 3 / 2 - ukuranPersegiBawah / 2);
        
        DrawRectOptimized(
            xKanan,
            yPersegi,
            ukuranPersegiBawah,
            ukuranPersegiBawah,
            Colors.SaddleBrown,
            matrix
        );

        // Pipa Angklung
        int tinggiTrapesiumBawah = S(60), tinggiTrapesiumAtas = S(100), offsetTrapesiumBawah = S(10), offsetTrapesiumAtas = S(10);
        int yPipa = yPersegi;

        // Trapesium bawah kiri
        Vector2[] trapesiumBawahKiri = new Vector2[]
        {
            new Vector2(xKiri, yPipa), // Kiri bawah
            new Vector2(xKiri, yPipa - tinggiTrapesiumBawah + offsetTrapesiumBawah), // Kiri atas
            new Vector2(xKiri + S(12), yPipa - tinggiTrapesiumBawah), // Kanan atas
            new Vector2(xKiri + S(12), yPipa) // Kanan bawah
        };
        
        List<Vector2> transformedTrapesiumBawahKiri = new List<Vector2>();
        foreach (var point in trapesiumBawahKiri)
        {
            transformedTrapesiumBawahKiri.Add(TransformPoint(matrix, point));
        }
        DrawPolygon(transformedTrapesiumBawahKiri.ToArray(), new Color[] { Colors.DarkGoldenrod });
        
        // Trapesium atas kiri
        Vector2[] trapesiumAtasKiri = new Vector2[]
        {
            new Vector2(xKiri, yPipa - tinggiTrapesiumBawah * 3 / 4), // Kiri bawah
            new Vector2(xKiri, yPipa - tinggiTrapesiumBawah * 3 / 4 - tinggiTrapesiumAtas + offsetTrapesiumAtas), // Kiri atas
            new Vector2(xKiri + S(7), yPipa - tinggiTrapesiumBawah * 3 / 4 - tinggiTrapesiumAtas), // Kanan atas
            new Vector2(xKiri + S(7), yPipa - tinggiTrapesiumBawah * 3 / 4) // Kanan bawah
        };
        
        List<Vector2> transformedTrapesiumAtasKiri = new List<Vector2>();
        foreach (var point in trapesiumAtasKiri)
        {
            transformedTrapesiumAtasKiri.Add(TransformPoint(matrix, point));
        }
        DrawPolygon(transformedTrapesiumAtasKiri.ToArray(), new Color[] { Colors.DarkGoldenrod });
        
        // Trapesium bawah kanan
        Vector2[] trapesiumBawahKanan = new Vector2[]
        {
            new Vector2(xKanan, yPipa), // Kiri bawah
            new Vector2(xKanan, yPipa - tinggiTrapesiumBawah * 4 / 5 + offsetTrapesiumBawah), // Kiri atas
            new Vector2(xKanan + S(12), yPipa - tinggiTrapesiumBawah * 4 / 5), // Kanan atas
            new Vector2(xKanan + S(12), yPipa) // Kanan bawah
        };
        
        List<Vector2> transformedTrapesiumBawahKanan = new List<Vector2>();
        foreach (var point in trapesiumBawahKanan)
        {
            transformedTrapesiumBawahKanan.Add(TransformPoint(matrix, point));
        }
        DrawPolygon(transformedTrapesiumBawahKanan.ToArray(), new Color[] { Colors.DarkGoldenrod });
        
        // Trapesium atas kanan
        Vector2[] trapesiumAtasKanan = new Vector2[]
        {
            new Vector2(xKanan, yPipa - tinggiTrapesiumBawah * 1 / 2), // Kiri bawah
            new Vector2(xKanan, yPipa - tinggiTrapesiumBawah * 1 / 2 - tinggiTrapesiumAtas * 3 / 5 + offsetTrapesiumAtas), // Kiri atas
            new Vector2(xKanan + S(7), yPipa - tinggiTrapesiumBawah * 1 / 2 - tinggiTrapesiumAtas * 3 / 5), // Kanan atas
            new Vector2(xKanan + S(7), yPipa - tinggiTrapesiumBawah * 1 / 2) // Kanan bawah
        };
        
        List<Vector2> transformedTrapesiumAtasKanan = new List<Vector2>();
        foreach (var point in trapesiumAtasKanan)
        {
            transformedTrapesiumAtasKanan.Add(TransformPoint(matrix, point));
        }
        DrawPolygon(transformedTrapesiumAtasKanan.ToArray(), new Color[] { Colors.DarkGoldenrod });
    }
}