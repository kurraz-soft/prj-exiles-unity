using buildings;
using buildings.resources;
using items;
using scripts.resources;

namespace peasants.workers
{
    class Lumberjack : PeasantBehavior
    {
        public LumberjackHut workplace;

        public State state = State.Free;

        private LumberTree targetTree;
        private Storage targetStorage;

        public enum State
        {
            Free,
            MoveToTree,
            MoveToHut,
            DropToStorage,
        }

        void Update()
        {
            if(peasant.state != Peasant.State.AtWork || !peasant.PathComplete()) return;

            int value;

            switch (state)
            {
                case State.Free:
                    
                    //Find tree

                    targetTree = workplace.GetTree();

                    if (targetTree != null)
                    {
                        peasant.MoveTo(targetTree.transform.position);
                        state = State.MoveToTree;
                    }
                    else
                    {
                        //TODO Ramble
                    }

                    break;
                case State.MoveToTree:

                    //Cut the tree
                    //TODO animate
                    targetTree.hitpoints -= peasant.DecreaseEnergy();
                    if (targetTree.hitpoints <= 0)
                    {
                        value = targetTree.Cut();
                        //Get wood to inventory
                        peasant.items.AddItem(ItemType.WOOD, value);

                        state = State.MoveToHut;
                        peasant.MoveTo(workplace);
                    }

                    break;
                case State.MoveToHut:

                    //place wood to hut
                    workplace.resourceCollected += peasant.items[ItemType.WOOD];
                    peasant.items.AddItem(ItemType.WOOD, -peasant.items[ItemType.WOOD]);

                    if (workplace.CanDrop())
                    {
                        //Move collected to storage
                        workplace.resourceCollected -= workplace.maxResourceDropped;
                        peasant.items.AddItem(ItemType.WOOD, workplace.maxResourceDropped);

                        targetStorage = GameManager.Instance.BuildingManager.GetClosestStorage(transform.position);
                        state = State.DropToStorage;
                        peasant.MoveTo(targetStorage);
                    }
                    else
                    {
                        state = State.Free;
                    }

                    break;

                case State.DropToStorage:

                    //Place collected to storage and go back
                    value = peasant.items[ItemType.WOOD];
                    peasant.items.AddItem(ItemType.WOOD, -value);

                    targetStorage.items.AddItem(ItemType.WOOD, value);
                    targetStorage = null;

                    state = State.Free;

                    break;
            }
        }
    }
}
