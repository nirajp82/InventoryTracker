using FluentValidation;
using InventoryTracker.Domain;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name).MaximumLength(50).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            #region Members
            private readonly IMapperHelper _mapperHelper;
            private readonly IUnitOfWork _unitOfWork;
            #endregion


            #region Constuctor
            public Handler(IUnitOfWork unitOfWork, IMapperHelper mapperHelper)
            {
                _unitOfWork = unitOfWork;
                _mapperHelper = mapperHelper;
            }
            #endregion


            #region Methods
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _unitOfWork.ItemRepo.Delete(request.Name);
                _unitOfWork.Commit();
                return Task.FromResult(Unit.Value);
            }
            #endregion
        }
    }
}