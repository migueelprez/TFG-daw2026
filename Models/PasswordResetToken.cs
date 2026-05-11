namespace TFG.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public bool Usado { get; set; } = false;
    }
}
