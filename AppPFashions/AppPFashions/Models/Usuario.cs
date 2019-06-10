﻿using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppPFashions.Models
{
    public class Usuario
    {
        [PrimaryKey]
        public int userid { get; set; }
        public string cusuar { get; set; }
        public string dusuar { get; set; }
        public string cclave { get; set; }
        public string cfunci { get; set; }
        public string demail { get; set; }
        public string ctraba { get; set; }
        public bool IsRemembered { get; set; }
        public string Password { get; set; }
    }
}
