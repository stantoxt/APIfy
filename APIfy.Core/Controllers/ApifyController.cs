using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using APIfy.Core.Repositories;

namespace APIfy.Core.Controllers
{
    public class ApifyController<TModel, TKey> : BaseApifyController<TModel, TKey, object>
		where TModel : class
    {
		public ApifyController(DbContext dbContext)
			: base(dbContext)
		{

		}

        public virtual IHttpActionResult Get()
        {
            IEnumerable<TModel> dbEntries = ControllerRepo.Get();
            return Ok(dbEntries);
        }

        public virtual IHttpActionResult Get(TKey id)
        {
            TModel entry = ControllerRepo.Get(id);

            if (entry == null)
                return NotFound();

            return Ok(entry);
        }

        public virtual IHttpActionResult Post([FromBody]TModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ControllerRepo.Add(value);
                    UnitOfWork.SaveChanges();
                }
                else return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                OnException(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }

        public virtual IHttpActionResult Put([FromBody]TModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ControllerRepo.Update(value);
                    UnitOfWork.SaveChanges();
                    return Ok();
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                OnException(ex);

                return InternalServerError(ex);
            }
        }

        public virtual IHttpActionResult Delete(TKey id)
        {
            try
            { 
                ControllerRepo.Delete(id);
                UnitOfWork.SaveChanges();
            }
            catch(Exception ex)
            {
                OnException(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }
    }
}
