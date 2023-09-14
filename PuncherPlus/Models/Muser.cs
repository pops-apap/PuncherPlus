using System;
using System.Collections.Generic;

namespace PuncherPlus.Models;

public partial class Muser
{
    public int Id { get; set; }

    public string Nick { get; set; } = null!;

    public string GivenName { get; set; } = null!;

    public string FamilyName { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
