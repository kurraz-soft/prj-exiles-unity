using peasants;

namespace buildings
{
    abstract public class WorkBuilding : Building
    {
        public int workplaces = 0;
        public Profession profession = Profession.None;

        public abstract int WorkersCount { get; }

        public abstract void WorkerArrived(Peasant p);

        public abstract void WorkerLeft(Peasant p);

        public abstract void ReleaseWorkers();
    }
}
