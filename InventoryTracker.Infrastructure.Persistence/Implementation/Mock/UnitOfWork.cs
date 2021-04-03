using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members
        IItemRepo _itemRepo;
        #endregion


        #region Public Members
        public IItemRepo ItemRepo => _itemRepo ??= new ItemRepo();
        #endregion


        #region Methods
        public void Commit(CancellationToken cancellationToken = default)
        {
            if (ItemRepo is IMockRepo mockRepo)
                mockRepo.OnCommit();
        }
        #endregion
    }
}
