using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IngTransactions.Services
{
    public class ClientService : IClientService
    {
        private readonly HttpClient httpClient;
        X509Certificate2 signingCertificate;
        X509Certificate2 tlsCertificate;
        private const string KeyId = "5E4299BE";
        public ClientService(X509Certificate2 sign, X509Certificate2 tls)
        {
            signingCertificate = sign;
            tlsCertificate = tls;
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(tlsCertificate);
            this.httpClient = new HttpClient(clientHandler);
            httpClient.BaseAddress = new Uri("https://api.sandbox.ing.com");
        }
        public async Task<string> Authenticate()
        {
            Uri uri = new Uri(httpClient.BaseAddress, "Service");

            var body = new Dictionary<string, string>
            {
                { "grant_type","client_credentials" },
            };

            var encodedBody = new FormUrlEncodedContent(body);
            var payload = "grant_type=client_credentials";
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sandbox.ing.com/oauth2/token");
            request.Content = encodedBody;
            var currentDate = DateTime.Now.ToUniversalTime().ToString("r");
            var computedValue = ComputeSHA256HashAsBase64String(payload);
            var digest = "SHA-256=" + computedValue;
            var signatureString = @$"(request-target): post /oauth2/token date: {currentDate} digest: {digest}";
            
            var signature = SignData(tlsCertificate, signatureString);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Digest", digest);
            request.Headers.Add("Date", currentDate);
            request.Headers.Add("TPP-Signature-Certificate", @"-----BEGIN CERTIFICATE-----MIIENjCCAx6gAwIBAgIEXkKZvjANBgkqhkiG9w0BAQsFADByMR8wHQYDVQQDDBZBcHBDZXJ0aWZpY2F0ZU1lYW5zQVBJMQwwCgYDVQQLDANJTkcxDDAKBgNVBAoMA0lORzESMBAGA1UEBwwJQW1zdGVyZGFtMRIwEAYDVQQIDAlBbXN0ZXJkYW0xCzAJBgNVBAYTAk5MMB4XDTIwMDIxMDEyMTAzOFoXDTIzMDIxMTEyMTAzOFowPjEdMBsGA1UECwwUc2FuZGJveF9laWRhc19xc2VhbGMxHTAbBgNVBGEMFFBTRE5MLVNCWC0xMjM0NTEyMzQ1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkJltvbEo4/SFcvtGiRCar7Ah/aP0pY0bsAaCFwdgPikzFj+ij3TYgZLykz40EHODtG5Fz0iZD3fjBRRM/gsFPlUPSntgUEPiBG2VUMKbR6P/KQOzmNKF7zcOly0JVOyWcTTAi0VAl3MEO/nlSfrKVSROzdT4Aw/h2RVy5qlw66jmCTcp5H5kMiz6BGpG+K0dxqBTJP1WTYJhcEj6g0r0SYMnjKxBnztuhX5XylqoVdUy1a1ouMXU8IjWPDjEaM1TcPXczJFhakkAneoAyN6ztrII2xQ5mqmEQXV4BY/iQLT2grLYOvF2hlMg0kdtK3LXoPlbaAUmXCoO8VCfyWZvqwIDAQABo4IBBjCCAQIwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cHM6Ly93d3cuaW5nLm5sL3Rlc3QvZWlkYXMvdGVzdC5jcmwwHwYDVR0jBBgwFoAUcEi7XgDA9Cb4xHTReNLETt+0clkwHQYDVR0OBBYEFLQI1Hig4yPUm6xIygThkbr60X8wMIGGBggrBgEFBQcBAwR6MHgwCgYGBACORgEBDAAwEwYGBACORgEGMAkGBwQAjkYBBgIwVQYGBACBmCcCMEswOTARBgcEAIGYJwEDDAZQU1BfQUkwEQYHBACBmCcBAQwGUFNQX0FTMBEGBwQAgZgnAQIMBlBTUF9QSQwGWC1XSU5HDAZOTC1YV0cwDQYJKoZIhvcNAQELBQADggEBAEW0Rq1KsLZooH27QfYQYy2MRpttoubtWFIyUV0Fc+RdIjtRyuS6Zx9j8kbEyEhXDi1CEVVeEfwDtwcw5Y3w6Prm9HULLh4yzgIKMcAsDB0ooNrmDwdsYcU/Oju23ym+6rWRcPkZE1on6QSkq8avBfrcxSBKrEbmodnJqUWeUv+oAKKG3W47U5hpcLSYKXVfBK1J2fnk1jxdE3mWeezoaTkGMQpBBARN0zMQGOTNPHKSsTYbLRCCGxcbf5oy8nHTfJpW4WO6rK8qcFTDOWzsW0sRxYviZFAJd8rRUCnxkZKQHIxeJXNQrrNrJrekLH3FbAm/LkyWk4Mw1w0TnQLAq+s=-----END CERTIFICATE-----");
            request.Headers.Add("authorization", $"Signature keyId=\"SN={signingCertificate.SerialNumber}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
            using (HttpResponseMessage response = await this.httpClient.SendAsync(request))
            {
                var result = await response.Content.ReadAsStringAsync();
            }

            return "";
        }

        public string ComputeSHA256HashAsBase64String(string stringToHash)
        {
            using (var hash = SHA256.Create())
            {
                Byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return Convert.ToBase64String(result);
            }
        }

        public string SignData(X509Certificate2 cert, string stringToSign)
        {
            var dataToSign = Encoding.UTF8.GetBytes(stringToSign);
            var signedData = cert.GetRSAPrivateKey().SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var base64Signature = Convert.ToBase64String(signedData);
            return base64Signature;
        }

    }
}
