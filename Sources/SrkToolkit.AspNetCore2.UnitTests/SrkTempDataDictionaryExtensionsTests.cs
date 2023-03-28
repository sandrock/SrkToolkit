
namespace SrkToolkit.Web.Tests;

using System;
using Xunit;

public class SrkTempDataDictionaryExtensionsTests
{
    [Fact]
    public void AddError_Works()
    {
        var context = new AspNetCoreTestContext();
        context.TempData.AddError("ErrorMessage");

        var messages = context.TempData.GetAll();
        Assert.Single(messages);
    }

    [Fact]
    public void AddConfirmation_Works()
    {
        var context = new AspNetCoreTestContext();
        context.TempData.AddConfirmation("Message");

        var messages = context.TempData.GetAll();
        Assert.Single(messages);
    }

    [Fact]
    public void AddInfo_Works()
    {
        var context = new AspNetCoreTestContext();
        context.TempData.AddInfo("Message");

        var messages = context.TempData.GetAll();
        Assert.Single(messages);
    }

    [Fact]
    public void AddWarning_Works()
    {
        var context = new AspNetCoreTestContext();
        context.TempData.AddWarning("Message");

        var messages = context.TempData.GetAll();
        Assert.Single(messages);
    }
}
