namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface IAuthRepository
{
    Task<bool> DoesUserHavePermissionCodeAsync(Guid userID, string permissionCode);
}