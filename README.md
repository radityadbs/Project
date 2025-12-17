# Final Project Blazor .NET – Microservice & Docker (Ringkasan)

## Deskripsi Singkat
Aplikasi ini merupakan **Blazor .NET Fullstack (InteractiveServer)** berbasis **Microservice** yang dijalankan menggunakan **Docker**.  
Aplikasi berfungsi sebagai **Sistem Pendaftaran Kredit Sederhana** dengan fitur registrasi pengguna, verifikasi email menggunakan **RabbitMQ**, autentikasi login, pengelolaan profil, serta integrasi **API eksternal** untuk menampilkan data desa.

---

## Teknologi
- .NET 8 & Blazor InteractiveServer  
- Minimal API  
- Entity Framework Core (Code First)  
- MSSQL Server  
- RabbitMQ  
- EmailWorker (Background Service)  
- Docker & Docker Compose  

---

## Arsitektur
Aplikasi terdiri dari beberapa service:
- **FinalProject**  
  Blazor UI + Backend API (Register, Login, Profile, Upload Foto, API Desa)
- **EmailWorker**  
  Background service untuk mengirim email verifikasi dari RabbitMQ
- **RabbitMQ**  
  Message broker untuk antrian email
- **MSSQL**  
  Database utama aplikasi

---

## Fitur Utama
- Registrasi pengguna
- Verifikasi email via RabbitMQ
- Login (diblokir jika email belum diverifikasi)
- Update profil & upload foto
- Integrasi API eksternal desa (get_token → get_desa)
- Seluruh service berjalan dalam Docker

---

## Alur Kerja
1. User melakukan registrasi  
2. Sistem membuat token verifikasi email  
3. Token dikirim ke RabbitMQ (`email_queue`)  
4. EmailWorker mengirim email verifikasi  
5. User klik link verifikasi  
6. Status email diverifikasi  
7. User dapat login dan mengelola profil  

---

## Integrasi API Desa
- **Get Token**
```http
GET http://api.inixindojogja.com/index.php/svc/get_token
```
---
- **Get Desa**
```http
GET http://api.inixindojogja.com/index.php/svc/get_desa
```
Authorization: Bearer {token}
Data desa ditampilkan dalam bentuk dropdown pada halaman registrasi.
---
- **Menjalankan EmailWorker**
```bash
docker build -t emailworker:latest -f EmailWorker/Dockerfile .
kubectl apply -f k8s/emailworker-depl.yaml 
```
---
- **Menjalankan FinalProject**
```bash
docker build -t finalproject:latest -f FinalProject/Dockerfile .                       
kubectl apply -f k8s/finalproject-depl.yaml
```
---
- **Akses aplikasi:**
```text
http://localhost:5000
```
---
- **Catatan**
1. User tidak dapat login sebelum email diverifikasi

2. Email dikirim menggunakan RabbitMQ + EmailWorker

3. Data desa berasal dari API eksternal

4. Semua service berjalan menggunakan Docker
---
## Penutup
Project ini dibuat dengan bantuan AI untuk memenuhi Mini Project Blazor .NET dengan Microservice & Docker dan telah mengimplementasikan seluruh requirement yang diminta.
enjoy
