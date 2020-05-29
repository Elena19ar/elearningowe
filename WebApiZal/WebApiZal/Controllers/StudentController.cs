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
    builder.EntitySet<Student>("Student");
    builder.EntitySet<zapis>("zapis"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StudentController : ODataController
    {
        private DBModel db = new DBModel();

        // GET: odata/Student
        [EnableQuery]
        public IQueryable<Student> GetStudent()
        {
            return db.Student;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(decimal key)
        {
            return db.Student.Count(e => e.indeks == key) > 0;
        }
    }
}
