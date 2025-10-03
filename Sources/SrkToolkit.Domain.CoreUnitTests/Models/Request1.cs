namespace SrkToolkit.Domain.Tests.Models;

public class Request1 : BaseRequest
{
    public string Id { get; set; }
}

public class Result1 : BaseResult<Request1, Error1>
{
    public Result1()
    {
    }

    public Result1(Request1 request)
        : base(request)
    {
    }

    public string Id { get; set; }
}

public enum Error1
{
    Unknown,
    Error42,
    IAmNotATeapot,
}