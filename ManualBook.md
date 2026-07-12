# Manual Book: WoWo Defense

**WoWo Defense** adalah game First-Person Shooter (FPS) 3D bergenre tower defense / object defense yang dikembangkan menggunakan Unity dan *Infima Games - Low Poly Shooter Pack*. Di game ini, pemain bertugas untuk melindungi Kuil suci di tengah desa dari serangan gerombolan bandit yang datang bergelombang (waves).

---

## 1. Misi Utama & Mekanisme Kemenangan
Tujuan utama Anda adalah **melindungi Kuil (Temple)** dari kehancuran:
* **Kuil Health Points (HP):** Kuil memiliki HP maksimum sebesar **100**.
* **Kondisi Kalah (Game Over):** Jika musuh berhasil mendekati kuil dan mengurangi HP kuil hingga mencapai **0**, permainan akan berakhir dan layar *Game Over* akan muncul.
* **Kondisi Menang (Victory):** Bertahan dan kalahkan semua gelombang musuh (total **3 Wave**) untuk menyelesaikan level.

---

## 2. Sistem Wave (Gelombang Musuh)
Musuh datang secara bertahap dalam sistem Wave yang diatur oleh `WaveManager`. Setiap kali Wave selesai dibersihkan, Anda memiliki waktu jeda **5 detik** untuk bersiap sebelum Wave berikutnya dimulai.

### Spesifikasi Wave per Level:
* **Jumlah Wave:** 3 Wave per level.
* **Jumlah Musuh per Wave:**
  * **Wave 1:** 8 musuh
  * **Wave 2:** 12 musuh
  * **Wave 3:** 16 musuh
* **Lokasi Spawn:** Musuh akan muncul secara acak dari berbagai titik spawn (*Spawn Points*) di sekitar peta secara menyebar.

---

## 3. Karakteristik Musuh (Enemies)
Musuh bertindak sebagai unit penabrak bunuh diri (*suicide units*). Mereka akan mencari jalur terbaik menuju kuil menggunakan NavMesh, menabrakkan diri untuk memberikan damage besar pada kuil, lalu hancur/mati.

### Tipe Musuh Berdasarkan Level:
| Atribut | Level 1 (Bandit Biasa) | Level 2 (Bandit Raksasa / Giant Boss) |
| :--- | :--- | :--- |
| **Ukuran (Scale)** | Normal (Skala 1.5x) | Raksasa (Skala 2.6x) |
| **Darah (HP)** | **30 HP** | **200 HP** *(bertambah +50 tiap Wave)* |
| **Kecepatan** | Cepat (Base 1.5 + varian acak) | Lambat (Base 0.5 + peningkatan lambat) |
| **Damage ke Kuil**| **5 Damage** per tabrakan | **30 Damage** per tabrakan *(Sangat Berbahaya!)* |

---

## 4. Sistem Persenjataan (Weapons)
Pemain dibekali dengan dua jenis senjata api modern dengan sistem amunisi:
* **Senjata Utama (Assault Rifle):** Memiliki daya hancur tinggi dan tembakan otomatis (automatic fire), sangat cocok untuk membersihkan kelompok musuh.
* **Senjata Kedua (Pistol):** Senjata cadangan semi-otomatis dengan mobilitas tinggi.

### Fitur Amunisi & HUD:
Sisa peluru Anda ditampilkan pada layar HUD. Warna indikator peluru akan berubah secara dinamis untuk memperingatkan Anda:
* **Putih:** Amunisi aman.
* **Kuning (Peluru ≤ 10):** Peringatan amunisi mulai menipis.
* **Merah (Peluru ≤ 5):** Kritis! Segera lakukan isi ulang (*Reload*).

---

## 5. Panduan Kontrol Lengkap

### A. Kontrol Mobile (HP/Tablet)
Jika dimainkan pada perangkat seluler, kontrol menggunakan tombol virtual pada layar:
* **Analog Kiri:** Bergerak ke depan, belakang, kiri, atau kanan.
* **Layar Bagian Kanan:** Geser jari pada area kanan layar untuk memutar kamera atau mengarahkan bidikan.
* **Analog Roda Kanan (⊕):** Tekan untuk menembak secara otomatis (Tahan dan Seret analog ini untuk membidik secara fleksibel).
* **Tombol Aim:** Masuk/keluar dari mode bidik keker (ADS - Aim Down Sights) untuk akurasi lebih tinggi.
* **Tombol R (Reload):** Mengisi ulang amunisi senjata.
* **Tombol J (Jump):** Melompat untuk melewati rintangan.
* **Tombol Run:** Berlari cepat (*Sprint*) untuk berpindah tempat secara kilat.
* **Tombol 1:** Mengganti senjata aktif ke **Assault Rifle**.
* **Tombol 2:** Mengganti senjata aktif ke **Pistol**.

### B. Kontrol PC (Keyboard & Mouse)
Jika dimainkan pada PC, gunakan kombinasi keyboard dan mouse standar:
* **Tombol W, A, S, D:** Bergerak (Maju, Kiri, Mundur, Kanan).
* **Mouse Movement:** Mengarahkan kamera dan arah bidikan karakter.
* **Klik Kiri Mouse:** Menembak senjata (Fire).
* **Klik Kanan Mouse:** Membidik dengan keker (ADS - Aim Down Sights).
* **Tombol R:** Mengisi ulang peluru (Reload).
* **Tombol Spasi (Space):** Melompat.
* **Tombol Run (Shift Kiri):** Berlari cepat (Sprint).
* **Tombol E:** Mengambil/memungut senjata baru yang berada di tanah (*Weapon Pickup*).
* **Tombol 1 / 2:** Mengganti senjata (1 untuk Assault Rifle, 2 untuk Pistol).
* **Tombol Escape (Esc):** Menjeda permainan (*Pause Game*).

---

## 6. Antarmuka Pengguna (User Interface)
* **Instructions Panel:** Panel panduan yang muncul di awal level untuk menjelaskan misi dan kontrol. Permainan akan otomatis terjeda (paused) selama panel ini aktif. Tekan tombol **Start** untuk memulai aksi.
* **Pause Menu:** Menjeda permainan dengan menekan tombol Pause atau tombol `Esc`. Di sini Anda bisa memilih untuk melanjutkan game (*Resume*) atau kembali ke menu utama (*Main Menu*).
* **Game Over Screen:** Menjeda permainan saat HP Kuil habis. Anda dapat memilih **Retry** untuk mengulang level tersebut.
* **Victory Screen:** Muncul ketika semua wave berhasil dibersihkan. Memungkinkan Anda melanjutkan ke **Next Level** (jika di Level 1) atau kembali ke menu utama.

---

## 7. Tips & Strategi Sukses
1. **Fokus ke Raksasa di Level 2:** Bandit raksasa memiliki HP tebal dan bisa menghancurkan Kuil hanya dengan 4 kali tabrakan (30 damage per tabrakan). Prioritaskan tembakan ke kepala (*headshot*) mereka sejak dini.
2. **Manfaatkan Jeda Wave:** Gunakan waktu jeda 5 detik antar wave untuk mengisi penuh amunisi Anda agar tidak kehabisan peluru di tengah pertempuran.
3. **Gunakan Tombol Run secara Taktis:** Jangan berdiri di satu tempat terlalu lama. Berlarilah ke sudut pertahanan lain saat musuh mulai spawn dari titik yang berlawanan.
