﻿using System.Diagnostics;

namespace Docker.API.Helpers
{
    public class ExecutionHelper
    {
        private static readonly CancellationTokenSource s_defaultTokenSource = new();
        public static async Task<string> ExecuteDockerCommand(string command, CancellationToken? token = null)
        {
            var fullCommand = $"docker {command}";
            var result = await ExecuteAsync(fullCommand, token);
            return !result.Success ? throw new Exception(result.ToString()) : result.ToString();
        }
        public static async Task<ExecutionResult> ExecuteAsync(string script, CancellationToken? token = null)
        {
            if (!File.Exists("/bin/bash"))
            {
                return new(false, "/bin/bash is not found on current ssytem", "");
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

            var error = await p.StandardError.ReadToEndAsync(token ?? s_defaultTokenSource.Token);
            var output = await p.StandardOutput.ReadToEndAsync(token ?? s_defaultTokenSource.Token);

            await p.WaitForExitAsync(token ?? s_defaultTokenSource.Token);

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
