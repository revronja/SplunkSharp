using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Gtk;
using System.Xml.Serialization;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
	//}


	//public static void Main(string[] args)
	//{

	//    InitPhase();
	//    //Task t = new Task(DownloadPageAsync());
	//    //t.Start();
	//    string res = DownloadPageAsync().Result;
	//    Console.WriteLine(res);

	//}
	//
	//      //static readonly byte[] apiCertHash = { 0xZZ, 0xYY, ....};
	//        //sha256
	//        //4DEC549B9BA6CD92510C9F0F41A94C6C33E62A2CBDA22696B0BB13E0D1B0DAB

	static string someString = "041649B557582184D287858F709D805082D8B82C";

        static void InitPhase()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            // Override automatic validation of SSL server certificates.
            ServicePointManager.ServerCertificateValidationCallback =
                   ValidateServerCertficate;
        }

        private static bool ValidateServerCertficate(
                object sender,
                X509Certificate cert,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
        {
            //// The path to the certificate.
            string Certificate = "/Users/haydenredd/Projects/SplunkSharp/SplunkSharp/Resources/splunk-VirtualBox.crt";

            // Load the certificate into an X509Certificate object.
            cert = X509Certificate.CreateFromCertFile(Certificate);

            string results = cert.GetCertHashString();

            if (results == someString)
            {
                Console.WriteLine(results + " = " + someString);
                return true;
            }
            else
            {
                Console.WriteLine(results + " != " + someString);
			    
                return false;
			    //Environment.Exit(1);
            }
          
        }
        
    	[XmlType("response")]
        public class Token
        {
            [XmlElement("sessionKey")]
            public string sessionKey { get; set; }
        }

        public static string respstring;

        private static async ValueTask<string> DownloadPageAsync()
        {

            string page = "https://10.0.0.62:8089/services/auth/login";

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", ""));
            postData.Add(new KeyValuePair<string, string>("password", ""));

            HttpContent body = new FormUrlEncodedContent(postData);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await
                   client.PostAsync(page, body))
            using (HttpContent content = response.Content)
		    {
			    client.DefaultRequestHeaders.Add("Authorization", "Basic");
                string result = await content.ReadAsStringAsync();
                //Console.WriteLine(result);

                var res2 = await content.ReadAsStreamAsync();
            

                XmlSerializer serializer = new XmlSerializer(typeof(Token));
                var deserialized = (Token)serializer.Deserialize(res2);

                respstring += deserialized.sessionKey;

                //respstring += result;

        }

        return respstring;
    }
}

    
