using System.Diagnostics;

namespace Distributed.Tracing.Extensions
{
    public static class StartDiagnosticExtension
    {
        public static DiagnosticListener? SourceListener(string listener)
        {
            var source = new DiagnosticListener(listener);
            return source;
        }
        public static Activity? StartActivity(this DiagnosticListener source, string name, object? tags)
        {
            var activity = new Activity(name);
            using var act = source.StartActivity(activity, tags);
            return activity;
        }
    }
}
