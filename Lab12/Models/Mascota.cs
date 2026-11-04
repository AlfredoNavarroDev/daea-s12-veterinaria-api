using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Models;

public partial class Mascota
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Nombre { get; set; } = null!;

    [Required]
    [StringLength(40)]
    public string Especie { get; set; } = null!;

    [StringLength(60)]
    public string? Raza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    [Required]
    public int PropietarioId { get; set; }

    [ForeignKey("PropietarioId")]
    [InverseProperty("Mascotas")]
    public virtual Propietario? Propietario { get; set; }
}
