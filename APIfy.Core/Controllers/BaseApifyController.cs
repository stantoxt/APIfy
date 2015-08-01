using APIfy.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIfy.Core.Controllers
{
	public class BaseApifyController<TModel, TKey, TDto> : ApiController
		where TModel : class
		where TDto : class
	{
        protected DbRepository<TModel, TKey> ControllerRepo { get; private set; }

        protected UnitOfWork UnitOfWork { get; private set; }

		public BaseApifyController(DbContext db)
        {
            ControllerRepo = new DbRepository<TModel, TKey>(db);
            UnitOfWork = new UnitOfWork(db);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnitOfWork.Dispose();
			}

			base.Dispose(disposing);
		}

		protected virtual void OnException(Exception e)
		{

		}
	}
}
