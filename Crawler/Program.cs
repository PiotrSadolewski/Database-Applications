using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;

namespace Crawler
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            if (args.Length == 0) {
                throw new ArgumentNullException("You have to provide url adress");
            }

            string url = args[0];
            checkUrl(url);

            using (var client = new HttpClient()) {
                HttpResponseMessage responseMessage = await client.GetAsync(url);

                if (responseMessage.IsSuccessStatusCode) {
                    string text = await responseMessage.Content.ReadAsStringAsync();
                    List<string> emails = emailFinder(text);
                    if (emails.Count > 0) {
                        foreach (var email in emails) {
                            Console.WriteLine(email);
                        }
                    }
                    else 
{
                        Console.WriteLine("e-mail adress not found");
                    }
                }
            }
        }

        private static List<string> emailFinder(string t)
        {
            List<string> emaiList = new List<string>();
            Regex r1 = new Regex(@"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})", RegexOptions.IgnoreCase);
            Match m = r1.Match(t);
            while (m.Success)
            {
                emaiList.Add(m.Groups[0].Value);
                m = m.NextMatch();
            }
            emaiList = emaiList.Distinct().ToList();
            return emaiList;
        }

        private static void checkUrl(string url) 
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "Head";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
            }
            catch
            {
                throw new ArgumentException("invalid url adress");
            }
        }
    }
}
