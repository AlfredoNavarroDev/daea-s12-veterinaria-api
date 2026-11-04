using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Models;

public partial class Cita
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }

    [Required]
    [StringLength(200)]
    public string Motivo { get; set; } = null!;

    [Required]
    public int MascotaId { get; set; }

    [Required]
    public int VeterinarioId { get; set; }

    [ForeignKey("MascotaId")]
    [InverseProperty("Citas")]
    public virtual Mascota? Mascota { get; set; }

    [ForeignKey("VeterinarioId")]
    [InverseProperty("Citas")]
    public virtual Veterinario? Veterinario { get; set; }
}
