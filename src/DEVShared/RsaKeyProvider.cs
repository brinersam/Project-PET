using System.Security.Cryptography;

namespace DEVShared;

public class RsaKeyProvider : IRsaKeyProvider
{
    private const string _privateKeyPath = "etc/key.private";
    private const string _publicKeyPath = "etc/key.public";
    private readonly bool _createNewKeys;
    private readonly RSA _rsa;

    public RsaKeyProvider(bool createNewKeys)
    {
        _createNewKeys = createNewKeys;
        _rsa = RSA.Create();
        EnsureKeysExist();
    }

    public RSA GetPublicRsa()
    {
        byte[] publicKeyBytes = File.ReadAllBytes(_publicKeyPath);
        _rsa.ImportRSAPublicKey(publicKeyBytes, out _);
        return _rsa;
    }

    public RSA GetPrivateRsa()
    {
        byte[] privateKeyBytes = File.ReadAllBytes(_privateKeyPath);
        _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
        return _rsa;
    }

    private void EnsureKeysExist()
    {
        var isKeysRequired = _createNewKeys &&
                          (File.Exists(_privateKeyPath) == false || File.Exists(_publicKeyPath) == false);

        var currentFolder = Directory.GetCurrentDirectory();

        if (isKeysRequired)
            GenerateKeys();
    }

    private void GenerateKeys()
    {
        byte[] privateKeyBytes = _rsa.ExportRSAPrivateKey();
        byte[] publicKeyBytes = _rsa.ExportRSAPublicKey();

        Directory.CreateDirectory(_privateKeyPath.Split("/")[0]);
        Directory.CreateDirectory(_publicKeyPath.Split("/")[0]);


        File.WriteAllBytes(_privateKeyPath, privateKeyBytes);
        File.WriteAllBytes(_publicKeyPath, publicKeyBytes);
    }
}
