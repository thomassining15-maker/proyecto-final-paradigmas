using System.ComponentModel.DataAnnotations;

namespace AppStreaming.web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;
    }
}
