using System;

namespace items
{
    [Serializable]
    public class ItemList : SerializableDictionary<string, int>
    {
        public override string ToString()
        {
            var s = "{";

            foreach (var kv in this)
            {
                s += kv.Key + ": " + kv.Value + "; ";
            }

            s += "}";

            return s;
        }

        public void AddItem(string type, int val)
        {
            if (!ContainsKey(type))
                this[type] = 0;

            var res = this[type] + val;
            if (res <= 0)
                Remove(type);
            else
                this[type] = res;
        }

        public ItemList Merge(ItemList itemList)
        {
            var res = Util.DeepClone(this) as ItemList;

            foreach (var kv in itemList)
            {
                if (!res.ContainsKey(kv.Key))
                    res[kv.Key] = 0;
                res[kv.Key] += kv.Value;
            }

            return res;
        }
    }
}