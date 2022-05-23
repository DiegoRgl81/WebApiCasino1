using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCasino.Filtros
{
    public class FiltroDeAccion : IActionFilter
    {
        private readonly ILogger<FiltroDeAccion> logger;

        public FiltroDeAccion(ILogger<FiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)    //Se ejecuta antes de la accion
        {
            logger.LogInformation("Antes de realizar la accion");
        }

        public void OnActionExecuted(ActionExecutedContext context) //Se ejecuta coando la accion ya se ha ejecutado
        {
            logger.LogInformation("Despues de realizar la accion");
        }
    }
}
