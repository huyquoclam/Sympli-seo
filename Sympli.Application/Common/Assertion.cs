using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Common;

public static class Assertion
{
    /// <summary>
    /// Check if string is not null or empty
    /// </summary>
    /// <param name="str">Object will be checked</param>
    public static void StringNotNullOrEmpty(string? str, string? fieldName = null)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new InvalidOperationException($"{fieldName ?? nameof(str)} is required");
        }
    }

    ///<summary>
    /// Check if url is valid
    /// </summary>
    /// <param name="url">Url will be checked</param>"
    public static void UrlIsValid(string? url, string? fieldName = null)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            throw new InvalidOperationException($"{fieldName ?? nameof(url)} is not a valid url");
        }
    }
}
