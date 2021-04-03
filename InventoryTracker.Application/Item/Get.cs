using FluentValidation;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class Get
    {
        public class Query : IRequest<Dto.Item>
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

        public class Handler : IRequestHandler<Query, Dto.Item>
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
            public Task<Dto.Item> Handle(Query request, CancellationToken cancellationToken)
            {
                Domain.Item dbResponse = _unitOfWork.ItemRepo.Find(request.Name);
                return Task.FromResult(_mapperHelper.Map<Domain.Item, Dto.Item>(dbResponse));
            }
            #endregion
        }
    }
}