using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class SaveList
    {
        public class Command : IRequest<object>
        {
            public class Item : Dto.BaseItem
            {
                public Guid? Version { get; set; }
            }

            public IEnumerable<Item> List { get; set; }
        }

        public class CommandValidator : AbstractValidator<List<Command.Item>>
        {
            public CommandValidator()
            {
                RuleFor(c => c).NotNull().NotEmpty();
                RuleForEach(c => c).ChildRules(item =>
                {
                    item.RuleFor(item => item.Name).MaximumLength(50).NotEmpty();
                    item.RuleFor(item => item.Quantity).GreaterThanOrEqualTo(0);
                });
            }
        }

        public class Handler : IRequestHandler<Command, object>
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
            public Task<object> Handle(Command command, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Item> list = _mapperHelper.MapList<SaveList.Command.Item, Domain.Item>(command.List);
                _unitOfWork.ItemRepo.Save(list);
                _unitOfWork.Commit();
                object response = new
                {
                    Inserted = command.List.Where(c => c.Version == default).Count(),
                    Updated = command.List.Where(c => c.Version != default).Count()
                };
                return Task.FromResult(response);
            }
            #endregion
        }
    }
}
