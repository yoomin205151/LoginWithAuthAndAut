﻿using System;
using System.Collections.Generic;

namespace PracticeLogin.Models.DB;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
