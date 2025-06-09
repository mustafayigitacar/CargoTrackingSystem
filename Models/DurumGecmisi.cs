using System;
using KargoTakip.Enums;

namespace KargoTakip.Models
{
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
}