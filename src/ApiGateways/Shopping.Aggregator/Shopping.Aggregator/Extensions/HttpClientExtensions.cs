using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;


namespace Shopping.Aggregator.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Post with serialized request body.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        /// <summary>
        /// 比stackoverflow 在往content's parentt 去抓 => httpresponsemessage.
        /// Read httpresponse as DTO (connvert jsonstring to DTO)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Something went wrong calling the API: {responseMessage.ReasonPhrase}");
            }

            var dataAsString = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(dataAsString,new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
        }

    }
}
