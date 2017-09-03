using buildings.food;

namespace peasants.workers
{
    class Fisherman : PeasantBehavior
    {
        public FishermanHut workplace;
        public State state;

        public enum State
        {
            
        }

        void Update()
        {
            if (peasant.state != Peasant.State.AtWork || !peasant.PathComplete()) return;

            switch (state)
            {
                //TODO implement
            }
        }
    }
}
