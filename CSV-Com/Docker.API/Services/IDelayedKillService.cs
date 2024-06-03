namespace Docker.API.Services
{
    public interface IDelayedKillService
    {
        void KillApplicationDelayed(int milliseconds);
    }
}