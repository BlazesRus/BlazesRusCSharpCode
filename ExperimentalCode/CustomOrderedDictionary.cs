﻿/*	Code Created by James Michael Armstrong (https://github.com/BlazesRus)
    Latest Code Release at https://github.com/BlazesRus/BlazesRusSharedCode
*/

using System.Collections.Generic;

namespace CSharpSharedCode.ExperimentalCode
{
    //Similar to https://msdn.microsoft.com/en-us/library/system.collections.specialized.ordereddictionary(v=vs.110).aspx
    public class CustomOrderedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// Holds an index referring to keys contained for optional ordered retrieval of values
        /// </summary>
        List<TKey> KeyIndex = new List<TKey>();

        /// <summary>
        /// Applies Add while recording into key index
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            KeyIndex.Add(key);
            base.Add(key, value);
        }

        ///// <summary>
        ///// Applies Add while recording into key index or combine key contents into value if already exists
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        //public void CombineOrAdd(TKey key, TValue value)
        //{
        //    if(this.ContainsKey(key))
        //    {
        //        var KeyContents  = this[key];
        //        KeyContents += value;
        //        this[key] = this[key]+value;
        //    }
        //    else
        //    {
        //        this.Add(key, value);
        //    }
        //}

        /// <summary>
        /// Applies Add without recording into key index (default Dictionary Add operation)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddUnindexed(TKey key, TValue value)
        {
            base.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool Remove(TKey key)
        {
            KeyIndex.Remove(key);
            return base.Remove(key);
        }
        /// <summary>
        /// Applies Remove without removing from key index (default Dictionary Remove operation);Might causes errors for ValueAt operation
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveUnindexed(TKey key)
        {
            return base.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Clear()
        {
            KeyIndex.Clear();
            base.Clear();
        }
        /// <summary>
        /// Retrieves value in Dictionary at key index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue ValueAt(int index)
        {
            TKey IndexedKey = KeyIndex[index];
            TValue RetrievedValue = default(TValue);
            TryGetValue(IndexedKey, out RetrievedValue);
            return RetrievedValue;
        }
    }
}
