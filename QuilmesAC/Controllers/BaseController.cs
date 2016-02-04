namespace QuilmesAC.Controllers
{
    using MiniProfiler;
    using Models;
    using System.Web.Mvc;

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