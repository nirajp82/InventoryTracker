using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    /// <summary>
    /// Check specified name is valid (exists in Reposiroty) before performing any action.
    /// </summary>
    public class Exists
    {
        public class Query : IRequest<bool>
        {
            public string Name { get; set; }
            public Query(string name)
            {
                Name = name;
            }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name).MaximumLength(50).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            #region Members
            private readonly IUnitOfWork _unitOfWork;
            #endregion


            #region Constuctor
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            #endregion


            #region Methods
            public Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                bool hasAny = _unitOfWork.ItemRepo.HasAny(e => string.Compare(e.Name, request.Name, true) == 0);
                return Task.FromResult(hasAny);
            }
            #endregion
        }
    }
}