﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Genderize
{
    public class GenderizeClient
    {
        public static JsonSerializerSettings SerializerSettings { get; }
        private const string Url = "https://api.genderize.io/";
        private static readonly HttpClient DefaultInnerClient;

        private readonly HttpClient _httpClient;

        static GenderizeClient()
        {
            DefaultInnerClient = new HttpClient();
            SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public GenderizeClient() : this(DefaultInnerClient)
        {
        }

        public GenderizeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NameGender> GetNameGender(
            string name, 
            string country = null, 
            string language = null, 
            CancellationToken cancellationToken = default (CancellationToken))
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var queryValues = new Dictionary<string, string> {{"name", name}};
            if (!string.IsNullOrEmpty(country))
            {
                queryValues.Add("country_id", country);
            }
            if (!string.IsNullOrEmpty(language))
            {
                queryValues.Add("language_id", language);
            }

            var uri = new Uri(Url + ToQueryString(queryValues));
            var result = await _httpClient.GetStringAsync(uri)
                .ConfigureAwait(false);

            //TODO hande fail cases

            var nameGender = JsonConvert.DeserializeObject<NameGender>(result, SerializerSettings);
            return nameGender;
        }

        private string ToQueryString(IDictionary<string, string> values)
        {
            var array = (from key in values.Keys
                    select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(values[key])))
                .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
