using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    internal class ItemRepo : RepositoryBase<Item>, IItemRepo, IMockRepo
    {
        #region Constructor
        public ItemRepo(IDateTimeService dateTimeService) : base(dateTimeService)
        {
        }
        #endregion


        #region Methods
        public void Delete(string name) => Delete(e => e.Name == name);

        public Item Find(string name) => FindFirst(e => e.Name == name);

        public void Save(Item item)
        {
            if (item.Version == default)
                Add(item);
            else
                Update(item);
        }

        public void Save(IEnumerable<Item> list) => list.ToList().ForEach(Save);
        #endregion
    }
}
