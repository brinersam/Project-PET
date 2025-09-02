using System.Security.Cryptography;

namespace DEVShared;
public interface IRsaKeyProvider
{
    RSA GetPrivateRsa();
    RSA GetPublicRsa();
}
