﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vanguard.Framework.Core.Repositories;

namespace Vanguard.Framework.Data.Tests.Repositories
{
    public class Product : IEntity
    {
        public Product()
        {
        }

        public Product(int id, string name, string category)
        {
            Id = id;
            Name = name;
            Category = category;
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }
    }
}
