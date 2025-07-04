﻿namespace Application.Authentication.Commands.Login
{
    public class LoginCommandDto
    {
        public required bool Success { get; set; }

        public string BearerToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string TwoFactorPendingToken { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }
    }
}
