using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.DTOs
{
    public class CredencialesUsuario
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese la contraseña")]
        public string Password  { get; set; }
    }
}
