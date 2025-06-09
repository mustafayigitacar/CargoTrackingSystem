namespace KargoTakip.Models
{
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
}