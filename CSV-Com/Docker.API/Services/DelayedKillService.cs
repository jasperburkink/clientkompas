namespace Docker.API.Services
{
    public class DelayedKillService : IDelayedKillService
    {
        public void KillApplicationDelayed(int milliseconds)
        {
            _ = KillApplicationDelayedInternal(milliseconds);
        }

        private Task KillApplicationDelayedInternal(int milliseconds) => Task.Run(async () =>
        {
            await Task.Delay(milliseconds);
            Environment.Exit(0);
        });
    }
}
