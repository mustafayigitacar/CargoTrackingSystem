using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using KargoTakip.Models;
using KargoTakip.Enums;

namespace KargoTakip.Services
{
    public class KargoService : IKargoService
    {
        private Dictionary<string, Gonderi> _gonderiler;
        private List<Adres> _kayitliAdresler;
        private int _gonderiSayisi;

        public KargoService()
        {
            _gonderiler = new Dictionary<string, Gonderi>();
            _kayitliAdresler = new List<Adres>();
            _gonderiSayisi = 0;

            SubeAdresleriEkle();
        }

        private void SubeAdresleriEkle()
        {
            _kayitliAdresler.Add(new Adres("Trabzon Şube", "", "0461 461-61-61",
                "Atatürk Meydanı No:61 ", "Trabzon", "Türkiye"));
            _kayitliAdresler.Add(new Adres("İstanbul Şube", "", "0461 461-00-00",
                "istiklal Caddesi No:34", "İstanbul", "Türkiye"));
            _kayitliAdresler.Add(new Adres("Ankara Şube", "", "0461 461-06-06",
                "Kızılay Meydan No:06", "Ankara", "Türkiye"));
            _kayitliAdresler.Add(new Adres("İzmir Şube", "", "0461 461-35-35",
                "Barbaros Mah. İnını Cad. No:78", "İzmir", "Türkiye"));
            _kayitliAdresler.Add(new Adres("Brooklyn Şube", "", "+1 555-123-4567",
                "Grand Army Plaza, Brooklyn, NY 11238", "Brooklyn", "Amerika Birleşik Devletleri"));
            _kayitliAdresler.Add(new Adres("Berlin Şube", "", "+49 30-12345678",
                "Alexanderplatz, 10178 Berlin", "Berlin", "Almanya"));
        }

        public Adres YeniAdresEkle()
        {
            Console.WriteLine("\nYENİ ADRES EKLEME");
            Console.WriteLine("=".PadRight(30, '='));

            Console.Write("Ad (menüye dönmek için 'menu' yazın): ");
            string ad = Console.ReadLine() ?? string.Empty;
            if (ad.Trim().ToLower() == "menu") return null;
            Console.Write("Adres Detayı (menüye dönmek için 'menu' yazın): ");
            string adresDetay = Console.ReadLine() ?? string.Empty;
            if (adresDetay.Trim().ToLower() == "menu") return null;
            Console.Write("Şehir (menüye dönmek için 'menu' yazın): ");
            string sehir = Console.ReadLine() ?? string.Empty;
            if (sehir.Trim().ToLower() == "menu") return null;
            Console.Write("Ülke (menüye dönmek için 'menu' yazın): ");
            string ulke = Console.ReadLine() ?? string.Empty;
            if (ulke.Trim().ToLower() == "menu") return null;
            Console.Write("Telefon (menüye dönmek için 'menu' yazın): ");
            string telefon = Console.ReadLine() ?? string.Empty;
            if (telefon.Trim().ToLower() == "menu") return null;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(sehir))
            {
                Console.WriteLine("HATA: Ad ve Şehir alanları zorunludur!");
                return null;
            }

            var yeniAdres = new Adres(ad, "", telefon, adresDetay, sehir, ulke);
            _kayitliAdresler.Add(yeniAdres);

            Console.WriteLine($"\nAdres başarıyla eklendi: {yeniAdres}");
            return yeniAdres;
        }

        public void KayitliAdresleriListele()
        {
            if (_kayitliAdresler.Count == 0)
            {
                Console.WriteLine("Kayıtlı adres bulunmamaktadır.");
                return;
            }

            Console.WriteLine($"\nKAYITLI ADRESLER ({_kayitliAdresler.Count} adet)");
            Console.WriteLine("=".PadRight(80, '='));
            foreach (var adres in _kayitliAdresler)
            {
                Console.WriteLine(adres);
            }
            Console.WriteLine("=".PadRight(80, '='));
        }

        public Adres AdresSecimYap(string adresTipi)
        {
            while (true)
            {
                Console.WriteLine($"\n{adresTipi} ADRESİ SEÇİMİ");
                Console.WriteLine("=".PadRight(40, '='));
                Console.WriteLine("1. Kayıtlı adreslerden seç");
                Console.WriteLine("2. Yeni adres ekle");
                Console.Write("Seçiminiz (1-2, menüye dönmek için 'menu' yazın): ");
                string secim = Console.ReadLine() ?? string.Empty;
                if (secim.Trim().ToLower() == "menu") return null;
                switch (secim)
                {
                    case "1":
                        return KayitliAdresSecimYap();
                    case "2":
                        var yeniAdres = YeniAdresEkle();
                        if (yeniAdres == null)
                        {
                            Console.WriteLine("Adres eklenemedi! Tekrar deneyiniz.");
                            continue;
                        }
                        return yeniAdres;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen 1 veya 2 seçiniz.\n");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        private Adres KayitliAdresSecimYap()
        {
            while (true)
            {
                if (_kayitliAdresler.Count == 0)
                {
                    Console.WriteLine("Kayıtlı adres yok. Yeni adres eklemeniz gerekiyor.");
                    return YeniAdresEkle();
                }
                KayitliAdresleriListele();
                Console.Write($"Adres seçin (1-{_kayitliAdresler.Count}, menüye dönmek için 'menu' yazın): ");
                string secimStr = Console.ReadLine();
                if (secimStr != null && secimStr.Trim().ToLower() == "menu") return null;
                if (int.TryParse(secimStr, out int secim) && secim > 0 && secim <= _kayitliAdresler.Count)
                {
                    return _kayitliAdresler[secim - 1];
                }
                else
                {
                    Console.WriteLine("Geçersiz seçim! Lütfen geçerli bir numara giriniz.\n");
                    Thread.Sleep(1000);
                }
            }
        }

        private GonderiTuru GonderiTuruSec()
        {
            while (true)
            {
                Console.WriteLine("Gönderi Türü:");
                Console.WriteLine("1. Yurtiçi");
                Console.WriteLine("2. Yurtdışı");
                Console.Write("Seçiminiz (1-2, menüye dönmek için 'menu' yazın): ");
                string input = Console.ReadLine();
                if (input != null && input.Trim().ToLower() == "menu") return GonderiTuru.Yurtici;
                if (int.TryParse(input, out int turSecim) && (turSecim == 1 || turSecim == 2))
                {
                    return turSecim == 1 ? GonderiTuru.Yurtici : GonderiTuru.Yurtdisi;
                }
                Console.WriteLine("Geçersiz seçim! Lütfen 1 veya 2 seçiniz.\n");
                Thread.Sleep(1000);
            }
        }

        private double AgirlikAl()
        {
            while (true)
            {
                Console.Write("Ağırlık (kg, menüye dönmek için 'menu' yazın): ");
                string input = Console.ReadLine();
                if (input != null && input.Trim().ToLower() == "menu") return -1;
                if (double.TryParse(input, out double agirlik) && agirlik > 0)
                {
                    return agirlik;
                }
                Console.WriteLine("Geçersiz ağırlık! Lütfen geçerli bir ağırlık giriniz.\n");
                Thread.Sleep(1000);
            }
        }

        public string DetayliGonderiOlustur()
        {
            Console.WriteLine("\nYENİ GÖNDERİ OLUŞTURMA");
            Console.WriteLine("=".PadRight(40, '='));
            var tur = GonderiTuruSec();
            if (tur != GonderiTuru.Yurtici && tur != GonderiTuru.Yurtdisi) return null;
            var gonderenAdres = AdresSecimYap("GÖNDEREN");
            if (gonderenAdres == null) return null;
            Adres aliciAdres;
            while (true)
            {
                aliciAdres = AdresSecimYap("ALICI");
                if (aliciAdres == null) return null;
                if (tur == GonderiTuru.Yurtdisi && aliciAdres.Ulke.Trim().ToLower() == "türkiye")
                {
                    Console.WriteLine("UYARI: Yurtdışı gönderiler Türkiye adresine yapılamaz!");
                    Console.WriteLine("Menüye dönmek için 1'e, yeni adres seçmek için 0'a basın.");
                    string secim = Console.ReadLine()?.Trim();
                    if (secim == "1")
                    {
                        return null;
                    }
                    else if (secim == "0")
                    {
                        Console.WriteLine("Lütfen yeni bir alıcı adresi seçiniz.");
                        continue;
                    }
                }
                if (AynıAdresMi(gonderenAdres, aliciAdres))
                {
                    Console.WriteLine("\n  UYARI: Gönderen ve alıcı adresi aynı!");
                    Console.WriteLine($"Seçilen Adres: {aliciAdres}");
                    if (DevamEtmekIstiyor())
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Alıcı adresini yeniden seçiniz.\n");
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
            Console.Write("\nGönderi içeriği (menüye dönmek için 'menu' yazın): ");
            string icerik = Console.ReadLine();
            if (icerik != null && icerik.Trim().ToLower() == "menu") return null;
            if (string.IsNullOrEmpty(icerik))
                icerik = "Belirtilmemiş";
            var agirlik = AgirlikAl();
            if (agirlik == -1) return null;
            try
            {
                var takipNo = YeniGonderiOlustur(tur, gonderenAdres, aliciAdres, icerik, agirlik);
                Console.WriteLine("\nGönderi başarıyla oluşturuldu!");
                Console.WriteLine($"Takip Numarası: {takipNo}");
                return takipNo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: Gönderi oluşturulamadı - {ex.Message}");
                return null;
            }
        }

        private bool AynıAdresMi(Adres adres1, Adres adres2)
        {
            if (adres1 == null || adres2 == null)
                return false;
            if (ReferenceEquals(adres1, adres2))
                return true;
            return adres1.Ad.Equals(adres2.Ad, StringComparison.OrdinalIgnoreCase) &&
                   adres1.Soyad.Equals(adres2.Soyad, StringComparison.OrdinalIgnoreCase) &&
                   adres1.Telefon.Equals(adres2.Telefon, StringComparison.OrdinalIgnoreCase) &&
                   adres1.AdresDetay.Equals(adres2.AdresDetay, StringComparison.OrdinalIgnoreCase) &&
                   adres1.Sehir.Equals(adres2.Sehir, StringComparison.OrdinalIgnoreCase) &&
                   adres1.Ulke.Equals(adres2.Ulke, StringComparison.OrdinalIgnoreCase);
        }

        private bool DevamEtmekIstiyor()
        {
            while (true)
            {
                Console.Write("Bu adresle devam etmek istiyor musunuz? (E/H): ");
                string cevap = Console.ReadLine()?.Trim().ToUpper();
                switch (cevap)
                {
                    case "E":
                    case "EVET":
                        return true;
                    case "H":
                    case "HAYIR":
                        return false;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen 'E' (Evet) veya 'H' (Hayır) giriniz.\n");
                        break;
                }
            }
        }

        public string YeniGonderiOlustur(GonderiTuru tur, Adres gonderenAdres, Adres aliciAdres,
                                       string icerik, double agirlik)
        {
            var gonderi = new Gonderi(gonderenAdres, aliciAdres, icerik, agirlik, tur);
            _gonderiler[gonderi.TakipNumarasi] = gonderi;
            _gonderiSayisi++;
            Console.WriteLine($"Yeni gönderi oluşturuldu: {gonderi.TakipNumarasi}");
            Console.WriteLine($"Tür: {tur}, Ağırlık: {agirlik} kg, Ücret: {gonderi.Ucret:F2} TL");
            return gonderi.TakipNumarasi;
        }

        public bool DurumGuncelle(string takipNumarasi, GonderiDurumu yeniDurum, string aciklama = "")
        {
            if (string.IsNullOrWhiteSpace(takipNumarasi))
            {
                Console.WriteLine("HATA: Takip numarası boş olamaz!");
                return false;
            }
            if (!_gonderiler.TryGetValue(takipNumarasi, out Gonderi gonderi))
            {
                Console.WriteLine($"HATA: Takip numarası '{takipNumarasi}' bulunamadı!");
                return false;
            }
            gonderi.DurumGuncelle(yeniDurum, aciklama ?? string.Empty);
            Console.WriteLine("Durum başarıyla güncellendi!");
            return true;
        }
        public void GonderiTakipEt(string takipNumarasi)
        {
            if (string.IsNullOrWhiteSpace(takipNumarasi))
            {
                Console.WriteLine("HATA: Takip numarası boş olamaz!");
                return;
            }
            if (_gonderiler.TryGetValue(takipNumarasi, out Gonderi gonderi))
            {
                Console.WriteLine("\n" + "=".PadRight(60, '='));
                Console.WriteLine("GÖNDERİ DETAYLARI");
                Console.WriteLine("=".PadRight(60, '='));
                Console.WriteLine(gonderi.GetDetaylar());
                Console.WriteLine("\nDURUM GEÇMİŞİ:");
                Console.WriteLine("-".PadRight(60, '-'));
                var gecmis = gonderi.DurumGecmisiniGetir();
                foreach (var durum in gecmis)
                {
                    Console.WriteLine($"  {durum}");
                }
                Console.WriteLine("=".PadRight(60, '='));
            }
            else
            {
                Console.WriteLine($"HATA: Takip numarası '{takipNumarasi}' bulunamadı!");
            }
        }
        public void TumGonderileriListele()
        {
            if (_gonderiler.Count == 0)
            {
                Console.WriteLine("Henüz gönderi bulunmamaktadır.");
                Console.WriteLine("-".PadRight(60, '-'));
                Thread.Sleep(1000);
                return;
            }
            Console.WriteLine($"\nTÜM GÖNDERİLER ({_gonderiler.Count} adet)");
            Console.WriteLine("=".PadRight(80, '='));
            foreach (var gonderi in _gonderiler.Values.OrderBy(g => g.OlusturmaTarihi))
            {
                Console.WriteLine($"{gonderi.TakipNumarasi} | {gonderi.Tur} | " +
                                $"{gonderi.AliciAdres.Sehir} | {gonderi.Durum} | {gonderi.Ucret:F2} TL");
            }
            Console.WriteLine("=".PadRight(80, '='));
        }
        public void IstatistikleriGoster()
        {
            var istatistikler = new Dictionary<GonderiDurumu, int>();
            foreach (GonderiDurumu durum in Enum.GetValues<GonderiDurumu>())
            {
                istatistikler[durum] = _gonderiler.Values.Count(g => g.Durum == durum);
            }
            Console.WriteLine("\nDURUM İSTATİSTİKLERİ");
            Console.WriteLine("=".PadRight(40, '='));
            foreach (var kvp in istatistikler.Where(x => x.Value > 0))
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value} adet");
            }
            Console.WriteLine($"\nToplam Gönderi: {_gonderiSayisi} adet");
            Console.WriteLine($"Kayıtlı Adres: {_kayitliAdresler.Count} adet");
            Console.WriteLine("=".PadRight(40, '='));
        }
        public void SehireGoreGonderileriListele(string sehir)
        {
            if (string.IsNullOrWhiteSpace(sehir))
            {
                Console.WriteLine("HATA: Şehir adı boş olamaz!");
                return;
            }
            var sehirGonderileri = _gonderiler.Values
                .Where(g => g.AliciAdres.Sehir.ToLower().Contains(sehir.ToLower()))
                .OrderBy(g => g.OlusturmaTarihi)
                .ToList();
            if (sehirGonderileri.Count == 0)
            {
                Console.WriteLine($"{sehir} Şehrine gönderilecek kargo bulunamadı.");
                return;
            }
            Console.WriteLine($"\n{sehir.ToUpper()} ŞEHRİNE GÖNDERİLER ({sehirGonderileri.Count} adet)");
            Console.WriteLine("=".PadRight(70, '='));
            foreach (var gonderi in sehirGonderileri)
            {
                Console.WriteLine($"{gonderi.TakipNumarasi} | {gonderi.AliciAdres.Ad} {gonderi.AliciAdres.Soyad} | " +
                                $"{gonderi.Durum} | {gonderi.Ucret:F2} TL");
            }
            Console.WriteLine("=".PadRight(70, '='));
        }
    }
}