using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ABM_Clientes.Models
{
    public class Clientes
    {
        public int ID { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        public DateTime? FechaDeNacimiento { get; set; }
        [Required]
        public string CUIT { get; set; }
        public string? Domicilio { get; set; }
        [Required]
        public string TelefonoCelular { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
