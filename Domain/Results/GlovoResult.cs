using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Results;

public class GlovoResult
{
    public bool IsSuccess { get; set; }
    public int Code { get; set; }
    public string? Error { get; set; }

    private GlovoResult() { }

    public static GlovoResult Success()
    {
        return new GlovoResult
        {
            IsSuccess = true,
            Code = GlovoStatusCodes.Ok,
            Error = null
        };
    }

    public static GlovoResult Fail(string error, int code)
    {
        return new GlovoResult
        {
            IsSuccess = false,
            Code = code,
            Error = error
        };
    }
}
