
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using SrkToolkit.Domain;

    public static class SrkDomainControllerExtensions
    {
        public static bool ValidateResult<TRequest, TResultCode>(
            this Controller controller, SrkToolkit.Domain.BaseResult<TRequest, TResultCode> result,
            MessageDisplayMode errorDisplayMode = MessageDisplayMode.ModelState)
            where TRequest : SrkToolkit.Domain.BaseRequest
            where TResultCode : struct
        {
            ShowResultErrors(controller, result.Errors, errorDisplayMode);

            if (result.Request != null)
            {
                foreach (var item in result.Request.ValidationErrors)
                {
                    foreach (var error in item.Value)
                    {
                        controller.ModelState.AddModelError(item.Key, error);
                    }
                }
            }

            return result.Succeed;
        }

        public static bool ValidateResult<TResultCode>(
            this Controller controller,
            SrkToolkit.Domain.BasicResult<TResultCode> result,
            MessageDisplayMode errorDisplayMode = MessageDisplayMode.ModelState)
            where TResultCode : struct
        {
            ShowResultErrors(controller, result.Errors, errorDisplayMode);

            return result.Succeed;
        }

        private static void ShowResultErrors<TResultCode>(Controller controller, IList<ResultError<TResultCode>> errors, MessageDisplayMode displayMode)
            where TResultCode : struct
        {
            if (errors != null && errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; i++)
                {
                    string message = errors[i].DisplayMessage
                                  ?? errors[i].Code.ToString();
                    switch (displayMode)
                    {
                        case MessageDisplayMode.None:
                            break;

                        case MessageDisplayMode.TempData:
                            controller.TempData.AddError(message);
                            break;
                        case MessageDisplayMode.ModelState:
                            controller.ModelState.AddModelError("", message);
                            break;

                        default:
                            throw new NotSupportedException("Display mode '" + displayMode + "' is not supported.");
                    }
                }
            }
        }
    }
}
