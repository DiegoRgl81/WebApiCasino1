using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.DTOs
{
    public class ParticipantePatchDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} solo puede tener 50 caracteres maximo")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Range(1, 54)]
        public int NumeroLoteria { get; set; }

    }
}
