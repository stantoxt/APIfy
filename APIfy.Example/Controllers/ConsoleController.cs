using APIfy.Core.Abstractions;
using APIfy.Database.Contexts;
using APIfy.Database.Models;

namespace APIfy.Example.Controllers
{
    public class ModuleController : CRUDAsyncController<Module, int>
    {
        public ModuleController()
            : base(new ApifyContext())
        { }
    }
}
