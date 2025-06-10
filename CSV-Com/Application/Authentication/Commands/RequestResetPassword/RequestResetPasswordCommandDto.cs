namespace Application.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandDto
    {
        public required bool Success { get; set; }

        public ICollection<string> Errors { get; set; } = [];
    }
}
