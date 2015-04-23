using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PrivateSpace.Logic
{
    public abstract class BaseController<T>:Controller
    {
        public abstract ActionResult Index();

        public abstract ActionResult List(int pageIndex, int pageSize = 10);

        public abstract ActionResult Detail(int id);

        public abstract JsonResult Delete(int id);

        public abstract ActionResult Add(T model);
    }
}
