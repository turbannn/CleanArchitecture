using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Results;

public class GenericGlovoResult<TValue>
{
    public bool IsSuccess { get; set; }
    public int Code { get; set; }
    public string? Error { get; set; }
    public TValue? Value { get; set; }

    private GenericGlovoResult(){}

    public static GenericGlovoResult<TValue> Success(TValue value)
    {
        return new GenericGlovoResult<TValue>
        {
            IsSuccess = true,
            Code = GlovoStatusCodes.Ok,
            Error = null,
            Value = value
        };
    }

    public static GenericGlovoResult<TValue> Fail(string error, int code)
    {
        return new GenericGlovoResult<TValue>
        {
            IsSuccess = false,
            Code = code,
            Error = error,
            Value = default
        };
    }
}
