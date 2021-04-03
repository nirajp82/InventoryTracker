using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

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
        readonly IDateTimeService _dateTimeService;
        #endregion


        #region Constructor
        public RepositoryBase(IDateTimeService dateTimeService)
        {
            //Create snapshot of current repository using deep copy
            _tempRepo = JsonSerializer.Deserialize<List<TDomain>>(JsonSerializer.Serialize(_finalRepo.ToList()));

            _createdEntitiesRepo = new List<TDomain>();
            _updatedEntitiesRepo = new List<TDomain>();
            _deletedEntitiesRepo = new List<TDomain>();
            _dateTimeService = dateTimeService;
        }
        #endregion


        #region Methods
        public void Add(TDomain entity)
        {
            _tempRepo.Add(entity);
            _createdEntitiesRepo.Add(entity);
        }

        public void Delete(Expression<Func<TDomain, bool>> predicate)
        {
            var deleteList = Find(predicate).ToList();

            foreach (var entity in deleteList)
                _deletedEntitiesRepo.Add(entity);

            var list = _tempRepo.ToList();
            list.RemoveAll(e => deleteList.Contains(e));
            _tempRepo = list;
        }

        public TDomain FindFirst(Expression<Func<TDomain, bool>> predicate)
        {
            return _tempRepo.Where(predicate.Compile()).FirstOrDefault();
        }

        public IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate)
        {
            if (predicate != null)
                return _tempRepo.Where(predicate.Compile());

            return _tempRepo;
        }

        public IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate, int? offset = null, int? limit = null,
            string orderByProperty = null, bool orderByDesc = false)
        {
            var result = Find(predicate);
            if (!string.IsNullOrWhiteSpace(orderByProperty))
                result = result.OrderBy(orderByProperty, orderByDesc);
            if (offset.HasValue)
                result = result.Skip(offset.Value);
            if (limit.HasValue)
                result = result.Take(limit.Value);
            return result;
        }

        public int Count(Expression<Func<TDomain, bool>> predicate)
        {
            return Find(predicate).Count();
        }

        public bool HasAny(Expression<Func<TDomain, bool>> predicate)
        {
            return _tempRepo.Where(predicate.Compile()).Any();
        }

        public void Update(TDomain entity)
        {
            TDomain domain = _tempRepo.First(e => string.Compare(e.UniqueIdentifier, entity.UniqueIdentifier, true) == 0);
            HelperFunc.CopyProps(entity, domain);
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

            //Make sure entity does not exists in the final list
            foreach (var entity in _createdEntitiesRepo)
            {
                if (_finalRepo.Any(e => string.Compare(e.UniqueIdentifier, entity.UniqueIdentifier, true) == 0))
                    errors.Add($"{entity.UniqueIdentifier} is added by other user");
            }

            foreach (var entity in _updatedEntitiesRepo)
            {
                if (_finalRepo.Any(e => string.Compare(e.UniqueIdentifier, entity.UniqueIdentifier, true) == 0 && e.Version != entity.Version))
                    errors.Add($"Data for {entity.UniqueIdentifier} has been modified by other user!");
            }

            foreach (var entity in _deletedEntitiesRepo)
            {
                if (_finalRepo.Any(e => string.Compare(e.UniqueIdentifier, entity.UniqueIdentifier, true) == 0 && e.Version != entity.Version))
                    errors.Add($"{entity.UniqueIdentifier} has been modified/delete by other user");
            }
            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private void UpdateDefaultValue()
        {
            foreach (var entity in _createdEntitiesRepo)
            {
                entity.CreatedOn = _dateTimeService.Current;
                entity.Version = Guid.NewGuid();
            }

            foreach (var entity in _updatedEntitiesRepo)
                entity.Version = Guid.NewGuid();
        }
        #endregion
    }
}
