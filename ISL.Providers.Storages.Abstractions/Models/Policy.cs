﻿using System.Collections.Generic;

namespace ISL.Providers.Storages.Abstractions.Models
{
    public class Policy
    {
        public string PolicyName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
