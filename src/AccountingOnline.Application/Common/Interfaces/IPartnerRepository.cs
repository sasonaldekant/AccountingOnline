using AccountingOnline.Application.Features.Partners.DTOs;

namespace AccountingOnline.Application.Common.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<PartnerDto>> GetAllAsync();
        Task<PartnerDto?> GetByIdAsync(int id);
        Task<PartnerDto> CreateAsync(PartnerCreateDto request);
        Task<PartnerDto> UpdateAsync(int id, PartnerUpdateDto request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PartnerComboDto>> GetPartnerComboAsync();
        Task<bool> ExistsAsync(string sifraPartner, int? excludeId = null);
    }
}