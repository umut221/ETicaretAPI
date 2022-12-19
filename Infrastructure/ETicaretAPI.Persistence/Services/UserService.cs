using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly private UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                NameSurname = model.NameSurname,
                Email = model.Email
            }, model.Password);

            CreateUserResponse response = new() { Successed = result.Succeeded };
            if (result.Succeeded)
                response.Message = "User successfully created.";
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Description}\n";
                }
            return response;
        }
    }
}
