using APIfy.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;

namespace APIfy.Core.Abstractions
{
    public class CRUDController<TModel, TKey> : ApiController where TModel : class
    {
        protected CRUDRepository<TModel, TKey> ControllerRepo { get; private set; }

        protected UnitOfWork UnitOfWork { get; private set; }

        protected ExceptionHandler OnException;

        public CRUDController(DbContext db)
        {
             ControllerRepo = new CRUDRepository<TModel, TKey>(db);
             UnitOfWork = new UnitOfWork(db);
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
                if (OnException != null)
                    OnException(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }

        public virtual IHttpActionResult Put(TKey id, [FromBody]TModel value)
        {
            try
            {
                if (ModelState.IsValid) {
                    ControllerRepo.Update(id, value);
                    UnitOfWork.SaveChanges();
                    return Ok();
                }

                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                if (OnException != null)
                    OnException(ex);

                return InternalServerError(ex);
            }
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
                if (OnException != null)
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

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                UnitOfWork.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
