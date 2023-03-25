using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class MemberUpdateDto
{
    [Required]
    public string? Country { get; set; }
    
    [Required]
    [StringLength(500, ErrorMessage = "Description can be at most 500 characters long")]
    public string? Description { get; set; }
}