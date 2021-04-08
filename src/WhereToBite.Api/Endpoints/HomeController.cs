using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace WhereToBite.Api.Endpoints
{
    public class HomeController
    {
        // GET: /<controller>/
        [UsedImplicitly]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}