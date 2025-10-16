using AccountingOnline.Domain.Common;

namespace AccountingOnline.Domain.Entities;

public class Partner : AuditableEntity
{
    public int IdPartner { get; set; }
    public string SifraPartner { get; set; } = string.Empty;
    public string NazivPartnera { get; set; } = string.Empty;
    public string? Adresa { get; set; }
    public int IdMesto { get; set; }
    public string Pib { get; set; } = string.Empty;
    public string? Telefon { get; set; }
    public string? Fax { get; set; }
    public int? IdReferent { get; set; }
    public string? Napomena { get; set; }
    public string? Kontakt { get; set; }
    public int IdStatus { get; set; }
    public int? IdDrzava { get; set; }
    public double Rabat { get; set; } = 0;
    public double Kasa { get; set; } = 0;
    public int? IdNacinPlacanja { get; set; }
    public int? IdCenovnaGrupa { get; set; }
    public string? Konto { get; set; }
    public int? IdPartnerGlavni { get; set; }
    public string? PdvBroj { get; set; }
    public DateTime? DatumOsnivanja { get; set; }
    public string? Delatnost { get; set; }
    public string? Email { get; set; }
    public string? WebSajt { get; set; }
    
    // Navigation properties
    public virtual Mesto? Mesto { get; set; }
    public virtual Status? Status { get; set; }
    public virtual Partner? PartnerGlavni { get; set; }
    public virtual ICollection<Partner> PodPartneri { get; set; } = new List<Partner>();
    public virtual ICollection<Dokument> Dokumenti { get; set; } = new List<Dokument>();
    
    // Business methods
    public bool IsActive() => Status?.Aktivan == true;
    
    public void UpdateContactInfo(string? telefon, string? email, string? kontakt)
    {
        Telefon = telefon;
        Email = email;
        Kontakt = kontakt;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetRabat(double noviRabat)
    {
        if (noviRabat < 0 || noviRabat > 100)
            throw new ArgumentException("Rabat mora biti između 0 i 100%");
            
        Rabat = noviRabat;
        UpdatedAt = DateTime.UtcNow;
    }
}