using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class SaveList
    {
        public class Command : IRequest
        {
            public IEnumerable<Save.Command> List { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.List).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
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


            #region Public Methods
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Item> list = _mapperHelper.MapList<Save.Command, Domain.Item>(request.List);
                _unitOfWork.ItemRepo.Save(list);
                _unitOfWork.Commit();
                return Task.FromResult(Unit.Value);
            }
            #endregion
        }
    }
}
