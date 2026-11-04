using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Models;

[Index("Licencia", Name = "UQ_Veterinarios_Licencia", IsUnique = true)]
public partial class Veterinario
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
    [StringLength(10)]
    [RegularExpression(@"^[A-Z]{4}\d{4}$", ErrorMessage = "La licencia debe tener el formato CMVP0001 (4 letras mayusculas + 4 digitos).")]
    [Unicode(false)]
    public string Licencia { get; set; } = null!;

    [StringLength(60)]
    public string? Especialidad { get; set; }

    [Phone]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [JsonIgnore]
    [InverseProperty("Veterinario")]
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
