using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
public class RegisterDto
{
    [Required]
    [StringLength(32, ErrorMessage = "Username must be between 3 and 32 characters", MinimumLength = 3)]
    public string? Username { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? Country { get; set; }
    [Required]
    [StringLength(32, ErrorMessage = "Password must be between 7 and 32 characters", MinimumLength = 7)]
    public string? Password { get; set; }
    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}