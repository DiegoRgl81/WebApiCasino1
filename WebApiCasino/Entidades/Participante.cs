using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.Entidades
{
    public class Participante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} solo puede tener 50 caracteres maximo")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Range(1, 54)]
        public int NumeroLoteria { get; set; }

        public List<RifaParticipante> RifaParticipante { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)    //Valodacion Modelo
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new string[] { nameof(Nombre) });
                }
            }
        }*/
    }
}
