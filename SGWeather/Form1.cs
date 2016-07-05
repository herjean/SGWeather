using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SGWeather
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            callWebAPI("2hr_nowcast", "NEA_API_SECRET_KEY");
        }

        public void callWebAPI(string datasetName, string keyref)
        {
            //Construct URL
            string url = "http://nea.gov.sg/api/WebAPI?dataset=" + datasetName + "&keyref=" + keyref;

            //Call API URL

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                //print the receive xml
                Console.WriteLine("Response stream received.");
                Console.WriteLine(readStream.ReadToEnd());

                //Write it into a text file
                WriteToTextFile(readStream);
            }
            catch (WebException we)
            {
                Stream receiveStream = we.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                Console.WriteLine("Error encounted - ");
                Console.WriteLine(readStream.ReadToEnd());
            }
        }



        private void WriteToTextFile(StreamReader lines)
        {
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(@"\Weather.txt"))
            {
                outputFile.WriteLine("Response stream received.");
                outputFile.WriteLine(lines.ReadToEnd());
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
