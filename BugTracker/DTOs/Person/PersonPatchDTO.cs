﻿using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class PersonPatchDTO
    {
        [Required]
        public string Name { get; set; }
        [EnumDataType(typeof(PersonRole))]
        public int? Role { get; set; }
    }
}
