using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.DTOs;
using WebApiCasino.Entidades;
using WebApiCasino.Filtros;
using WebApiCasino.Servicios;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("api/rifas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class RifasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;


        /*private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<RifasController> logger;*/


        public RifasController(ApplicationDbContext dbContext, IMapper mapper/*, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ILogger<RifasController> logger*/)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            /*this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;*/
        }


        /*[HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]  //Si se llega una peticion http a esta ruta, nos retorna las corespondientes instancias. 
        //Las poximas peticiones Http que lleguen despues de los 10 segundos, se van a servir del cache en si.
        //Sirve para ahorrar tirmpo de procesamiento de nuestras peticiones.
        //[Authorize]

        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new 
            {
                RifasControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                RifasControllerScoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                RifasControllerSingleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton()
            });
        }*/
        
        [HttpGet]
        //[ServiceFilter(typeof(FiltroDeAccion))]
        public async Task<ActionResult<List<RifaDTO>>> Get()
        {
            //throw new NotImplementedException(); //Filtro de Excepcion
            //logger.LogInformation("Estamos obteniendo las rifas");  //Log de information mostrado en consola
            //servicio.RealizarTarea();

            var rifas = await dbContext.Rifas.ToListAsync();
            return mapper.Map<List<RifaDTO>>(rifas);
        }

        /*Tipos de mensajes de log:
         * Critical: Mensajes de mayor severidad para el sistema
         * Error
         * Warning
         * Information
         * Debug
         * Trace: Mensajes de menor categoria */

        
        [HttpGet("{id:int}", Name = "obtenerrifas")] //api/rifas/
        public async Task<ActionResult<RifaDTO>> Get(int id)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(rifaBD => rifaBD.Id == id);

            if (rifa == null)
            {
                return NotFound();
            }

            return mapper.Map<RifaDTO>(rifa);
        }

        
        [HttpGet("{nombre}")] //api/rifas/
        public async Task<ActionResult<List<RifaDTO>>> Get([FromRoute] string nombre)
        {
            var rifas = await dbContext.Rifas.Where(rifaBD => rifaBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<RifaDTO>>(rifas);
        }

        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RifaCreacionDTO rifaCreacionDTO)
        {
            var existeRifaConNombresIguales = await dbContext.Rifas.AnyAsync(x => x.Nombre == rifaCreacionDTO.Nombre);

            if (existeRifaConNombresIguales)
            {
                return BadRequest($"Ya existe una rifa con el nombre {rifaCreacionDTO.Nombre}");
            }

            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);

            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        
        [HttpPut("{id:int}")] // api/rifas/1
        public async Task<ActionResult> Put(RifaDTO rifaCreacionDTO, int id)
        {
            var existe = await dbContext.Rifas.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            rifa.Id = id;

            dbContext.Update(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("La rifa a eliminar no fue encontrada.");
            }

            dbContext.Remove(new Rifa()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        /*[HttpGet] //api/rifas/
        public ActionResult<List<Rifa>> Get()
        {
            return new List<Rifa>()
            {
                new Rifa() { Id = 1, Nombre = "Rifa de autos" },
                new Rifa() { Id = 2, Nombre = "Rifa de casas" }
            };
        }*/
    }
}
