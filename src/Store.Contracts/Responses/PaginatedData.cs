﻿using System.Collections.Generic;

#pragma warning disable 1591

namespace Store.Contracts.Responses
{
    public class PaginatedData<TItem> : Dictionary<string, object>
        where TItem: class, new()
    {
        public PagingInfo PagingInfo
        {
            set
            {
                Add("PagingInfo", value);
            }
        }

        public List<TItem> Collection
        {
            set
            {
                var collectionName = new TItem().GetType().Name;

                if (collectionName.EndsWith("s"))
                    collectionName += "es";
                else
                    collectionName += "s";

                Add(collectionName, value);
            }
        }
    }
}

#pragma warning restore 1591
