namespace Feed_Bridge.Models.DTOs
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }     
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
}

