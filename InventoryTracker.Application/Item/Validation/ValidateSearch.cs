using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    /// <summary>
    /// Search If specified criteria is valid before performing search.
    /// For ex: Dont allow user to search if Offset > Total Record Count in Repository
    /// </summary>
    public class ValidateSearch
    {
        public class Query : IRequest<bool>
        {
            public string NameLike { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(c => c.NameLike).MaximumLength(50);
                RuleFor(c => c.Offset).GreaterThanOrEqualTo(0);
                RuleFor(c => c.Limit).GreaterThanOrEqualTo(1);
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
                if (request.Offset != 0)
                {
                    Expression<Func<Domain.Item, bool>> predicate = (item
                        => string.IsNullOrWhiteSpace(request.NameLike) ||
                            item.Name.ToLower().Contains(request.NameLike));

                    int count = _unitOfWork.ItemRepo.Count(predicate);
                    if (request.Offset > count)
                        return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }
            #endregion
        }
    }
}