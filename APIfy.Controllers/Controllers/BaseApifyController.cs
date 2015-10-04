using APIfy.Repositories;
using System;
using System.Data.Entity;
using System.Web.Http;

namespace APIfy.Controllers
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
