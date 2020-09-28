using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SynContextDemo.Controllers
{
    public class HomeController : Controller
    {
        public async Task Index()
        {
            // ASP.NET 預設採用 AspNetSynchronizationContext 實作
            // https://referencesource.microsoft.com/#System.Web/AspNetSynchronizationContext.cs,9b31a5e94e4b9894
            var sc = SynchronizationContext.Current;

            Log(1);

            await Task.Run(() =>
            {
                Log(2);

                // SynchronizationContext.Post(SendOrPostCallback, Object) 方法
                // https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.synchronizationcontext.post
                // Post方法會啟動「非同步要求」以張貼訊息。
                sc.Post((d) =>
                {
                    Thread.Sleep(1000);
                    // 這裡會使用 sc 當下取得的 Thread Context 來執行以下程式碼
                    Helpers.Log(3);
                }, null);

                Log(4);
            });

            Log(5);
        }
        public void Log(int num)
        {
            Response.Write($"<h1>{num}</h1>");
        }
    }

    public static class Helpers
    {
        public static void Log(int num)
        {
            HttpContext.Current.Response.Write($"<h1>{num}</h1>");
        }
    }
}