YouTube: https://youtu.be/_kVfO23iJnw
Figma: https://www.figma.com/design/DL0vuVzO7AAKZJqVCXxb15/Untitled?node-id=0-1&t=8M51sLkLYyw3oFY0-1
Drive: https://drive.google.com/drive/folders/1eHnUoTaQz8AU17DFQTEWPEG6XZYPOCdm?usp=drive_link


[*Untuk desain motif, saya bekerjasama dengan kelas sebelah karena menggunakan motif yang sama. Tetapi, implementasi desain ke C# tetap dikerjakan perorangan.]

=================================================================
              ANGKLUNG 8 NADA - VIRTUAL SIMULATOR
=================================================================

Penulis Program: Farras Ahmad Rasyid
Kelas: 2A D4 Informatika
NIM: 231524006
Mata Kuliah: Komputer Grafik
Semester: 4 
Tahun: 2025

-----------------------------------------------------------------
DESKRIPSI PROGRAM
-----------------------------------------------------------------

"Angklung 8 Nada" adalah aplikasi simulasi alat musik tradisional 
Sunda yang menghadirkan pengalaman interaktif bermain angklung 
secara virtual. Program ini menampilkan visualisasi 8 angklung 
dengan nada yang berbeda (Do-Re-Mi-Fa-Sol-La-Si-Do) yang dapat 
dimainkan menggunakan keyboard.

Terinspirasi dari warisan budaya Sunda yang kaya, aplikasi ini 
tidak hanya berfungsi sebagai media hiburan, tetapi juga sebagai 
sarana edukasi dan pelestarian budaya melalui teknologi modern. 
Visualisasi yang detail dan respons audio yang autentik memberikan 
pengalaman yang mendekati bermain angklung sesungguhnya.

Fitur utama:
• Visualisasi 8 angklung dengan detail yang realistis
• Animasi getaran yang responsif saat angklung dimainkan
• Audio yang otentik untuk setiap nada angklung
• Interaksi intuitif melalui keyboard (tombol A-K)
• Elemen visual pendukung (pohon bambu, pendopo, bunga) yang 
  menciptakan suasana tradisional Indonesia
• Performa yang dioptimalkan untuk pengalaman bermain yang lancar

-----------------------------------------------------------------
JADWAL PLANNING & PROGRESS DEVELOPMENT
-----------------------------------------------------------------

07 April 2025 - Konsep & Riset
• Riset tentang struktur dan cara kerja angklung
• Studi dasar fisika getaran angklung untuk simulasi
• Perancangan konsep awal UI/UX aplikasi
• Menentukan teknologi yang akan digunakan (Godot Engine)

09 April 2025 - Perancangan Dasar
• Sketsa awal desain visual angklung
• Merancang struktur project
• Implementasi kelas dasar dan inheritance
• Pembuatan kelas BaseKarya3n4 sebagai fondasi

11 April 2025 - Implementasi Visual Dasar
• Pembuatan fungsi GambarAngklung
• Implementasi rendering dasar untuk angklung
• Pembuatan visualisasi penyangga
• Implementasi fungsi SisiAngklung

13 April 2025 - Pengembangan Elemen Visual Tambahan
• Implementasi GambarPohonBambu
• Implementasi GambarPendopo
• Implementasi GambarBunga
• Pengaturan tata letak elemen visual

15 April 2025 - Implementasi Sistem Animasi
• Pengembangan mekanisme timing berbasis PhysicsProcess
• Implementasi animasi getaran dasar
• Perancangan sistem transformasi objek
• Optimasi kinerja animasi

17 April 2025 - Integrasi Audio
• Rekaman dan pemrosesan sampel suara angklung
• Implementasi sistem audio playback
• Integrasi trigger audio dengan interaksi
• Pengujian sinkronisasi audio dengan animasi visual

19 April 2025 - Interaksi & Kontrol
• Implementasi sistem mapping keyboard
• Pengembangan respons interaksi user
• Implementasi toggle animasi dengan tombol M
• Penambahan feedback visual untuk interaksi

21 April 2025 - Optimasi & Debugging
• Optimasi performa dengan pre-alokasi memori
• Implementasi interpolasi untuk animasi yang lebih mulus
• Debugging masalah rendering dan respons kontrol
• Optimasi perhitungan matematis untuk transformasi

23 April 2025 - Final Polish & Delivery
• Penyempurnaan visual keseluruhan
• Penambahan dokumen panduan penggunaan
• Peninjauan kode untuk kebersihan dan maintainability
• Pengujian akhir pada berbagai kondisi

-----------------------------------------------------------------
CARA PENGGUNAAN
-----------------------------------------------------------------

1. Jalankan aplikasi melalui Godot Engine atau file executable
2. Gunakan keyboard untuk memainkan angklung:
   • A - Do (nada pertama)
   • S - Re (nada kedua)
   • D - Mi (nada ketiga)
   • F - Fa (nada keempat)
   • G - Sol (nada kelima)
   • H - La (nada keenam)
   • J - Si (nada ketujuh)
   • K - Do tinggi (nada kedelapan)
3. Tekan M untuk mengaktifkan/menonaktifkan animasi getaran motif

-----------------------------------------------------------------
TEKNOLOGI YANG DIGUNAKAN
-----------------------------------------------------------------

• Engine: Godot 
• Bahasa Pemrograman: C#
• Teknik Rendering: Custom pixel-based rendering
• Animasi: Physics-based animation dengan interpolasi
• Audio: Integrated audio system dengan multi-channel support

=================================================================
© 2025 - Alat musik angklung telah diakui UNESCO sebagai Warisan 
Budaya Tak Benda (Intangible Cultural Heritage) sejak 2010.
=================================================================