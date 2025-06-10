# Kargo Takip Sistemi

Bu proje, bir kargo şirketinin gönderilerini takip etmek için geliştirilmiş bir konsol uygulamasıdır.

## Özellikler

- Yurtiçi ve yurtdışı gönderi oluşturma
- Gönderi durumu takibi ve güncelleme
- Detaylı gönderi bilgileri görüntüleme
- Şehir bazlı gönderi listeleme
- Adres yönetimi
- İstatistik raporlama

## Proje Yapısı

```
src/
├── Enums/
│   ├── GonderiDurumu.cs
│   └── GonderiTuru.cs
├── Models/
│   ├── Adres.cs
│   ├── DurumGecmisi.cs
│   └── Gonderi.cs
├── Services/
│   ├── IKargoService.cs
│   └── KargoService.cs
└── UI/
    └── Program.cs
```

## Kurulum

1. Projeyi klonlayın
2. Visual Studio veya tercih ettiğiniz IDE ile açın
3. Projeyi derleyin ve çalıştırın

## Kullanım

Program başlatıldığında ana menü üzerinden aşağıdaki işlemler yapılabilir:

1. Gönderi Takip Et
2. Durum Güncelle
3. Tüm Gönderileri Listele
4. İstatistikleri Göster
5. Şehire Göre Gönderileri Listele
6. Yeni Gönderi Oluştur
7. Kayıtlı Adresleri Listele
8. Yeni Adres Ekle

## Geliştirme

Proje, SOLID prensiplerine uygun olarak geliştirilmiştir ve aşağıdaki katmanlardan oluşur:

- **Models**: Veri modellerini içerir
- **Enums**: Enum tanımlamalarını içerir
- **Services**: İş mantığı katmanını içerir
- **UI**: Kullanıcı arayüzü kodlarını içerir


## Gönderileri Oluşturma

![Image](https://github.com/user-attachments/assets/42b995b8-9006-4690-8ba3-2fd3141f4ead)

## Durum Güncelleme ve Gönderi Takibi

![Image](https://github.com/user-attachments/assets/709bc2ad-6708-4a47-bb76-e2c9cc7cb9a0)

## Genel Kargo İstatistikleri 

![Image](https://github.com/user-attachments/assets/f054d7a0-0f83-4d57-823e-80a5e747f1fb)

## Yeni Gönderi Oluşturma 

![Image](https://github.com/user-attachments/assets/c6b24a19-c574-48d0-b4d7-31d8b3b61cfc)

## Yeni Adres Ekleme 

![Image](https://github.com/user-attachments/assets/88966caa-c58b-4a38-b6fa-6ad8ea5d4e72)

## Tek Parça Çalışan Örnek

Tek parça çalışan örneği Youtube da izlemek isterseniz [BURAYA](https://youtu.be/0CS8A70m1Go?si=UT9iCwSjC_LCeILB) tıklayabilirsiniz.


## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. 
