using Microsoft.AspNetCore.Mvc;
using ServerOnly.DB;
using ServerOnly.Interfaces;

namespace ServerOnly.Controllers
{
    public class FileCreationController : ControllerBase
    {
        readonly ILogger<FileCreationController> _logger;
        readonly IYandex _yandex;
        readonly IDocument _document;
        readonly DataBaseContext _context;

        public FileCreationController(
            ILogger<FileCreationController> logger,
            IYandex yandex,
            IDocument document,
            DataBaseContext context
            )
        {
              _logger = logger;
            _yandex = yandex;
            _document = document;
            _context = context;
        }

        [HttpGet]
        [Route("getdocx")]
        public async Task<IActionResult> GetDocx()
        {
            var response = await _yandex.GetResponse("e0333ikbb0f0qk4qs94v");
            //if (response.Done != true)
            //{
            //    //TODO-> ЧТО ТО СДЕЛАТЬ (ЖДИТЕЕЕ!!!)
            //    return;
            //}
            var text = _yandex.CreateTextTemplate(response);
            var memoryStream = _document.GetMemoryStream(text, "MyTemplate.docx");

            return File(memoryStream,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "demo.docx");
        }
    }
}
