using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.DTOs
{
    public class RifaCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener 20 caracteres maximo")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
