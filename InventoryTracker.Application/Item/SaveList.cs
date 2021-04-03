using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    class SaveList
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


            #region Public Methods
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Item> items = _mapperHelper.MapList<Save.Command, Domain.Item>(request.List);
                _unitOfWork.ItemRepo.Save(items);
                _unitOfWork.Commit();
                return Task.FromResult(Unit.Value);
            }
            #endregion
        }
    }
}
