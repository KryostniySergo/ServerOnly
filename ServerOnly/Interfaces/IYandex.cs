using ServerOnly.DB.ResponseYA;

namespace ServerOnly.Interfaces
{
    public interface IYandex
    {
        public Task<ResponseFromYa?> GetResponse(string audioId);
        public string GetFullText(ResponseFromYa? response);
    }
}
