using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
public class RegisterDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? Country { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 4)]
    public string? Password { get; set; }
}