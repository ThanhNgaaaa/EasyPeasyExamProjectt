namespace EasyPeasyExam
{
    public class RegisterModel
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string? Username { get; set; }
        public string UserRoleName { get; set; }
    }
}
