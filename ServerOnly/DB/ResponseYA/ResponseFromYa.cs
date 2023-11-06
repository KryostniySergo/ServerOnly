namespace ServerOnly.DB.ResponseYA
{
    public partial class ResponseFromYa
    {
        public bool Done { get; set; }
        public Response Response { get; set; }
        public string Id { get; set; }
    }

    public partial class Response
    {
        public List<Chunk> Chunks { get; set; }
    }

    public partial class Chunk
    {
        public List<Alternative> Alternatives { get; set; }
    }

    public partial class Alternative
    {
        public string Text { get; set; }
    }
}
