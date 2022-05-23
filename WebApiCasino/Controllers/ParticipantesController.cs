using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.DTOs;
using WebApiCasino.Entidades;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("api/participantes")]
    
    public class ParticipantesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ParticipantesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet]
        public async Task<ActionResult<ParticipanteDTO>> Get()
        {
            var participante = await context.Participantes
                .Include(participanteDB => participanteDB.RifaParticipante)
                .ThenInclude(rifaParticipanteDB => rifaParticipanteDB.Rifa)
                .FirstOrDefaultAsync();
            return mapper.Map<ParticipanteDTO>(participante);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ParticipanteDTO>> Get(int id)
        {
            var participante = await context.Participantes
                .Include(participanteDB => participanteDB.RifaParticipante)
                .ThenInclude(rifaParticipanteDB => rifaParticipanteDB.Rifa)
                .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ParticipanteDTO>(participante);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> Post(ParticipanteCreacionDTO participanteCreacionDTO)
        {
            if(participanteCreacionDTO.RifasIds == null)
            {
                return BadRequest("No se puede crear un participante sin una rifa");
            }

            var rifasIds = await context.Rifas
                .Where(rifaBD => participanteCreacionDTO.RifasIds.Contains(rifaBD.Id)).Select(x => x.Id).ToListAsync();

            if(participanteCreacionDTO.RifasIds.Count != rifasIds.Count)
            {
                return BadRequest("No existe una de las rifas enviadas");
            }

            var participante = mapper.Map<Participante>(participanteCreacionDTO);

            context.Add(participante);
            await context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPut("{id:int}")] // api/rifas/1
        public async Task<ActionResult> Put(int id, ParticipanteCreacionDTO participanteCreacionDTO)
        {
            var participanteBD = await context.Participantes
                .Include(x => x.RifaParticipante)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (participanteBD == null)
            {
                return NotFound();
            }

            participanteBD = mapper.Map(participanteCreacionDTO, participanteBD);

            AsignarOrden(participanteBD);

            await context.SaveChangesAsync();
            return Ok();
        }

        private void AsignarOrden(Participante participante)
        {
            if (participante.RifaParticipante != null)
            {
                for (int i = 0; i < participante.RifaParticipante.Count; i++)
                {
                    participante.RifaParticipante[i].Orden = i;
                }
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Participantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El participante a eliminar no fue encontrado.");
            }

            context.Remove(new Participante { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //Sirve para realizar actualizaciones parciales, actualizacion de 1 o mas campos, pero no toda la entidad completa.
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ParticipantePatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var participanteDB = await context.Participantes.FirstOrDefaultAsync(x => x.Id == id);

            if (participanteDB == null)
            {
                return NotFound();
            }

            var participanteDTO = mapper.Map<ParticipantePatchDTO>(participanteDB);

            patchDocument.ApplyTo(participanteDTO, ModelState);

            var esValido = TryValidateModel(participanteDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(participanteDTO, participanteDB);

            await context.SaveChangesAsync();
            return NoContent();
        }                                                                                                               
    }
}
