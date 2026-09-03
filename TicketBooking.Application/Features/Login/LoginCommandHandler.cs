using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Application.Common.Models;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Infrastructure.Interfaces;

namespace TicketBooking.Application.Features.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(IJwtProvider jwtProvider, UserManager<ApplicationUser> userManager)
        {
            _jwtProvider = jwtProvider;
            _userManager = userManager;
        }
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) 
            {
                return Result<LoginResponse>.Failure(
                    new Error("Auth.InvalidCredentials", "Invalid email or password")
                    );
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid) 
            {
                return Result<LoginResponse>.Failure(
                    new Error("Auth.InvalidCredentials", "Invalid email or password")
                );
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _jwtProvider.Generate(user, userRoles);

            var response = new LoginResponse
            (
                token,
                user.Email!,
                DateTime.UtcNow.AddHours(1) 
            );

            return Result<LoginResponse>.Success(response);
        }
    }
}
