using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public enum GonderiDurumu
{
    Bekliyor,
    Hazirlaniyor,
    Yolda,
    DagitimMerkezinde,
    TeslimEdildi,
    TeslimAlinamadi,
    Iade
}

public enum GonderiTuru
{
    Yurtici,
    Yurtdisi
}

public class DurumGecmisi
{
    public DateTime Tarih { get; set; }
    public GonderiDurumu Durum { get; set; }
    public string Aciklama { get; set; }

    public DurumGecmisi(GonderiDurumu durum, string aciklama = "")
    {
        Tarih = DateTime.Now;
        Durum = durum;
        Aciklama = aciklama ?? string.Empty;
    }

    public override string ToString()
    {
        return $"{Tarih:dd.MM.yyyy HH:mm} - {GetDurumAciklama()} {(string.IsNullOrEmpty(Aciklama) ? "" : $"({Aciklama})")}";
    }

    private string GetDurumAciklama()
    {
        return Durum switch
        {
            GonderiDurumu.Bekliyor => "Gönderi beklemede",
            GonderiDurumu.Hazirlaniyor => "Gönderi hazırlanıyor",
            GonderiDurumu.Yolda => "Gönderi yolda",
            GonderiDurumu.DagitimMerkezinde => "Dağıtım merkezinde",
            GonderiDurumu.TeslimEdildi => "Teslim edildi",
            GonderiDurumu.TeslimAlinamadi => "Teslim alınamadı",
            GonderiDurumu.Iade => "İade edildi",
            _ => "Bilinmeyen durum"
        };
    }
}

public class Adres
{
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Telefon { get; set; }
    public string AdresDetay { get; set; }
    public string Sehir { get; set; }
    public string Ulke { get; set; }

    public Adres(string ad, string soyad, string telefon, string adresDetay, string sehir, string ulke)
    {
        Ad = ad ?? string.Empty;
        Soyad = soyad ?? string.Empty;
        Telefon = telefon ?? string.Empty;
        AdresDetay = adresDetay ?? string.Empty;
        Sehir = sehir ?? string.Empty;
        Ulke = ulke ?? string.Empty;
    }

    public override string ToString()
    {
        return $"{Ad} {Soyad} - {Sehir}, {Ulke} - Tel: {Telefon}";
    }
}

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
        if (Durum != yeniDurum)
        {
            Durum = yeniDurum;
            _durumGecmisi.Add(new DurumGecmisi(yeniDurum, aciklama ?? string.Empty));
            Console.WriteLine($"[{TakipNumarasi}] Durum güncellendi: {GetDurumAciklama(yeniDurum)}");
        }
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

public class KargoTakipSistemi
{
    private Dictionary<string, Gonderi> _gonderiler;
    private List<Adres> _kayitliAdresler;
    private int _gonderiSayisi;

    public KargoTakipSistemi()
    {
        _gonderiler = new Dictionary<string, Gonderi>();
        _kayitliAdresler = new List<Adres>();
        _gonderiSayisi = 0;

        VarsayilanAdresleriEkle();
    }

    private void VarsayilanAdresleriEkle()
    {
        _kayitliAdresler.Add(new Adres("Trabzon Şube", "", "0461 461-61-61",
            "Atatürk Meydanı No:61 ", "Trabzon", "Türkiye"));
        _kayitliAdresler.Add(new Adres("İstanbul Şube", "", "0461 461-00-00",
            "istiklal Caddesi No:34", "İstanbul", "Türkiye"));
        _kayitliAdresler.Add(new Adres("Ankara Şube", "", "0461 461-06-06",
            "Kızılay Meydan No:06", "Ankara", "Türkiye"));
        _kayitliAdresler.Add(new Adres("İzmir Şube", "", "0461 461-35-35",
            "Barbaros Mah. İnönü Cad. No:78", "İzmir", "Türkiye"));
        _kayitliAdresler.Add(new Adres("Brooklyn Şube", "", "+1 555-123-4567",
            "Grand Army Plaza, Brooklyn, NY 11238", "Brooklyn", "Amerika Birleşik Devletleri"));
        _kayitliAdresler.Add(new Adres("Almanya Şube", "", "+49 30-12345678",
            "Alexanderplatz, 10178 Berlin", "Berlin", "Almanya"));
    }

    public Adres YeniAdresEkle()
    {
        Console.WriteLine("\nYENİ ADRES EKLEME");
        Console.WriteLine("=".PadRight(30, '='));

        Console.Write("Ad: ");
        string ad = Console.ReadLine() ?? string.Empty;

        Console.Write("Soyad: ");
        string soyad = Console.ReadLine() ?? string.Empty;

        Console.Write("Telefon: ");
        string telefon = Console.ReadLine() ?? string.Empty;

        Console.Write("Adres Detayı: ");
        string adresDetay = Console.ReadLine() ?? string.Empty;

        Console.Write("Şehir: ");
        string sehir = Console.ReadLine() ?? string.Empty;

        Console.Write("Ülke: ");
        string ulke = Console.ReadLine() ?? string.Empty;


        if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(sehir))
        {
            Console.WriteLine("HATA: Ad ve Şehir alanları zorunludur!");
            return null;
        }

        var yeniAdres = new Adres(ad, soyad, telefon, adresDetay, sehir, ulke);
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

        for (int i = 0; i < _kayitliAdresler.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_kayitliAdresler[i]}");
        }
        Console.WriteLine("=".PadRight(80, '='));
    }

    public Adres AdresSecimYap(string adresTipi)
    {
        Console.WriteLine($"\n{adresTipi} ADRESİ SEÇİMİ");
        Console.WriteLine("=".PadRight(40, '='));
        Console.WriteLine("1. Kayıtlı adreslerden seç");
        Console.WriteLine("2. Yeni adres ekle");
        Console.Write("Seçiminiz (1-2): ");

        string secim = Console.ReadLine() ?? string.Empty;

        switch (secim)
        {
            case "1":
                return KayitliAdresSecimYap();
            case "2":
                var yeniAdres = YeniAdresEkle();
                if (yeniAdres == null)
                {
                    Console.WriteLine("Adres eklenemedi! Kayıtlı adreslerden ilki seçilecek.");
                    return _kayitliAdresler.FirstOrDefault();
                }
                return yeniAdres;
            default:
                Console.WriteLine("Geçersiz seçim! Kayıtlı adreslerden ilki seçilecek.");
                return _kayitliAdresler.FirstOrDefault();
        }
    }

    private Adres KayitliAdresSecimYap()
    {
        if (_kayitliAdresler.Count == 0)
        {
            Console.WriteLine("Kayıtlı adres yok. Yeni adres eklemeniz gerekiyor.");
            return YeniAdresEkle();
        }

        KayitliAdresleriListele();
        Console.Write($"Adres seçin (1-{_kayitliAdresler.Count}): ");

        if (int.TryParse(Console.ReadLine(), out int secim) &&
            secim > 0 && secim <= _kayitliAdresler.Count)
        {
            return _kayitliAdresler[secim - 1];
        }
        else
        {
            Console.WriteLine("Geçersiz seçim! İlk adres seçildi.");
            return _kayitliAdresler[0];
        }
    }

    public string DetayliGonderiOlustur()
    {
        Console.WriteLine("\nYENİ GÖNDERİ OLUŞTURMA");
        Console.WriteLine("=".PadRight(40, '='));

        Console.WriteLine("Gönderi Türü:");
        Console.WriteLine("1. Yurtiçi");
        Console.WriteLine("2. Yurtdışı");
        Console.Write("Seçiminiz (1-2): ");

        if (!int.TryParse(Console.ReadLine(), out int turSecim) || (turSecim != 1 && turSecim != 2))
        {
            Console.WriteLine("Geçersiz seçim! Yurtiçi seçildi.");
            turSecim = 1;
        }

        GonderiTuru tur = turSecim == 1 ? GonderiTuru.Yurtici : GonderiTuru.Yurtdisi;

        var gonderenAdres = AdresSecimYap("GÖNDEREN");
        if (gonderenAdres == null)
        {
            Console.WriteLine("HATA: Gönderen adres seçilemedi!");
            return null;
        }

        var aliciAdres = AdresSecimYap("ALICI");
        if (aliciAdres == null)
        {
            Console.WriteLine("HATA: Alıcı adres seçilemedi!");
            return null;
        }

        Console.Write("\nGönderi içeriği: ");
        string icerik = Console.ReadLine();
        if (string.IsNullOrEmpty(icerik))
            icerik = "Belirtilmemiş";

        Console.Write("Ağırlık (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double agirlik) || agirlik <= 0)
        {
            Console.WriteLine("Geçersiz ağırlık! 1 kg olarak ayarlandı.");
            agirlik = 1.0;
        }

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

        if (_gonderiler.TryGetValue(takipNumarasi, out Gonderi gonderi))
        {
            gonderi.DurumGuncelle(yeniDurum, aciklama ?? string.Empty);
            Console.WriteLine("Durum başarıyla güncellendi!");
            return true;
        }

        Console.WriteLine($"HATA: Takip numarası '{takipNumarasi}' bulunamadı!");
        return false;
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
            Console.WriteLine($"{sehir} şehrine gönderilecek kargo bulunamadı.");
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

public class Program
{
    public static void Main()
    {
        var kargoSistemi = new KargoTakipSistemi();

        Console.WriteLine("KARGO TAKİP SİSTEMİ");
        Console.WriteLine("=".PadRight(50, '='));

        bool devamEt = true;
        while (devamEt)
        {
            try
            {
                Console.WriteLine("\nANA MENÜ:");
                Console.WriteLine("1. Gönderi Takip Et");
                Console.WriteLine("2. Durum Güncelle");
                Console.WriteLine("3. Tüm Gönderileri Listele");
                Console.WriteLine("4. İstatistikleri Göster");
                Console.WriteLine("5. Şehire Göre Gönderileri Listele");
                Console.WriteLine("6. Yeni Gönderi Oluştur (Detaylı)");
                Console.WriteLine("7. Kayıtlı Adresleri Listele");
                Console.WriteLine("8. Yeni Adres Ekle");
                Console.WriteLine("0. Çıkış");
                Console.Write("\nSeçiminiz: ");

                string secim = Console.ReadLine() ?? string.Empty;

                switch (secim)
                {
                    case "1":
                        Console.Write("Takip numarasını girin: ");
                        string takipNo = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(takipNo))
                        {
                            kargoSistemi.GonderiTakipEt(takipNo.Trim());
                        }
                        else
                        {
                            Console.WriteLine("HATA: Geçerli bir takip numarası giriniz!");
                        }
                        break;

                    case "2":
                        Console.Write("Takip numarasını girin: ");
                        string guncellenecekTakipNo = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(guncellenecekTakipNo))
                        {
                            Console.WriteLine("HATA: Geçerli bir takip numarası giriniz!");
                            break;
                        }

                        Console.WriteLine("\nDurum seçenekleri:");
                        var durumlar = Enum.GetValues<GonderiDurumu>();
                        for (int i = 0; i < durumlar.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {durumlar[i]}");
                        }

                        Console.Write("Durum seçin (1-7): ");
                        if (int.TryParse(Console.ReadLine(), out int durumIndex) &&
                            durumIndex > 0 && durumIndex <= durumlar.Length)
                        {
                            Console.Write("Açıklama (opsiyonel): ");
                            string aciklama = Console.ReadLine();

                            bool basarili = kargoSistemi.DurumGuncelle(guncellenecekTakipNo.Trim(),
                                durumlar[durumIndex - 1], aciklama ?? string.Empty);

                            if (!basarili)
                            {
                                Console.WriteLine("-".PadRight(60, '-'));
                                Console.WriteLine("Durum güncelleme işlemi başarısız oldu!");
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Console.WriteLine("HATA: Geçersiz durum seçimi!");
                        }
                        break;

                    case "3":
                        kargoSistemi.TumGonderileriListele();
                        break;

                    case "4":
                        kargoSistemi.IstatistikleriGoster();
                        break;

                    case "5":
                        Console.Write("Şehir adını girin: ");
                        string sehir = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(sehir))
                        {
                            kargoSistemi.SehireGoreGonderileriListele(sehir.Trim());
                        }
                        else
                        {
                            Console.WriteLine("HATA: Geçerli bir şehir adı giriniz!");
                        }
                        break;

                    case "6":
                        kargoSistemi.DetayliGonderiOlustur();
                        break;

                    case "7":
                        kargoSistemi.KayitliAdresleriListele();
                        break;

                    case "8":
                        kargoSistemi.YeniAdresEkle();
                        break;

                    case "0":
                        devamEt = false;
                        Console.WriteLine("Kargo takip sistemi kapatılıyor...");
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                        break;
                }

                if (devamEt && secim != "0")
                {
                    Console.WriteLine("\nYükleniyor...");
                    Console.WriteLine("-".PadRight(60, '-'));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}");
                Console.WriteLine("Program devam ediyor...");
                Thread.Sleep(2000);
            }
        }
    }
}