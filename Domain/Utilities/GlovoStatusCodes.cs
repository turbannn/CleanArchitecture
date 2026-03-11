using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utilities;

public static class GlovoStatusCodes
{
    public const int Ok = 200;
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int NotFound = 404;
    public const int InternalServerError = 500;
}
