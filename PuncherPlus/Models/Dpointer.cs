using System;
using System.Collections.Generic;

namespace PuncherPlus.Models;

public class Dpointer
{
    public long Id { get; set; }

    public DateTime? CreateAt { get; set; }

    public string Motivo { get; set; } = null!;
    public string MotivoTipo { get; set; } = null!;
    public string MotivoDetalle { get; set; } = null!;

    public int IdUser { get; set; }

    public string? pointerType { get; set; } 
    public string? latepunch { get; set; } 

    
}
