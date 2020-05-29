using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApiZal.Models;

namespace WebApiZal.Controllers
{
    /*
    Для класса WebApiConfig может понадобиться внесение дополнительных изменений, чтобы добавить маршрут в этот контроллер. Объедините эти инструкции в методе Register класса WebApiConfig соответствующим образом. Обратите внимание, что в URL-адресах OData учитывается регистр символов.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiZal.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<zapis>("zapis");
    builder.EntitySet<Student>("Student"); 
    builder.EntitySet<zapis_przedmiotow>("zapis_przedmiotow"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class zapisController : ODataController
    {
        private DBModel db = new DBModel();

        // GET: odata/zapis
        [EnableQuery]
        public IQueryable<zapis> Getzapis()
        {
            return db.zapis;
        }

        // GET: odata/zapis(5)
        [EnableQuery]
        public SingleResult<zapis> Getzapis([FromODataUri] int key)
        {
            return SingleResult.Create(db.zapis.Where(zapis => zapis.zapis_numer == key));
        }

        // PUT: odata/zapis(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<zapis> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            zapis zapis = db.zapis.Find(key);
            if (zapis == null)
            {
                return NotFound();
            }

            patch.Put(zapis);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!zapisExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(zapis);
        }

        // POST: odata/zapis
        public IHttpActionResult Post(zapis zapis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.zapis.Add(zapis);
            db.SaveChanges();

            return Created(zapis);
        }

        // PATCH: odata/zapis(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<zapis> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            zapis zapis = db.zapis.Find(key);
            if (zapis == null)
            {
                return NotFound();
            }

            patch.Patch(zapis);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!zapisExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(zapis);
        }

        // DELETE: odata/zapis(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            zapis zapis = db.zapis.Find(key);
            if (zapis == null)
            {
                return NotFound();
            }

            db.zapis.Remove(zapis);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/zapis(5)/Student
        [EnableQuery]
        public SingleResult<Student> GetStudent([FromODataUri] int key)
        {
            return SingleResult.Create(db.zapis.Where(m => m.zapis_numer == key).Select(m => m.Student));
        }

        // GET: odata/zapis(5)/zapis_przedmiotow
        [EnableQuery]
        public IQueryable<zapis_przedmiotow> Getzapis_przedmiotow([FromODataUri] int key)
        {
            return db.zapis.Where(m => m.zapis_numer == key).SelectMany(m => m.zapis_przedmiotow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool zapisExists(int key)
        {
            return db.zapis.Count(e => e.zapis_numer == key) > 0;
        }
    }
}
