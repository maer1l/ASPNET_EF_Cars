using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_EF_Cars.Models;

[Table("cars")]
public partial class Car
{
    [Key]
    [Column("car_id")]
    public int CarId { get; set; }

    [Column("brand")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Brand { get; set; }

    [Column("model")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Model { get; set; }

    [Column("speed")]
    public double? Speed { get; set; }

    [Column("price", TypeName = "money")]
    public decimal? Price { get; set; }

    [Column("year")]
    public DateOnly? Year { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }
}
