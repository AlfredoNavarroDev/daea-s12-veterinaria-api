using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Models;

[Index("Dni", Name = "UQ_Propietarios_Dni", IsUnique = true)]
public partial class Propietario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Nombres { get; set; } = null!;

    [Required]
    [StringLength(80)]
    public string Apellidos { get; set; } = null!;

    [Required]
    [StringLength(8, MinimumLength = 8)]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 digitos numericos.")]
    [Unicode(false)]
    public string Dni { get; set; } = null!;

    [Phone]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [EmailAddress]
    [StringLength(120)]
    public string? Email { get; set; }

    [StringLength(150)]
    public string? Direccion { get; set; }

    [JsonIgnore]
    [InverseProperty("Propietario")]
    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}
