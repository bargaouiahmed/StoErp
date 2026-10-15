using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoErp.Api.Modules.Auth.Dtos;
using StoErp.Modules.Auth.Application.Commands.LoginWithEmail;
using StoErp.Modules.Auth.Application.Commands.LoginWithUsername;
using StoErp.Modules.Auth.Application.Commands.RegisterUser;
using StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;
using StoErp.Modules.Auth.Application.Commands.VerifyUser;

namespace StoErp.Api.Modules.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                Firstname= request.Firstname,
                Lastname= request.Lastname,
                DateOfBirth= request.DateOfBirth,
                LanguagePreference = request.LanguagePreference
            };

            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var identifier = request.Identifier;
            var isEmail = request.Identifier.Contains("@") && request.Identifier.Contains(".") && request.Identifier.IndexOf("@") < request.Identifier.LastIndexOf(".");
            if (isEmail)
            {
                var command = new LoginWithEmailCommand(identifier, request.Password);
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            else
            {
                var command = new LoginWithUsernameCommand(identifier, request.Password);
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
        }


        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUserRequest request, CancellationToken cancellationToken)
        {
            var command = new VerifyUserCommand(request.Email, request.TokenSecret);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetEmailRequest request, CancellationToken cancellationToken)
        {
            var command = new RequestPasswordResetCommand(request.Email);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
