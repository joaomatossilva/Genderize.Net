using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Genderize.Exceptions;
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
            var result = await GetResultString(uri)
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

		private async Task<string> GetResultString(Uri uri)
		{
			var httpResult = await _httpClient.GetAsync(uri)
				.ConfigureAwait(false);

			var content = await httpResult.Content.ReadAsStringAsync()
				.ConfigureAwait(false);

			if (httpResult.IsSuccessStatusCode)
			{
				return content;
			}

			ErrorData errorData = null;
			try
			{
				errorData = JsonConvert.DeserializeObject<ErrorData>(content);
			}
			catch (Exception ex)
			{
				throw new GeneralHttpException("Unable to read error data from the server", ex);
			}


			switch (httpResult.StatusCode)
			{
				case System.Net.HttpStatusCode.BadRequest:
					throw new BadRequestException(content);
				case (System.Net.HttpStatusCode) 429: // too many reqeusts
					throw new TooManyRequestsException(content);
				case System.Net.HttpStatusCode.InternalServerError:
					throw new InternalServerErrorException(content);
				default:
					throw new GeneralHttpException($"Unexpected Status code: {httpResult.StatusCode}");
			}
		}
    }
}
