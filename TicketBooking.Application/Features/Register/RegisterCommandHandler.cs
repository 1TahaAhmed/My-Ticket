using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketBooking.Application.Common.Models;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Infrastructure.Interfaces;

namespace TicketBooking.Application.Features.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IJwtProvider _jwtProvider;

        private const string DefaultRole = "User";

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email
            );

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(new Error("Identity.RegisterFailed", errors));
            }

            if (!await _roleManager.RoleExistsAsync(DefaultRole))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(DefaultRole));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRole);
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(new Error("Identity.RoleAssignmentFailed", roleErrors));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _jwtProvider.Generate(user, userRoles);

            return Result<string>.Success(token);
        }
    }
}