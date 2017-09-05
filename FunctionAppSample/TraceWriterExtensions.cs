using Microsoft.Azure.WebJobs.Host;
using System;
using System.Runtime.CompilerServices;

namespace FunctionAppSample
{
    static class TraceWriterExtensions
    {
        public static void WriteError(this TraceWriter trace, string message, Exception e = null, [CallerMemberName] string callerName = "")
        {
            trace.Error($"[{callerName}] {message}", e);
        }

        public static void WriteWarning(this TraceWriter trace, string message, [CallerMemberName] string callerName = "")
        {
            trace.Warning($"[{callerName}] {message}");
        }

        public static void WriteInfo(this TraceWriter trace, string message, [CallerMemberName] string callerName = "")
        {
            trace.Info($"[{callerName}] {message}");
        }

        public static void WriteVerbose(this TraceWriter trace, string message, [CallerMemberName] string callerName = "")
        {
            trace.Verbose($"[{callerName}] {message}");
        }
    }
}
