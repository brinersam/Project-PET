﻿
using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface IAccountRepository
{
    Task<Result<User, Error>> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task Save(User user, CancellationToken cancellationToken);
}
