
namespace SrkToolkit.Web.Tests;

using Microsoft.AspNetCore.Mvc;
using SrkToolkit.Domain;
using System;
using Xunit;

public class SrkDomainControllerExtensionsTests
{
    [Fact]
    public void ValidResult_TempData_Works()
    {
        var context = new AspNetCoreTestContext();
        var controller = new MyController();
        controller.ControllerContext = context.CreateControllerContext();
        var result = new BasicResult<ErrorCode>();
        result.Succeed = true;
        var isValid = controller.ValidateResult(result, MessageDisplayMode.TempData);
        Assert.True(isValid);
        Assert.Empty(context.TempData);
    }

    [Fact]
    public void InvalidResult_TempData_Works()
    {
        var context = new AspNetCoreTestContext();
        var controller = new MyController();
        controller.ControllerContext = context.CreateControllerContext();
        var result = new BasicResult<ErrorCode>();
        result.Errors.Add(ErrorCode.Unauthorized);
        result.Succeed = result.Errors.Count == 0;
        var isValid = controller.ValidateResult(result, MessageDisplayMode.TempData);
        Assert.False(isValid);
        Assert.Single(context.TempData);
    }

    class MyController : Controller
    {
        public ActionResult MyAction()
        {
            return this.NoContent();
        }
    }

    enum ErrorCode
    {
        Invalid,
        Unauthorized,
    }
}
