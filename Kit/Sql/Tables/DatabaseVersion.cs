﻿using System;
using System.Collections.Generic;
using System.Text;
using Kit.Sql.Attributes;

namespace Kit.Sql.Tables
{
    public class DatabaseVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Version { get; set; }
        public DatabaseVersion() { }
    }
}
