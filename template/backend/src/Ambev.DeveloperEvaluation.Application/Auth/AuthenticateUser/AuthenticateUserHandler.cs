using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticateUserHandler(
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            // var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            
            // if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            // {
            //     throw new UnauthorizedAccessException("Invalid credentials");
            // }

            // var activeUserSpec = new ActiveUserSpecification();
            // if (!activeUserSpec.IsSatisfiedBy(user))
            // {
            //     throw new UnauthorizedAccessException("User is not active");
            // }

            // var token = _jwtTokenGenerator.GenerateToken(user);

            return Task.FromResult(new AuthenticateUserResult
            {
                Token = string.Empty, // token,
                // Email = user.Email,
                // Name = user.Username,
                // Role = user.Role.ToString()
            });
        }
    }
}
