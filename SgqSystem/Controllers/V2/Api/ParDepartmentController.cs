using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using SgqSystem.Controllers.Api;

namespace SgqSystem.Controllers.V2.Api
{
    public class ParDepartmentController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParDepartment
        public IHttpActionResult GetParDepartment()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.ParDepartment.Where(x=>x.Active).ToList());
        }

        // GET: api/ParDepartment/5
        [ResponseType(typeof(ParDepartment))]
        public async Task<IHttpActionResult> GetParDepartment(int id)
        {
            ParDepartment ParDepartment = await db.ParDepartment.FindAsync(id);
            if (ParDepartment == null)
            {
                return NotFound();
            }

            return Ok(ParDepartment);
        }

        // PUT: api/ParDepartment/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParDepartment(int id, ParDepartment ParDepartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ParDepartment.Id)
            {
                return BadRequest();
            }

            db.Entry(ParDepartment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParDepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ParDepartment
        [ResponseType(typeof(ParDepartment))]
        public async Task<IHttpActionResult> PostParDepartment(ParDepartment ParDepartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParDepartment.Add(ParDepartment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ParDepartment.Id }, ParDepartment);
        }

        // DELETE: api/ParDepartment/5
        [ResponseType(typeof(ParDepartment))]
        public async Task<IHttpActionResult> DeleteParDepartment(int id)
        {
            ParDepartment ParDepartment = await db.ParDepartment.FindAsync(id);
            if (ParDepartment == null)
            {
                return NotFound();
            }

            db.ParDepartment.Remove(ParDepartment);
            await db.SaveChangesAsync();

            return Ok(ParDepartment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParDepartmentExists(int id)
        {
            return db.ParDepartment.Count(e => e.Id == id) > 0;
        }
    }
}