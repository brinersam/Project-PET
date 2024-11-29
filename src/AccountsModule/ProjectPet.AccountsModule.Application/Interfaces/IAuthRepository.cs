using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface IAuthRepository
{
    Result<Permission, Error> GetPermissionsForRole(Guid value);
}