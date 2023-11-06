using ServerOnly.DB.ResponseYA;

namespace ServerOnly.Interfaces
{
    public interface IYandex
    {
        public Task<ResponseFromYa?> GetResponse(string audioId);
        public Dictionary<string, object> CreateTextTemplate(ResponseFromYa? response);
        public string GetFullText(ResponseFromYa? response);
    }
}
