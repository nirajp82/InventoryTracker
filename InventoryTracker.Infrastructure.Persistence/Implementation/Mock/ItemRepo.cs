using InventoryTracker.Domain;
using System.Collections.Generic;
using System.Linq;

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
        public void Delete(string name) => Delete(e => string.Compare(e.Name, name, true) == 0);

        public Item Find(string name) => FindFirst(e => string.Compare(e.Name, name, true) == 0);

        public IEnumerable<Item> GetAll() => base.Find(null);

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
