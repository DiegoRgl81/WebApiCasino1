using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("api/alumnos")]
    public class RifasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public RifasController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet] //api/rifas/
        public async Task<ActionResult<List<Rifa>>> Get()
        {
            return await dbContext.Rifas.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<List<Rifa>>> Post(Rifa rifa)
        {
            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")] // api/rifas/1
        public async Task<ActionResult> Put(Rifa rifa, int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            if (rifa.Id != id)
            {
                return BadRequest("El id de la rifa no coincide con el establecido en la url.");
            }

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
