using System.Security.Cryptography;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface IRsaKeyProvider
{
    RSA GetPrivateRsa();
    RSA GetPublicRsa();
}
