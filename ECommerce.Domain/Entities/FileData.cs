using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities;

[Table("file_data")]
public class FileData
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("end_point")]
    public string? EndPoint { get; set; }
    
    [Column("file_name")]
    public string? FileName { get; set; }

    [Column("unique_file_name")]
    public string? UniqueFileName { get; set; }

    [Column("file_length")]
    public long  FileLength { get; set; }

    [Column("content_type")]
    public string? ContentType { get; set; }

    [Column("basket_name")]
    public string? BasketName { get; set; }
}
