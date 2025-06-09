using System.Collections.Generic;
using KargoTakip.Models;
using KargoTakip.Enums;

namespace KargoTakip.Services
{
    public interface IKargoService
    {
        string YeniGonderiOlustur(GonderiTuru tur, Adres gonderenAdres, Adres aliciAdres, string icerik, double agirlik);
        bool DurumGuncelle(string takipNumarasi, GonderiDurumu yeniDurum, string aciklama = "");
        void GonderiTakipEt(string takipNumarasi);
        void TumGonderileriListele();
        void IstatistikleriGoster();
        void SehireGoreGonderileriListele(string sehir);
        Adres YeniAdresEkle();
        void KayitliAdresleriListele();
        Adres AdresSecimYap(string adresTipi);
        string DetayliGonderiOlustur();
    }
}