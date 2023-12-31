﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.API.Models.Northwind;

[Table("region", Schema = "public")]
public partial class Region
{
    public Region()
    {
        Territories = new HashSet<Territory>();
    }

    [Key]
    [Column("region_id")]
    public int RegionId { get; set; }
    [Required]
    [Column("region_description")]
    [StringLength(50)]
    public string RegionDescription { get; set; }

    [InverseProperty(nameof(Territory.Region))]
    public virtual ICollection<Territory> Territories { get; set; }
}
