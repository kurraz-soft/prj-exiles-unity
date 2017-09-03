using buildings;

namespace peasants.workers
{
    public class Miner : PeasantBehavior
    {
        public Mine workplace;
        public State state = State.Mining;

        private Storage targetStorage;

        public enum State
        {
            Mining,
            MoveToStorage,
        }

        void Update()
        {
            if (peasant.state != Peasant.State.AtWork || !peasant.PathComplete()) return;

            int points;

            switch (state)
            {
                case State.Mining:

                    points = workplace.Mining(peasant.DecreaseEnergy());
                    if (points > 0)
                    {
                        peasant.items.AddItem(workplace.oreType, points);
                        state = State.MoveToStorage;;
                        targetStorage = GameManager.Instance.BuildingManager.GetClosestStorage(transform.position);
                        peasant.MoveTo(targetStorage);
                    }

                    break;
                case State.MoveToStorage:

                    points = peasant.items[workplace.oreType];
                    peasant.items.AddItem(workplace.oreType, -points);
                    targetStorage.items.AddItem(workplace.oreType, points);

                    state = State.Mining;
                    peasant.MoveTo(workplace);
                    targetStorage = null;

                    break;
            }
        }
    }
}
