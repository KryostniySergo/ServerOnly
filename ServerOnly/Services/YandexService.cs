using ServerOnly.DB.ResponseYA;
using ServerOnly.Interfaces;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerOnly.Services
{
    public class YandexService : IYandex
    {
        readonly HttpClient _client;
        readonly ILogger<YandexService> _logger;
        readonly StringBuilder _stringBuilder;
        readonly IConfiguration _configuration;

        public YandexService(IHttpClientFactory client,
            ILogger<YandexService> logger,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _client = client.CreateClient();
            _client.DefaultRequestHeaders.Add("Authorization",
               $"Api-Key {_configuration["Token:Api"]}");

            _logger = logger;
            _stringBuilder = new StringBuilder();
        }

        // TODO -> ПЕРЕНЕСТИ В CONTROLLER
        //public async void GetStt(Audio audio)
        //{
        //    var response = await GetResponseFromStt(audio.AudioId);
        //    if (response.Done != true)
        //    {
        //        //TODO-> ЧТО ТО СДЕЛАТЬ (ЖДИТЕЕЕ!!!)
        //        return;
        //    }
        //    var text = GetFullTextFromResponse(response);
        //    //_context.audios.Update(audio);
        //    //await _context.SaveChangesAsync();
        //}

        public async Task<ResponseFromYa?> GetResponse(string audioId)
        {
            //TODO -> ПОМЕНЯТЬ
            try
            {
                var result = await _client.GetAsync(
                    $"https://operation.api.cloud.yandex.net/operations/{audioId}");

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Ошибка на этапе получение текста из STT. Status code: {result.StatusCode}");
                }

                var response = await result.Content.ReadFromJsonAsync<ResponseFromYa>();

                _logger.LogTrace("GetResponse->Response получен удачно");
                return response;
            }
            catch (Exception exeption)
            {
                _logger.LogError("GetResponse->" + exeption.Message);
                return new ResponseFromYa() { Done = false };
            }
        }

        public Dictionary<string, object> CreateTextTemplate(ResponseFromYa? response)
        {
            _stringBuilder.Clear();
            var TemplatePhrases = new HashSet<string> { "наименование", "пациент", "заключение", "врач" };
            var resultDict = new Dictionary<string, object>();

            if (response == null)
            {
                foreach (var item in TemplatePhrases)
                {
                    resultDict.Add(item, "Не получилось");
                }
            }
            for (int i = 0; i < response.Response.Chunks.Count; i++)
            {
                if (TemplatePhrases.Contains(response.Response.Chunks[i].Alternatives[0].Text.ToLower()))
                {
                    resultDict.Add(response.Response.Chunks[i].Alternatives[0].Text.ToLower(),
                        response.Response.Chunks[i + 1].Alternatives[0].Text.ToLower());
                }
            }

            return resultDict;
        }

        public string GetFullText(ResponseFromYa? response)
        {
            _stringBuilder.Clear();

            if (response == null)
            {
                return "";
            }

            foreach (var item in response.Response.Chunks)
            {
                _stringBuilder.Append(item.Alternatives[0].Text + " ");
            }

            return _stringBuilder.ToString();
        }
    }
}
