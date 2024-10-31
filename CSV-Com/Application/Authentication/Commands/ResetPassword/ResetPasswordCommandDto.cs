namespace Application.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandDto
    {
        public required bool Success { get; set; }

        public string? Error { get; set; }
    }
}
