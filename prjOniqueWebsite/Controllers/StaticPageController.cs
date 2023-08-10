using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace prjOniqueWebsite.Controllers
{
    public class StaticPageController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IWebHostEnvironment _environment;

        public StaticPageController(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IWebHostEnvironment environment)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _environment = environment;
        }
        public async Task<IActionResult> GenerateStaticOrderEmail(int orderId)
        {
            var viewResult = _viewEngine.FindView(ControllerContext, "OrderEmailContent", false);

            using (var sw = new StringWriter())
            {
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            { "OrderId", orderId } // 将 orderId 放入视图数据字典
        };
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDataDictionary,
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                var renderedView = sw.ToString();

                // 保存为静态 HTML 文件
                string fileName = $"{orderId}.html"; // 文件名
                string folderPath = Path.Combine(_environment.WebRootPath, "staticpage"); // 文件夹路径
                string filePath = Path.Combine(folderPath, fileName); // 完整文件路径

                Directory.CreateDirectory(folderPath); // 确保文件夹存在
                System.IO.File.WriteAllText(filePath, renderedView);
            }

            return Content($"Static view for OrderId {orderId} generated and saved.");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}



