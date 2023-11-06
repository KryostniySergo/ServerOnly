using MiniSoftware;
using ServerOnly.Interfaces;

namespace ServerOnly.Services
{
    public class DocumetService : IDocument
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly ILogger<DocumetService> _logger;

        public DocumetService(
            IWebHostEnvironment hostEnvironment,
            ILogger<DocumetService> logger) 
        {
            _hostingEnvironment = hostEnvironment;
            _logger = logger;
        }

        //Test
        static Dictionary<string, object> defaultValue = new Dictionary<string, object>()
        {
            ["title"] = "FooCompany",
            ["managers"] = new List<Dictionary<string, object>> {
            new Dictionary<string, object>{{"name","Jack"},{ "department", "HR" } },
            new Dictionary<string, object> {{ "name", "Loan"},{ "department", "IT" } }
        },
            ["employees"] = new List<Dictionary<string, object>> {
            new Dictionary<string, object>{{ "name", "Wade" },{ "department", "HR" } },
            new Dictionary<string, object> {{ "name", "Felix" },{ "department", "HR" } },
            new Dictionary<string, object>{{ "name", "Eric" },{ "department", "IT" } },
            new Dictionary<string, object> {{ "name", "Keaton" },{ "department", "IT" } }
        }
        };

        public Dictionary<string, object> CreateTemplateText(string[] Parameters, string[] Text)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            for (int i = 0; i < Parameters.Length; i++)
            {
                result.Add(Parameters[i], Text[i]);
            }
            return result;
        }

        public MemoryStream GetMemoryStream(string TemplateName = "TestTemplateComplex.docx")
        {
            string rootPath = _hostingEnvironment.ContentRootPath;
            string templatePath = Path.Combine(rootPath, "wwwroot", "TestTemplateComplex.docx");

            // TODO -> Нужно будет придумать: пользователи смогу создавать
            // свои template
            Dictionary<string, object> value = defaultValue;

            MemoryStream memoryStream = new MemoryStream();
            MiniWord.SaveAsByTemplate(memoryStream, templatePath, value);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
