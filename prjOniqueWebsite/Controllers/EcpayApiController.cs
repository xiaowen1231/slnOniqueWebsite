using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace prjOniqueWebsite.Controllers
{
    public class EcpayApiController : ControllerBase
    {

        private readonly IMemoryCache _memoryCache;
        public EcpayApiController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpPost]
        public IActionResult AddPayInfo(JObject info)
        {
            try
            {
                _memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AddAccountInfo(JObject info)
        {
            try
            {
                _memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 回傳給 綠界 失敗
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseError()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("0|Error");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        /// <summary>
        /// 回傳給 綠界 成功
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseOK()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("1|OK");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
