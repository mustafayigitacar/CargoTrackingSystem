using System;
using System.Threading;
using KargoTakip.Services;
using KargoTakip.Enums;

namespace KargoTakip.UI
{
    public class Program
    {
        public static void Main()
        {
            var kargoSistemi = new KargoService();

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
                    Console.WriteLine("6. Yeni Gönderi Oluştur ");
                    Console.WriteLine("7. Kayıtlı Adresleri Listele");
                    Console.WriteLine("8. Yeni Adres Ekle");
                    Console.WriteLine("0. Çıkış");
                    Console.Write("\nSeçiminiz: ");

                    string secim = Console.ReadLine() ?? string.Empty;

                    switch (secim)
                    {
                        case "1":
                            Console.Write("Takip numarasını girin (menüye dönmek için 'menu' yazın): ");
                            string takipNo = Console.ReadLine();
                            if (takipNo != null && takipNo.Trim().ToLower() == "menu") break;
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
                            Console.Write("Takip numarasını girin (menüye dönmek için 'menu' yazın): ");
                            string guncellenecekTakipNo = Console.ReadLine();
                            if (guncellenecekTakipNo != null && guncellenecekTakipNo.Trim().ToLower() == "menu") break;
                            if (string.IsNullOrWhiteSpace(guncellenecekTakipNo))
                            {
                                Console.WriteLine("HATA: Geçerli bir takip numarası giriniz!");
                                break;
                            }
                            var durumlar = Enum.GetValues<GonderiDurumu>();
                            var kargoServiceType = kargoSistemi.GetType();
                            var gonderilerField = kargoServiceType.GetField("_gonderiler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            var gonderilerDict = gonderilerField?.GetValue(kargoSistemi) as System.Collections.IDictionary;
                            if (gonderilerDict == null || !gonderilerDict.Contains(guncellenecekTakipNo.Trim()))
                            {
                                Console.WriteLine("");
                                Console.WriteLine($"HATA: Takip numarası '{guncellenecekTakipNo}' bulunamadı!");
                                break;
                            }
                            Console.WriteLine("\nDurum seçenekleri:");
                            for (int i = 0; i < durumlar.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {durumlar[i]}");
                            }
                            Console.Write("Durum seçin (1-7, menüye dönmek için 'menu' yazın): ");
                            string durumSecim = Console.ReadLine();
                            if (durumSecim != null && durumSecim.Trim().ToLower() == "menu") break;
                            if (int.TryParse(durumSecim, out int durumIndex) &&
                                durumIndex > 0 && durumIndex <= durumlar.Length)
                            {
                                Console.Write("Açıklama (opsiyonel, menüye dönmek için 'menu' yazın): ");
                                string aciklama = Console.ReadLine();
                                if (aciklama != null && aciklama.Trim().ToLower() == "menu") break;
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
                            Console.Write("Şehir adını girin (menüye dönmek için 'menu' yazın): ");
                            string sehir = Console.ReadLine();
                            if (sehir != null && sehir.Trim().ToLower() == "menu") break;
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
                        Thread.Sleep(444);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}");
                    Console.WriteLine("Program devam ediyor...");
                    Thread.Sleep(444);
                }
            }
        }
    }
}