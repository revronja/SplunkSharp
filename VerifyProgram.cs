using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace VerifyCerts
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
		private void say()
        {
            Console.WriteLine("HI");
        }
		public static void PrintStore()
        {
            Console.WriteLine("\r\nExists Certs Name and Location");
            Console.WriteLine("------ ----- -------------------------");

            foreach (StoreLocation storeLocation in (StoreLocation[])
                Enum.GetValues(typeof(StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])
                    Enum.GetValues(typeof(StoreName)))
                {
                    X509Store store = new X509Store(storeName, storeLocation);

                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);

                        Console.WriteLine("Yes    {0,4}  {1}, {2}",
                            store.Certificates.Count, store.Name, store.Location);
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("No           {0}, {1}",
                            store.Name, store.Location);
                    }
                }
                Console.WriteLine();
            }
        }
  
		public static void AddCert()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            string file = "/Users/haydenredd/Projects/Whoislogginon/Whoislogginon/Resources/splunk-VirtualBox.crt";
            store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(file)));
            store.Close();
        }
		static void InitPhase()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            // Override automatic validation of SSL server certificates.
            ServicePointManager.ServerCertificateValidationCallback =
                   ValidateServerCertficate;
   
        }
		private static string results = null;
        private static bool ValidateServerCertficate(
                object sender,
                X509Certificate cert,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            try
            {

                X509Certificate2Collection certificatesInStore = store.Certificates;
                foreach (X509Certificate2 cert3 in certificatesInStore)
                {
                    Console.WriteLine(cert3.GetExpirationDateString());
                    Console.WriteLine(cert3.Issuer);
                    Console.WriteLine(cert3.GetEffectiveDateString());
                    Console.WriteLine(cert3.GetNameInfo(X509NameType.SimpleName, true));
                    Console.WriteLine(cert3.HasPrivateKey);
                    results += cert3.GetCertHashString();
                    Console.WriteLine(cert3.SubjectName.Name);
                    Console.WriteLine(cert3.GetCertHashString());
                    Console.WriteLine("-----------------------------------");

                }
            }
            finally
            {
                store.Close();
            }
            string someString = "041649B557582184D287858F709D805082D8B82C";

            //string results = cert.GetCertHashString();

            if (results == someString)
            {
                Console.WriteLine(results + " = " + someString);
                return true;
            }
            else
            {
                Console.WriteLine(results + " != " + someString);
                return false;
            }
        }





    }
   

}
