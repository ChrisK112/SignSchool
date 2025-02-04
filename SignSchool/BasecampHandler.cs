using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SignSchool
{
    public class BasecampHandler
    {


        public BasecampHandler()
        {

        }


        public async Task StartAPI(string code)
        {

             HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
              {
                    { "type", "web_server" },
                  { "client_id", "58108be6af33000fd7ca131c1dae089b8c9f3b37" },
                  { "redirect_uri", "https://client.telebank-online.com/SignSchool/Login.aspx"},
                { "client_secret", "4a92738463b201317208aa879084e0f31ca0cd0b" },
                { "code", code }
              };

  
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://launchpad.37signals.com/authorization/token", content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Do something with response. Example get content:
                var responseContent = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
                System.Diagnostics.Debug.WriteLine(responseContent);
               // return responseContent;

            }
            //return "";


            //ApiList<Project> projects = RunTest(Project.GetAllProjects(api));
            // POST https://launchpad.37signals.com/authorization/token?type=web_server&
            // client_id =your-client-id&
            //    redirect_uri=your-redirect-uri&
            //    client_secret=your-client-secret&
            //    code=verification-code



        }



    }
}