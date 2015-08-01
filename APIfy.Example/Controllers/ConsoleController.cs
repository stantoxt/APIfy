using System.Threading.Tasks;
using System.Web.Http;
using APIfy.Database.Contexts;
using APIfy.Database.Models;
using APIfy.Example.Filters;
using APIfy.Core.Controllers;

namespace APIfy.Example.Controllers
{
    public class ModuleController : ApifyAsyncController<Module, int>
    {
        public ModuleController()
            : base(new ApifyContext())
        {
        }

        [AuthFilter]
        public override Task<IHttpActionResult> Post([FromBody]Module value)
        {
            return base.Post(value);
        }
    }   
}
