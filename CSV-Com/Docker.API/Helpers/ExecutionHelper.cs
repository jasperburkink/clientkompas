using System.Diagnostics;

namespace Docker.API.Helpers
{
    public class ExecutionHelper
    {
        public static async Task<ExecutionResult> ExecuteAsync(string script, CancellationToken token)
        {
            if (!File.Exists(script))
            {
                return new(false, "Script not found", "");
            }

            var p = new Process();
            p.StartInfo.FileName = "/bin/bash";
            p.StartInfo.Arguments = $"-c \"{script}\"";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;

            _ = p.Start();

            var error = await p.StandardError.ReadToEndAsync(token);
            var output = await p.StandardOutput.ReadToEndAsync(token);

            await p.WaitForExitAsync(token);

            var success = p.ExitCode == 0;

            return new(success, error, output);
        }
    }

    public record ExecutionResult(bool Success, string Error, string Message)
    {
        public override string ToString()
        {
            return Success ? "Success" : "Error" + "\n" +
                (!string.IsNullOrWhiteSpace(Error) && !string.IsNullOrWhiteSpace(Message)
                ? $"Message: {Message}\nError: {Error}"
                : string.IsNullOrEmpty(Message) ? Error : Message);
        }
    }

}
