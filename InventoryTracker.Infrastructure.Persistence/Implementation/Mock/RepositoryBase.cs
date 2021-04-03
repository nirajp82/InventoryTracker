using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    internal abstract class RepositoryBase<TDomain> : IMockRepo, IRepositoryBase<TDomain> where TDomain : IBaseDomain
    {
        #region Members
        static IEnumerable<TDomain> _finalRepo = new List<TDomain>();
        static object _counterLock = new object();

        ICollection<TDomain> _tempRepo = new List<TDomain>();
        readonly ICollection<TDomain> _createdEntitiesRepo;
        readonly List<TDomain> _deletedEntitiesRepo;
        readonly ICollection<TDomain> _updatedEntitiesRepo = new List<TDomain>();
        #endregion


        #region Constructor
        public RepositoryBase()
        {
            _tempRepo = _finalRepo.ToList();

            _createdEntitiesRepo = new List<TDomain>();
            _updatedEntitiesRepo = new List<TDomain>();
            _deletedEntitiesRepo = new List<TDomain>();
        }
        #endregion


        #region Methods
        public void Add(TDomain entity)
        {
            _tempRepo.Add(entity);
            _createdEntitiesRepo.Add(entity);
        }

        public void Delete(Expression<Func<TDomain, bool>> predicate, CancellationToken cancellationToken = default)
        {
            _tempRepo = Find(predicate).ToList();
            foreach (var item in _finalRepo)
            {
                if (_tempRepo?.Any(e => e.UniqueIdentifier == item.UniqueIdentifier) == true)
                    _deletedEntitiesRepo.Add(item);
            }
        }

        public IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate)
        {
            return _tempRepo.Where(predicate.Compile());
        }

        public bool HasAny(Expression<Func<TDomain, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _tempRepo.Where(predicate.Compile()).Any();
        }

        public void Update(TDomain entity)
        {
            TDomain domain = _tempRepo.First(e => e.UniqueIdentifier == entity.UniqueIdentifier);
            domain = entity;
            _updatedEntitiesRepo.Add(domain);
        }

        public void OnCommit()
        {
            //execute following operation as atomic operation
            lock (_counterLock)
            {
                VerifyConcurrency();
                UpdateDefaultValue();
                _finalRepo = _tempRepo;
            }
        }
        #endregion



        #region Private Methods
        private void VerifyConcurrency()
        {
            ICollection<string> errors = new List<string>();

            //Make sure item does not exists in the final list
            foreach (var item in _createdEntitiesRepo)
            {
                if (_finalRepo.Any(e => e.UniqueIdentifier == item.UniqueIdentifier))
                    errors.Add($"{item.UniqueIdentifier} is added by other user");
            }

            foreach (var item in _updatedEntitiesRepo)
            {
                if (_finalRepo.Any(e => e.Version != item.Version))
                    errors.Add($"Data for {item.UniqueIdentifier} has been modified by other user!");
            }

            foreach (var item in _deletedEntitiesRepo)
            {
                if (_finalRepo.Any(e => e.UniqueIdentifier != item.UniqueIdentifier))
                    errors.Add($"{item.UniqueIdentifier} has been modified/delete by other user");
            }

            throw new ValidationException(string.Join(",", errors));
        }

        private void UpdateDefaultValue()
        {
            foreach (var item in _createdEntitiesRepo)
            {
                item.CreatedOn = DateTime.Now;
                item.Version = Guid.NewGuid();
            }

            foreach (var item in _updatedEntitiesRepo)
                item.Version = Guid.NewGuid();
        }
        #endregion
    }
}
