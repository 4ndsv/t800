using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace t800
{
    class Program
    {
        private static bool ShowEx = true;

        static void Main(string[] args)
        {
            args = new[] { "sslurl=https://revoked.badssl.com/" };
            try
            {
                foreach (var paramItem in args)
                {
                    var paramArr = paramItem.Split('=');

                    switch (paramArr[0])
                    {
                        case "sslurl":
                            checkSSL(paramArr[1]);
                            break;
                        case "showex":
                            ShowEx = paramArr[1] == "n" ? false : true;
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                if (ShowEx)
                {
                    Console.WriteLine("##########EXCEPTION##########");
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.InnerException);
                }
            }
            finally
            {
                Console.Read();
            }
        }

        private static void checkSSL(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            request.GetResponse();
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                Console.WriteLine("Certificate OK");
                return true;
            }
            else
            {
                Console.WriteLine("Certificate ERROR");
                Console.WriteLine(sslPolicyErrors);
                Console.WriteLine(certificate);
                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                    Console.WriteLine(chain.ChainStatus[0].StatusInformation);


                return false;
            }
        }

        private static void Menu()
        {

        }

    }
}
