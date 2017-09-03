using System;
using buildings;
using UnityEngine;

namespace peasants.workers
{
    public class Builder : PeasantBehavior
    {
        public Construct workplace;

        public State state = State.Free;
        public RequiredResource requiredResource;

        public enum State
        {
            Free,
            MoveToStorage,
            MoveFromStorage,
            Build,
        }

        public int MaxCanCarry { get { return 2; } }

        [Serializable]
        public struct RequiredResource
        {
            public string type;
            public int num;
            public Storage storage;

            public RequiredResource(string type, int num, Storage storage)
            {
                this.type = type;
                this.num = num;
                this.storage = storage;
            }

            public bool IsEmpty()
            {
                return num == 0;
            }

            public void Reset()
            {
                type = null;
                num = 0;
                storage = null;
            }
        }

        public void SendForResources(RequiredResource required)
        {
            requiredResource = required;
            peasant.MoveTo(required.storage);
            state = State.MoveToStorage;
        }

        void Update()
        {
            CheckMovement();
        }

        void CheckMovement()
        {
            if (peasant.state != Peasant.State.AtWork || !peasant.PathComplete()) return;

            switch (state)
            {
                case State.Free:
                    //Ramble
                    break;
                case State.MoveToStorage:
                    //Take order
                    //Check for availability of required resources
                    var storage = requiredResource.storage;
                    if (requiredResource.storage == null || !storage.items.ContainsKey(requiredResource.type))
                    {
                        //if somehow storage is missing then sending worker back to workplace
                        requiredResource.Reset();
                        state = State.Free;
                    }
                    else
                    {
                        //OK
                        var take = Math.Min(requiredResource.num, storage.items[requiredResource.type]);
                        storage.items.AddItem(requiredResource.type, -take);
                        peasant.items.AddItem(requiredResource.type, take);

                        //Change calculated num, so construct can assign missing num without waiting
                        requiredResource.num = take;

                        state = State.MoveFromStorage;
                    }

                    //Updating workplace
                    workplace.UpdateAssignedResources();

                    peasant.MoveTo(workplace);
                    break;
                case State.MoveFromStorage:
                    //Give to construct

                    peasant.items.AddItem(requiredResource.type, -requiredResource.num);
                    workplace.resourcesCost.AddItem(requiredResource.type, -requiredResource.num);
                    requiredResource.Reset();
                    workplace.UpdateAssignedResources();

                    state = State.Free;
                    break;
                case State.Build:
                    //work
                    //TODO animate
                    workplace.progressConsumedEnergy += peasant.DecreaseEnergy();

                    break;
            }
        }
    }
}
