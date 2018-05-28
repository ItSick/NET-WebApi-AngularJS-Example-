using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webApiDemo.Controllers
{
    public class ValuesController : ApiController
    {
        //static List<string> data = initStringUrlList();
        static List<Image> data = initImagesList();
        static List<Image> Copy = GetCopy();

        private static List<Image> initImagesList()
        {
            List<Class1> model = null;
            var client = new HttpClient();
            var task = client.GetAsync("http://api.tvmaze.com/search/shows?q=bad")
              .ContinueWith((taskwithresponse) =>
              {
                  var response = taskwithresponse.Result;
                  var jsonString = response.Content.ReadAsStringAsync();
                  jsonString.Wait();
                  model = JsonConvert.DeserializeObject<List<Class1>>(jsonString.Result);

              });
            task.Wait();
            List<Image> imagesList = new List<Image>();
            for (var i = 0; i < model.Count; i++)
            {
                if (model[i].show.image != null)
                {
                    model[i].show.image.id = i;
                    imagesList.Add(model[i].show.image);
                    imagesList.Add(model[i].show.image);
                    imagesList.Add(model[i].show.image);
                    imagesList.Add(model[i].show.image);
                }

            }

            return imagesList;
        }



        private static List<Image> GetCopy()
        {
            List<Image> copy = data;
            return copy;
        } 


        private void Reload()
        {
           Copy = data;
            
        }


        //private static List<string> initStringUrlList()
        //{
        //    List<Class1> model = null;
        //    var client = new HttpClient();
        //    var task = client.GetAsync("http://api.tvmaze.com/search/shows?q=bad")
        //      .ContinueWith((taskwithresponse) =>
        //      {
        //          var response = taskwithresponse.Result;
        //          var jsonString = response.Content.ReadAsStringAsync();
        //          jsonString.Wait();
        //          model = JsonConvert.DeserializeObject<List<Class1>>(jsonString.Result);

        //      });
        //     task.Wait();
        //     List<string> urls = new List<string>();
        //     for (var i = 0; i < model.Count; i++)
        //     {
        //        if(model[i].show.image != null)
        //        {
        //            urls.Add(model[i].show.image.medium);
        //            urls.Add(model[i].show.image.original);
        //        }
                
        //     }

        //    return urls;
        //} 

        // GET api/values
        //public List<Image> Get()
        //{
        //    return data;
        //}

        private static void RemoveDisplayedFromCopy(List<Image> currentFive)
        {
            for (var i = 0; i < currentFive.Count; i++)
            {
                for (var j = 0; j < Copy.Count; j++)
                {
                    if (currentFive[i].id == Copy[j].id)
                    {
                        Copy.Remove(Copy[j]);
                    }
                }
            }
        }



        // GET api/values
        public HttpResponseMessage Get()
        {
            Random rnd = new Random();
            List<Image> Five = Copy.OrderBy(x => rnd.Next()).Take(5).ToList();
            RemoveDisplayedFromCopy(Five);
            if (Copy.Count.Equals(0)) {
                Reload();
            }
            return Request.CreateResponse(HttpStatusCode.OK,Five);
        }

        // GET api/values/5
        public Image Get(int id)
        {
            return data[id];
        }

        // POST api/values
        public void Post([FromBody] Image value)
        {
            data.Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]Image value)
        {
            data[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            data.RemoveAt(id);
        }
    }
}