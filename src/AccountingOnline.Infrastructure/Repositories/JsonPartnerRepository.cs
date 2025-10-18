using AccountingOnline.Application.Common.Interfaces;
using AccountingOnline.Application.Features.Partners.DTOs;
using System.Text.Json;

namespace AccountingOnline.Infrastructure.Repositories
{
    public class JsonPartnerRepository : IPartnerRepository
    {
        private readonly string _jsonFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonPartnerRepository(IWebHostEnvironment environment)
        {
            _jsonFilePath = Path.Combine(environment.WebRootPath, "data", "partners.json");
            _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
            EnsureJsonFileExists();
        }

        public async Task<IEnumerable<PartnerDto>> GetAllAsync()
        {
            var jsonData = await ReadJsonDataAsync();
            return jsonData.Partners ?? new List<PartnerDto>();
        }

        public async Task<PartnerDto?> GetByIdAsync(int id)
        {
            var partners = await GetAllAsync();
            return partners.FirstOrDefault(p => p.IDPartner == id);
        }

        public async Task<PartnerDto> CreateAsync(PartnerCreateDto request)
        {
            if (await ExistsAsync(request.SifraPartner))
                throw new InvalidOperationException($"Partner sa šifrom '{request.SifraPartner}' već postoji.");

            var jsonData = await ReadJsonDataAsync();
            var partners = jsonData.Partners?.ToList() ?? new List<PartnerDto>();
            var newId = partners.Any() ? partners.Max(p => p.IDPartner) + 1 : 1;

            var newPartner = new PartnerDto
            {
                IDPartner = newId,
                SifraPartner = request.SifraPartner,
                NazivPartnera = request.NazivPartnera,
                Adresa = request.Adresa,
                IDMesto = request.IDMesto,
                PIB = request.PIB,
                Telefon = request.Telefon,
                FAX = request.FAX,
                IDReferent = request.IDReferent,
                Napomena = request.Napomena,
                Kontakt = request.Kontakt,
                IDStatus = request.IDStatus,
                IDDrzava = request.IDDrzava,
                Rabat = request.Rabat,
                Kasa = request.Kasa,
                IDNacinPlacanja = request.IDNacinPlacanja,
                IDCenovnaGrupa = request.IDCenovnaGrupa,
                Konto = request.Konto,
                IDPartnerGlavni = request.IDPartnerGlavni,
                PDVBroj = request.PDVBroj,
                MaticniBroj = request.MaticniBroj,
                SifraSort = request.SifraSort,
                IDVrstaPartnera = request.IDVrstaPartnera,
                Proizvodjac = request.Proizvodjac,
                BrojUgovora = request.BrojUgovora,
                DatumUgovora = request.DatumUgovora,
                Kredit = request.Kredit,
                DatumOtvaranja = request.DatumOtvaranja,
                NjihovaSifraZaNas = request.NjihovaSifraZaNas,
                BezZabrane = request.BezZabrane,
                TolerancijaValute = request.TolerancijaValute,
                OdlozenoPlacanje = request.OdlozenoPlacanje,
                KategorijaKupca = request.KategorijaKupca,
                StaraSifra = request.StaraSifra,
                NazivMesta = GetMestoName(request.IDMesto),
                NazivStatusa = GetStatusName(request.IDStatus),
                NazivVrstePartnera = GetVrstaPartneraName(request.IDVrstaPartnera),
                NazivDrzave = request.IDDrzava.HasValue ? GetDrzavaName(request.IDDrzava.Value) : null
            };

            partners.Add(newPartner);
            await SaveJsonDataAsync(new JsonDataContainer { Partners = partners });
            return newPartner;
        }

        public async Task<PartnerDto> UpdateAsync(int id, PartnerUpdateDto request)
        {
            if (await ExistsAsync(request.SifraPartner, id))
                throw new InvalidOperationException($"Partner sa šifrom '{request.SifraPartner}' već postoji.");

            var partners = (await GetAllAsync()).ToList();
            var existingPartner = partners.FirstOrDefault(p => p.IDPartner == id);
            if (existingPartner == null)
                throw new KeyNotFoundException($"Partner sa ID {id} nije pronađen.");

            // Update fields
            existingPartner.SifraPartner = request.SifraPartner;
            existingPartner.NazivPartnera = request.NazivPartnera;
            existingPartner.PIB = request.PIB;
            existingPartner.IDMesto = request.IDMesto;
            existingPartner.IDStatus = request.IDStatus;
            existingPartner.IDVrstaPartnera = request.IDVrstaPartnera;
            // ... (sve ostale fields)
            
            await SaveJsonDataAsync(new JsonDataContainer { Partners = partners });
            return existingPartner;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var partners = (await GetAllAsync()).ToList();
            var partnerToRemove = partners.FirstOrDefault(p => p.IDPartner == id);
            if (partnerToRemove == null) return false;
            partners.Remove(partnerToRemove);
            await SaveJsonDataAsync(new JsonDataContainer { Partners = partners });
            return true;
        }

        public async Task<IEnumerable<PartnerComboDto>> GetPartnerComboAsync()
        {
            var partners = await GetAllAsync();
            return partners.Where(p => p.IDStatus == 1)
                .Select(p => new PartnerComboDto { IDPartner = p.IDPartner, SifraPartner = p.SifraPartner, NazivPartnera = p.NazivPartnera, NazivMesta = p.NazivMesta, IDStatus = p.IDStatus, NazivStatusa = p.NazivStatusa })
                .OrderBy(p => p.NazivPartnera);
        }

        public async Task<bool> ExistsAsync(string sifraPartner, int? excludeId = null)
        {
            var partners = await GetAllAsync();
            return partners.Any(p => p.SifraPartner == sifraPartner && (!excludeId.HasValue || p.IDPartner != excludeId.Value));
        }

        private async Task<JsonDataContainer> ReadJsonDataAsync()
        {
            if (!File.Exists(_jsonFilePath)) return new JsonDataContainer { Partners = new List<PartnerDto>() };
            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<JsonDataContainer>(jsonContent, _jsonOptions) ?? new JsonDataContainer { Partners = new List<PartnerDto>() };
        }

        private async Task SaveJsonDataAsync(JsonDataContainer data)
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(_jsonFilePath, jsonContent);
        }

        private void EnsureJsonFileExists()
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            if (directory != null && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!File.Exists(_jsonFilePath))
            {
                var emptyData = new JsonDataContainer { Partners = new List<PartnerDto>() };
                File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(emptyData, _jsonOptions));
            }
        }

        private string GetMestoName(int id) => id switch { 1 => "Beograd", 2 => "Novi Sad", 3 => "Niš", 4 => "Kragujevac", 5 => "Subotica", _ => "Nepoznato" };
        private string GetStatusName(int id) => id switch { 1 => "Aktivan", 2 => "Neaktivan", 3 => "Blokiran", _ => "Nepoznat" };
        private string GetVrstaPartneraName(int id) => id switch { 1 => "Dobavljač", 2 => "Kupac", 3 => "Prevoznik", 4 => "Uslužni", _ => "Nepoznata" };
        private string GetDrzavaName(int id) => id switch { 1 => "Srbija", 2 => "Hrvatska", 3 => "BiH", 4 => "CG", _ => "Nepoznata" };

        private class JsonDataContainer { public IEnumerable<PartnerDto> Partners { get; set; } = new List<PartnerDto>(); }
    }
}