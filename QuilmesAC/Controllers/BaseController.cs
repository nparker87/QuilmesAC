namespace QuilmesAC.Controllers
{
    using System.Web.Mvc;
    using MiniProfiler;
    using Models;

    public class BaseController : Controller
    {
        private QuilmesDataContext _quilmes;
        public QuilmesDataContext QuilmesModel
        {
            // need to build the db before this can work
            get { return _quilmes ?? (_quilmes = MiniProfilerHelper.Create<QuilmesDataContext>()); }
        }
    }
}
