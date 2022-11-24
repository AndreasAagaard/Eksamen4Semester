using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
namespace mongo_service.Services;


public class VaultSharpService
{
    private readonly ILogger<VaultSharpService> _logger;

    public VaultSharpService(ILogger<VaultSharpService> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetMongoDBString(string name) 
    {
        var EndPoint = "https://localhost:8201/";
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = 
            (message, cert, chain, sslPolicyErrors) => true;

        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod =
        new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
        // Initialize settings. You can also set proxies, custom delegates etc. here.
        var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
        {
            Namespace = "",
            MyHttpClientProviderFunc = handler
                => new HttpClient(httpClientHandler) {
                BaseAddress = new Uri(EndPoint)
            }
        };

        IVaultClient vaultClient = new VaultClient(vaultClientSettings);

        // Use client to read a key-value secret.
        Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets
            .KeyValue.V2.ReadSecretAsync(path: "mongodbstrings", mountPoint: "secret");
        var kode = kv2Secret.Data.Data[name];
        return kode.ToString();
    }

}