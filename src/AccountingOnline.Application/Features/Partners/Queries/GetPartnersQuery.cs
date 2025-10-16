using MediatR;
using AccountingOnline.Application.Features.Partners.DTOs;
using AccountingOnline.Application.Common.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountingOnline.Application.Features.Partners.Queries;

public record GetPartnersQuery : IRequest<List<PartnerDto>>;

public class GetPartnersQueryHandler : IRequestHandler<GetPartnersQuery, List<PartnerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPartnersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PartnerDto>> Handle(GetPartnersQuery request, CancellationToken cancellationToken)
    {
        var partners = await _context.Partners
            .Include(p => p.Mesto)
            .Include(p => p.Status)
            .Include(p => p.VrstaPartnera)
            .Include(p => p.PartnerGlavni)
            .OrderBy(p => p.NazivPartnera)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<PartnerDto>>(partners);
    }
}

public record GetPartnerByIdQuery(int Id) : IRequest<PartnerDto?>;

public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, PartnerDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPartnerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PartnerDto?> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
    {
        var partner = await _context.Partners
            .Include(p => p.Mesto)
            .Include(p => p.Status)
            .Include(p => p.VrstaPartnera)
            .Include(p => p.PartnerGlavni)
            .FirstOrDefaultAsync(p => p.IDPartner == request.Id, cancellationToken);

        return partner != null ? _mapper.Map<PartnerDto>(partner) : null;
    }
}

public record GetPartnerComboQuery : IRequest<List<PartnerComboDto>>;

public class GetPartnerComboQueryHandler : IRequestHandler<GetPartnerComboQuery, List<PartnerComboDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPartnerComboQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PartnerComboDto>> Handle(GetPartnerComboQuery request, CancellationToken cancellationToken)
    {
        // Simulate spPartnerComboStatusNabavka stored procedure
        var partners = await _context.Partners
            .Include(p => p.Mesto)
            .Include(p => p.Status)
            .Where(p => p.IDStatus == 1) // Only active partners
            .OrderBy(p => p.NazivPartnera)
            .Select(p => new PartnerComboDto
            {
                IDPartner = p.IDPartner,
                SifraPartner = p.SifraPartner,
                NazivPartnera = p.NazivPartnera,
                NazivMesta = p.Mesto != null ? p.Mesto.NazivMesta : null,
                IDStatus = p.IDStatus,
                NazivStatusa = p.Status != null ? p.Status.NazivStatusa : "Aktivan"
            })
            .ToListAsync(cancellationToken);

        return partners;
    }
}

public record SearchPartnersQuery(string SearchTerm) : IRequest<List<PartnerDto>>;

public class SearchPartnersQueryHandler : IRequestHandler<SearchPartnersQuery, List<PartnerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchPartnersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PartnerDto>> Handle(SearchPartnersQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm.ToLower();
        
        var partners = await _context.Partners
            .Include(p => p.Mesto)
            .Include(p => p.Status)
            .Include(p => p.VrstaPartnera)
            .Include(p => p.PartnerGlavni)
            .Where(p => p.NazivPartnera.ToLower().Contains(searchTerm) ||
                       p.SifraPartner.ToLower().Contains(searchTerm) ||
                       p.PIB.Contains(searchTerm) ||
                       (p.Adresa != null && p.Adresa.ToLower().Contains(searchTerm)))
            .OrderBy(p => p.NazivPartnera)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<PartnerDto>>(partners);
    }
}