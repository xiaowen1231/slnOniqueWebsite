using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace prjOniqueWebsite.Controllers
{
    public class EcpayController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public EcpayController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult PayInfo(string id)
        {
            var cacheData = _memoryCache.Get(id);
            if (cacheData != null)
            {
                var dataStr = JsonConvert.SerializeObject(cacheData);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataStr);
                return View("EcpayView", data);
            }

            return Content("錯誤");
        }

        public ActionResult AccountInfo(string id)
        {
            
            var cacheData = _memoryCache.Get(id);
            var dataStr = JsonConvert.SerializeObject(cacheData);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataStr);
            return View("EcpayView", data);
        }

        public ActionResult Index()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //需填入 你的網址
            var website = $"https://localhost:7056";

            var order = new Dictionary<string, string>
        {
            //特店交易編號
            { "MerchantTradeNo",  orderId},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

            //交易金額
            { "TotalAmount",  "100"},

            //交易描述
            { "TradeDesc",  "無"},

            //商品名稱
            { "ItemName",  "測試商品"},

            //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
            { "ExpireDate",  "3"},

            //自訂名稱欄位1
            { "CustomField1",  ""},

            //自訂名稱欄位2
            { "CustomField2",  ""},

            //自訂名稱欄位3
            { "CustomField3",  ""},

            //自訂名稱欄位4
            { "CustomField4",  ""},

            //綠界回傳付款資訊的至 此URL
            { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},

            //使用者於綠界 付款完成後，綠界將會轉址至 此URL
            { "OrderResultURL", $"{website}/Ecpay/PayInfo/{orderId}"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
            { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
            { "ClientRedirectURL",  $"{website}/Ecpay/AccountInfo/{orderId}"},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID",  "2000132"},

            //忽略付款方式
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

            //交易類型 固定填入 aio
            { "PaymentType",  "aio"},

            //選擇預設付款方式 固定填入 ALL
            { "ChoosePayment",  "ALL"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"},
        };

            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);

            return View(order);
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";

            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = GetSHA256(checkValue);

            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte hashByte in hashBytes)
                {
                    result.Append(hashByte.ToString("X2"));
                }

                return result.ToString();
            }
        }

    }
}
