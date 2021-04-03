using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class Save
    {
        public class Command : IRequest
        {
            [JsonIgnore]
            public string Name { get; set; }

            public int Quantity { get; set; }

            public Guid? Version { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Quantity).GreaterThanOrEqualTo(0);
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
                Domain.Item model = _mapperHelper.Map<Command, Domain.Item>(request);
                _unitOfWork.ItemRepo.Save(model);
                _unitOfWork.Commit();
                return Task.FromResult(Unit.Value);
            } 
            #endregion
        }
    }
}
