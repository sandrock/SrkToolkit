
namespace SrkToolkit.Web.HttpErrors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public interface IErrorController : IController
    {
        bool IncludeExceptionDetails { get; set; }

        ActionResult Forbidden();
        ActionResult NotFound();
        ActionResult BadRequest();
        ActionResult Internal();
    }
}
