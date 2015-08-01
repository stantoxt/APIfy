using APIfy.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIfy.Core.Abstractions
{
    public class CRUDAsyncController<TModel, TKey> : ApiController where TModel : class
    {
        protected CRUDRepository<TModel, TKey> ControllerRepo { get; private set; }

        protected UnitOfWork UnitOfWork { get; private set; }

        protected ExceptionHandler OnException;

        public CRUDAsyncController(DbContext db)
        {
            ControllerRepo = new CRUDRepository<TModel, TKey>(db);
            UnitOfWork = new UnitOfWork(db);
        }

        public virtual async Task<IHttpActionResult> Get()
        {
            IEnumerable<TModel> dbEntries = await ControllerRepo.GetAsync();
            return Ok(dbEntries);
        }

        public virtual async Task<IHttpActionResult> Get(TKey id)
        {
            TModel entry = await ControllerRepo.GetAsync(id);

            if (entry == null)
                return NotFound();

            return Ok(entry);
        }

        public virtual async Task<IHttpActionResult> Post([FromBody]TModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ControllerRepo.AddAsync(value);
                    await UnitOfWork.SaveChangesAsync();
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (OnException != null)
                    OnException(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }

        public virtual async Task<IHttpActionResult> Put([FromBody]TModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ControllerRepo.UpdateAsync(value);
                    await UnitOfWork.SaveChangesAsync();
                    return Ok();
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (OnException != null)
                    OnException(ex);

                return InternalServerError(ex);
            }
        }

        public virtual async Task<IHttpActionResult> Delete(TKey id)
        {
            try
            {
                await ControllerRepo.DeleteAsync(id);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                OnException(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
