
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public interface IResultService
    {
        ActionResult Forbidden(string message = null);
        ActionResult NotFound(string message = null);
        ActionResult BadRequest(string message = null);
        ActionResult JsonSuccess();
        ActionResult JsonSuccess(object data);
        ActionResult JsonError();
        ActionResult JsonError(string errorCode);
        ActionResult JsonError(string errorCode, string errorMessage);
    }
}
