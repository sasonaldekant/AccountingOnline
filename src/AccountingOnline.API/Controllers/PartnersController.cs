using Microsoft.AspNetCore.Mvc;
using MediatR;
using AccountingOnline.Application.Features.Partners.Commands;
using AccountingOnline.Application.Features.Partners.Queries;
using AccountingOnline.Application.Features.Partners.DTOs;

namespace AccountingOnline.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PartnersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartnersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Dobija sve partnere
    /// </summary>
    /// <returns>Lista partnera</returns>
    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 400)]
    public async Task<ActionResult> GetAll()
    {
        try
        {
            var partners = await _mediator.Send(new GetPartnersQuery());
            return Ok(new { success = true, data = partners, message = "Partneri uspešno učitani" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Dobija partnera po ID
    /// </summary>
    /// <param name="id">ID partnera</param>
    /// <returns>Partner</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 404)]
    [ProducesResponseType(typeof(object), 400)]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            var partner = await _mediator.Send(new GetPartnerByIdQuery(id));
            if (partner == null)
                return NotFound(new { success = false, message = "Partner nije pronađen" });

            return Ok(new { success = true, data = partner });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Kreira novog partnera
    /// </summary>
    /// <param name="createDto">Podaci za kreiranje partnera</param>
    /// <returns>Kreiran partner</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(typeof(object), 400)]
    public async Task<ActionResult> Create([FromBody] PartnerCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToArray();
                return BadRequest(new { success = false, message = "Validaciona greška", errors });
            }

            var partner = await _mediator.Send(new CreatePartnerCommand(createDto));
            return CreatedAtAction(nameof(GetById), new { id = partner.IDPartner }, 
                new { success = true, data = partner, message = "Partner uspešno kreiran" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Greška pri kreiranju partnera", error = ex.Message });
        }
    }

    /// <summary>
    /// Ažurira postojećeg partnera
    /// </summary>
    /// <param name="id">ID partnera</param>
    /// <param name="updateDto">Podaci za ažuriranje</param>
    /// <returns>Ažuriran partner</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 404)]
    public async Task<ActionResult> Update(int id, [FromBody] PartnerUpdateDto updateDto)
    {
        try
        {
            if (id != updateDto.IDPartner)
                return BadRequest(new { success = false, message = "ID partnera se ne slaže" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToArray();
                return BadRequest(new { success = false, message = "Validaciona greška", errors });
            }

            var partner = await _mediator.Send(new UpdatePartnerCommand(updateDto));
            return Ok(new { success = true, data = partner, message = "Partner uspešno ažuriran" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { success = false, message = "Partner nije pronađen" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Greška pri ažuriranju partnera", error = ex.Message });
        }
    }

    /// <summary>
    /// Briše partnera
    /// </summary>
    /// <param name="id">ID partnera</param>
    /// <returns>Rezultat brisanja</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _mediator.Send(new DeletePartnerCommand(id));
            if (!success)
                return NotFound(new { success = false, message = "Partner nije pronađen" });

            return Ok(new { success = true, message = "Partner uspešno obrisan" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Greška pri brisanju partnera", error = ex.Message });
        }
    }

    /// <summary>
    /// Dobija partnere za combo/dropdown
    /// </summary>
    /// <returns>Lista partnera za dropdown</returns>
    [HttpGet("combo")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 400)]
    public async Task<ActionResult> GetCombo()
    {
        try
        {
            var partners = await _mediator.Send(new GetPartnerComboQuery());
            return Ok(new { success = true, data = partners });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Pretraguje partnere
    /// </summary>
    /// <param name="searchTerm">Termin za pretragu</param>
    /// <returns>Lista partnera koji odgovaraju pretrazi</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 400)]
    public async Task<ActionResult> Search([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { success = false, message = "Termin za pretragu je obavezan" });
            }

            var partners = await _mediator.Send(new SearchPartnersQuery(searchTerm));
            return Ok(new { success = true, data = partners, message = $"Pronađeno {partners.Count} partnera" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}