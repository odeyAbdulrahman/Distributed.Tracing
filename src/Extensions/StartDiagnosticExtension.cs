using System.Diagnostics;

namespace Distributed.Tracing.Extensions
{
    public static class StartDiagnosticExtension
    {
        public static void StartActivity(string listener, string name, object? tags)
        {
            var source = new DiagnosticListener(listener);
            var activity = new Activity(name);
            using var act = source.StartActivity(activity, tags);
        }
    }
}
