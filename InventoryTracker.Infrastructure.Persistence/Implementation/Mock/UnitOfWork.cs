using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members
        IItemRepo _itemRepo;
        private readonly IDateTimeService _dateTimeService;
        #endregion


        #region Public Members
        public IItemRepo ItemRepo => _itemRepo ??= new ItemRepo(_dateTimeService);
        #endregion


        #region Constructor
        public UnitOfWork(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }
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
