using System;
using System.Collections.Generic;
using System.Linq;
using KargoTakip.Enums;

namespace KargoTakip.Models
{
    public class Gonderi
    {
        public string TakipNumarasi { get; private set; }
        public Adres GonderenAdres { get; set; }
        public Adres AliciAdres { get; set; }
        public string Icerik { get; set; }
        public double Agirlik { get; set; }
        public decimal Ucret { get; set; }
        public DateTime OlusturmaTarihi { get; private set; }
        public GonderiDurumu Durum { get; set; }
        public GonderiTuru Tur { get; set; }

        private List<DurumGecmisi> _durumGecmisi;

        public Gonderi(Adres gonderenAdres, Adres aliciAdres, string icerik, double agirlik, GonderiTuru tur)
        {
            if (gonderenAdres == null) throw new ArgumentNullException(nameof(gonderenAdres));
            if (aliciAdres == null) throw new ArgumentNullException(nameof(aliciAdres));
            if (agirlik <= 0) throw new ArgumentException("Ağırlık pozitif olmalıdır.", nameof(agirlik));

            TakipNumarasi = TakipNumarasiOlustur(tur);
            GonderenAdres = gonderenAdres;
            AliciAdres = aliciAdres;
            Icerik = icerik ?? "Belirtilmemiş";
            Agirlik = agirlik;
            Tur = tur;
            OlusturmaTarihi = DateTime.Now;
            Durum = GonderiDurumu.Bekliyor;
            _durumGecmisi = new List<DurumGecmisi>();
            Ucret = UcretHesapla();

            _durumGecmisi.Add(new DurumGecmisi(GonderiDurumu.Bekliyor, "Gönderi oluşturuldu"));
            Console.WriteLine("\n" + "".PadRight(60, '.'));
        }

        private string TakipNumarasiOlustur(GonderiTuru tur)
        {
            return tur == GonderiTuru.Yurtici ? TakipNumarasiOlusturYurtIci() : TakipNumarasiOlusturYurtDisi();
        }

        private string TakipNumarasiOlusturYurtIci()
        {
            string guidParca = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            return $"TR{DateTime.Now:yyyyMMdd}{guidParca}";
        }

        private string TakipNumarasiOlusturYurtDisi()
        {
            string guidParca = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            return $"NONETR{DateTime.Now:yyyyMMdd}{guidParca}";
        }

        private decimal UcretHesapla()
        {
            decimal baslangicUcreti = Tur == GonderiTuru.Yurtici ? 15.00m : 35.00m;
            decimal agirlikUcreti = Tur == GonderiTuru.Yurtici ?
                (decimal)Agirlik * 2.50m : (decimal)Agirlik * 8.00m;

            return baslangicUcreti + agirlikUcreti;
        }

        public void DurumGuncelle(GonderiDurumu yeniDurum, string aciklama = "")
        {
            Durum = yeniDurum;
            _durumGecmisi.Add(new DurumGecmisi(yeniDurum, aciklama ?? string.Empty));
            Console.WriteLine($"[{TakipNumarasi}] Durum güncellendi: {GetDurumAciklama(yeniDurum)}");
        }

        public List<DurumGecmisi> DurumGecmisiniGetir()
        {
            return _durumGecmisi.OrderBy(d => d.Tarih).ToList();
        }

        public string GetDetaylar()
        {
            return $"Takip No: {TakipNumarasi}\n" +
                   $"Tür: {Tur}\n" +
                   $"Durum: {GetDurumAciklama(Durum)}\n" +
                   $"İçerik: {Icerik}\n" +
                   $"Ağırlık: {Agirlik} kg\n" +
                   $"Ücret: {Ucret:F2} TL\n" +
                   $"Oluşturma: {OlusturmaTarihi:dd.MM.yyyy HH:mm}\n" +
                   $"Gönderen: {GonderenAdres}\n" +
                   $"Alıcı: {AliciAdres}";
        }

        private string GetDurumAciklama(GonderiDurumu durum)
        {
            return durum switch
            {
                GonderiDurumu.Bekliyor => "Beklemede",
                GonderiDurumu.Hazirlaniyor => "Hazırlanıyor",
                GonderiDurumu.Yolda => "Yolda",
                GonderiDurumu.DagitimMerkezinde => "Dağıtım Merkezinde",
                GonderiDurumu.TeslimEdildi => "Teslim Edildi",
                GonderiDurumu.TeslimAlinamadi => "Teslim Alınamadı",
                GonderiDurumu.Iade => "İade Edildi",
                _ => "Bilinmeyen Durum"
            };
        }
    }
}