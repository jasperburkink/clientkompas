﻿using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Logout;
using Application.Authentication.Commands.RefreshToken;
using Application.Authentication.Commands.RequestResetPassword;
using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Authentication.Commands.ResetPassword;
using Application.Authentication.Commands.TwoFactorAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginCommandDto>> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshTokenCommandDto>> RefreshToken(RefreshTokenCommand command)
        {
            var result = await Mediator.Send(command);

            if (!result.Success || result.BearerToken == null || result.RefreshToken == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<LogoutCommandDto>> Logout(LogoutCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<RequestResetPasswordCommandDto>> RequestResetPassword(RequestResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<ResetPasswordCommandDto>> ResetPassword(ResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<TwoFactorAuthenticationCommandDto>> TwoFactorLogin(TwoFactorAuthenticationCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<TwoFactorAuthenticationCommandDto>> ResendTwoFactorToken(ResendTwoFactorAuthenticationTokenCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
