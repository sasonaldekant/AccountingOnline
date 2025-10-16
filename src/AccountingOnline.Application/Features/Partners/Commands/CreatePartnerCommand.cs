using MediatR;
using AccountingOnline.Application.Features.Partners.DTOs;
using AccountingOnline.Application.Common.Interfaces;
using AccountingOnline.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountingOnline.Application.Features.Partners.Commands;

public record CreatePartnerCommand(PartnerCreateDto PartnerDto) : IRequest<PartnerDto>;

public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, PartnerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePartnerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PartnerDto> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
    {
        // Check unique constraint
        var existingPartner = await _context.Partners
            .FirstOrDefaultAsync(p => p.SifraPartner == request.PartnerDto.SifraPartner, cancellationToken);

        if (existingPartner != null)
        {
            throw new InvalidOperationException($"Partner sa šifrom '{request.PartnerDto.SifraPartner}' već postoji.");
        }

        var entity = _mapper.Map<Partner>(request.PartnerDto);
        
        _context.Partners.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PartnerDto>(entity);
    }
}

public record UpdatePartnerCommand(PartnerUpdateDto PartnerDto) : IRequest<PartnerDto>;

public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand, PartnerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePartnerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PartnerDto> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Partners
            .FirstOrDefaultAsync(p => p.IDPartner == request.PartnerDto.IDPartner, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Partner sa ID {request.PartnerDto.IDPartner} nije pronađen.");
        }

        // Check unique constraint (exclude current partner)
        var existingPartner = await _context.Partners
            .FirstOrDefaultAsync(p => p.IDPartner != request.PartnerDto.IDPartner 
                                   && p.SifraPartner == request.PartnerDto.SifraPartner, cancellationToken);

        if (existingPartner != null)
        {
            throw new InvalidOperationException($"Partner sa šifrom '{request.PartnerDto.SifraPartner}' već postoji.");
        }

        _mapper.Map(request.PartnerDto, entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PartnerDto>(entity);
    }
}

public record DeletePartnerCommand(int PartnerId) : IRequest<bool>;

public class DeletePartnerCommandHandler : IRequestHandler<DeletePartnerCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeletePartnerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeletePartnerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Partners
            .FirstOrDefaultAsync(p => p.IDPartner == request.PartnerId, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        // Check if partner has related documents (business rule)
        var hasDocuments = await _context.Dokumenti
            .AnyAsync(d => d.IDPartner == request.PartnerId, cancellationToken);

        if (hasDocuments)
        {
            throw new InvalidOperationException("Ne možete obrisati partnera koji ima vezane dokumente.");
        }

        _context.Partners.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}