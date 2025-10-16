using System.ComponentModel.DataAnnotations;

namespace AccountingOnline.Application.Features.Partners.DTOs;

public class PartnerDto
{
    public int IDPartner { get; set; }
    public string SifraPartner { get; set; } = string.Empty;
    public string NazivPartnera { get; set; } = string.Empty;
    public string? Adresa { get; set; }
    public int IDMesto { get; set; }
    public string? NazivMesta { get; set; }
    public string PIB { get; set; } = string.Empty;
    public string? Telefon { get; set; }
    public string? FAX { get; set; }
    public int? IDReferent { get; set; }
    public string? NazivReferenta { get; set; }
    public string? Napomena { get; set; }
    public string? Kontakt { get; set; }
    public int IDStatus { get; set; }
    public string? NazivStatusa { get; set; }
    public int? IDDrzava { get; set; }
    public string? NazivDrzave { get; set; }
    public float Rabat { get; set; }
    public float Kasa { get; set; }
    public int? IDNacinPlacanja { get; set; }
    public string? NazivNacinaPlacanja { get; set; }
    public short? IDCenovnaGrupa { get; set; }
    public string? Konto { get; set; }
    public int? IDPartnerGlavni { get; set; }
    public string? NazivPartneraGlavnog { get; set; }
    public string? PDVBroj { get; set; }
    public string? MaticniBroj { get; set; }
    public string? SifraSort { get; set; }
    public int IDVrstaPartnera { get; set; }
    public string? NazivVrstePartnera { get; set; }
    public int? Proizvodjac { get; set; }
    public string? BrojUgovora { get; set; }
    public DateTime? DatumUgovora { get; set; }
    public decimal Kredit { get; set; }
    public DateTime? DatumOtvaranja { get; set; }
    public string? NjihovaSifraZaNas { get; set; }
    public int? BezZabrane { get; set; }
    public int? TolerancijaValute { get; set; }
    public bool? OdlozenoPlacanje { get; set; }
    public string? KategorijaKupca { get; set; }
    public string? StaraSifra { get; set; }
}

public class PartnerCreateDto
{
    [Required(ErrorMessage = "Šifra partnera je obavezna")]
    [StringLength(20, ErrorMessage = "Šifra partnera ne može biti duža od 20 karaktera")]
    public string SifraPartner { get; set; } = string.Empty;

    [Required(ErrorMessage = "Naziv partnera je obavezan")]
    [StringLength(100, ErrorMessage = "Naziv partnera ne može biti duži od 100 karaktera")]
    public string NazivPartnera { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Adresa ne može biti duža od 100 karaktera")]
    public string? Adresa { get; set; }

    [Required(ErrorMessage = "Mesto je obavezno")]
    public int IDMesto { get; set; }

    [Required(ErrorMessage = "PIB je obavezan")]
    [StringLength(20, ErrorMessage = "PIB ne može biti duži od 20 karaktera")]
    public string PIB { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Telefon ne može biti duži od 20 karaktera")]
    public string? Telefon { get; set; }

    [StringLength(20, ErrorMessage = "FAX ne može biti duži od 20 karaktera")]
    public string? FAX { get; set; }

    public int? IDReferent { get; set; }

    [StringLength(255, ErrorMessage = "Napomena ne može biti duža od 255 karaktera")]
    public string? Napomena { get; set; }

    [StringLength(50, ErrorMessage = "Kontakt ne može biti duži od 50 karaktera")]
    public string? Kontakt { get; set; }

    public int IDStatus { get; set; } = 1;
    public int? IDDrzava { get; set; }
    
    [Range(0, 100, ErrorMessage = "Rabat mora biti između 0 i 100%")]
    public float Rabat { get; set; } = 0;
    
    public float Kasa { get; set; } = 0;
    public int? IDNacinPlacanja { get; set; }
    public short? IDCenovnaGrupa { get; set; }

    [StringLength(10, ErrorMessage = "Konto ne može biti duži od 10 karaktera")]
    public string? Konto { get; set; }

    public int? IDPartnerGlavni { get; set; }

    [StringLength(20, ErrorMessage = "PDV broj ne može biti duži od 20 karaktera")]
    public string? PDVBroj { get; set; }

    [StringLength(20, ErrorMessage = "Matični broj ne može biti duži od 20 karaktera")]
    public string? MaticniBroj { get; set; }

    [StringLength(20, ErrorMessage = "Šifra sort ne može biti duža od 20 karaktera")]
    public string? SifraSort { get; set; }

    [Required(ErrorMessage = "Vrsta partnera je obavezna")]
    public int IDVrstaPartnera { get; set; }

    public int? Proizvodjac { get; set; }

    [StringLength(50, ErrorMessage = "Broj ugovora ne može biti duži od 50 karaktera")]
    public string? BrojUgovora { get; set; }

    public DateTime? DatumUgovora { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Kredit ne može biti negativan")]
    public decimal Kredit { get; set; } = 0;
    
    public DateTime? DatumOtvaranja { get; set; }

    [StringLength(50, ErrorMessage = "Njihova šifra za nas ne može biti duža od 50 karaktera")]
    public string? NjihovaSifraZaNas { get; set; }

    public int? BezZabrane { get; set; } = 0;
    public int? TolerancijaValute { get; set; }
    public bool? OdlozenoPlacanje { get; set; }

    [StringLength(20, ErrorMessage = "Kategorija kupca ne može biti duža od 20 karaktera")]
    public string? KategorijaKupca { get; set; }

    [StringLength(20, ErrorMessage = "Stara šifra ne može biti duža od 20 karaktera")]
    public string? StaraSifra { get; set; }
}

public class PartnerUpdateDto : PartnerCreateDto
{
    public int IDPartner { get; set; }
}

public class PartnerComboDto
{
    public int IDPartner { get; set; }
    public string SifraPartner { get; set; } = string.Empty;
    public string NazivPartnera { get; set; } = string.Empty;
    public string? NazivMesta { get; set; }
    public int IDStatus { get; set; }
    public string? NazivStatusa { get; set; }
}