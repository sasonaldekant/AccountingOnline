using AccountingOnline.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingOnline.Domain.Entities;

[Table("tblPartner")]
public class Partner : AuditableEntity
{
    [Key]
    [Column("IDPartner")]
    public int IDPartner { get; set; }

    [Required]
    [StringLength(20)]
    [Column("SifraPartner")]
    public string SifraPartner { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Column("NazivPartnera")]
    public string NazivPartnera { get; set; } = string.Empty;

    [StringLength(100)]
    [Column("Adresa")]
    public string? Adresa { get; set; }

    [Required]
    [Column("IDMesto")]
    public int IDMesto { get; set; }

    [Required]
    [StringLength(20)]
    [Column("PIB")]
    public string PIB { get; set; } = string.Empty;

    [StringLength(20)]
    [Column("Telefon")]
    public string? Telefon { get; set; }

    [StringLength(20)]
    [Column("FAX")]
    public string? FAX { get; set; }

    [Column("IDReferent")]
    public int? IDReferent { get; set; }

    [StringLength(255)]
    [Column("Napomena")]
    public string? Napomena { get; set; }

    [StringLength(50)]
    [Column("Kontakt")]
    public string? Kontakt { get; set; }

    [Required]
    [Column("IDStatus")]
    public int IDStatus { get; set; } = 1;

    [Column("IDDrzava")]
    public int? IDDrzava { get; set; }

    [Required]
    [Column("Rabat")]
    public float Rabat { get; set; } = 0;

    [Required]
    [Column("Kasa")]
    public float Kasa { get; set; } = 0;

    [Column("IDNacinPlacanja")]
    public int? IDNacinPlacanja { get; set; }

    [Column("IDCenovnaGrupa")]
    public short? IDCenovnaGrupa { get; set; }

    [StringLength(10)]
    [Column("Konto")]
    public string? Konto { get; set; }

    [Column("IDPartnerGlavni")]
    public int? IDPartnerGlavni { get; set; }

    [StringLength(20)]
    [Column("PDVBroj")]
    public string? PDVBroj { get; set; }

    [StringLength(20)]
    [Column("MaticniBroj")]
    public string? MaticniBroj { get; set; }

    [StringLength(20)]
    [Column("SifraSort")]
    public string? SifraSort { get; set; }

    [Required]
    [Column("IDVrstaPartnera")]
    public int IDVrstaPartnera { get; set; }

    [Column("Proizvodjac")]
    public int? Proizvodjac { get; set; }

    [StringLength(50)]
    [Column("BrojUgovora")]
    public string? BrojUgovora { get; set; }

    [Column("DatumUgovora")]
    public DateTime? DatumUgovora { get; set; }

    [Required]
    [Column("Kredit", TypeName = "money")]
    public decimal Kredit { get; set; } = 0;

    [Column("DatumOtvaranja")]
    public DateTime? DatumOtvaranja { get; set; }

    [StringLength(50)]
    [Column("NjihovaSifraZaNas")]
    public string? NjihovaSifraZaNas { get; set; }

    [Column("BezZabrane")]
    public int? BezZabrane { get; set; } = 0;

    [Column("TolerancijaValute")]
    public int? TolerancijaValute { get; set; }

    [Column("PartnerTimeStamp", TypeName = "timestamp")]
    [Timestamp]
    public byte[]? PartnerTimeStamp { get; set; }

    [Column("IDSinhINS")]
    public int? IDSinhINS { get; set; } = 0;

    [Column("IDSinhUPD")]
    public int? IDSinhUPD { get; set; } = 0;

    [Column("INDSinh")]
    public int? INDSinh { get; set; } = 0;

    [Column("OdlozenoPlacanje")]
    public bool? OdlozenoPlacanje { get; set; }

    [StringLength(20)]
    [Column("KategorijaKupca")]
    public string? KategorijaKupca { get; set; }

    [StringLength(20)]
    [Column("StaraSifra")]
    public string? StaraSifra { get; set; }

    // Navigation properties
    public virtual Mesto? Mesto { get; set; }
    public virtual Status? Status { get; set; }
    public virtual VrstaPartnera? VrstaPartnera { get; set; }
    public virtual Partner? PartnerGlavni { get; set; }
    public virtual ICollection<Partner> PodPartneri { get; set; } = new List<Partner>();
    public virtual ICollection<Dokument> Dokumenti { get; set; } = new List<Dokument>();

    // Business methods
    public bool IsActive() => IDStatus == 1;

    public void UpdateContactInfo(string? telefon, string? fax, string? kontakt)
    {
        Telefon = telefon;
        FAX = fax;
        Kontakt = kontakt;
    }

    public void SetRabat(float noviRabat)
    {
        if (noviRabat < 0 || noviRabat > 100)
            throw new ArgumentException("Rabat mora biti između 0 i 100%");

        Rabat = noviRabat;
    }

    public void SetCredit(decimal noviKredit)
    {
        if (noviKredit < 0)
            throw new ArgumentException("Kredit ne može biti negativan");

        Kredit = noviKredit;
    }
}