using System;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiry { get; set; }

    }
}