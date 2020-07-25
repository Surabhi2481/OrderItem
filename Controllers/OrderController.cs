﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrderItem.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        Cart ob = new Cart();

        static string GetToken(string url)
        {
            var user = new User { Name = "Surabhi", Password = "surabhi" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, data).Result;
                string name = response.Content.ReadAsStringAsync().Result;
                dynamic details = JObject.Parse(name);
                return details.token;
            }
        }

        // POST: api/Order
        [HttpPost]
        [Route("api/Order")]
        public Cart Post([FromBody] Cart cart)
        {
            string token = GetToken("http://52.186.89.79/api/Token");
            ob.Id = 1;
            ob.UserId = 1;
            ob.MenuItemId = cart.MenuItemId;
            int id = ob.MenuItemId;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://52.186.89.79/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();
                response = client.GetAsync("api/MenuItem/" + id).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                ob.MenuItemName = result;
                return ob;
            }
        }

    }
}


