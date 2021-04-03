using FluentValidation;
using InventoryTracker.Domain;
using InventoryTracker.Dto;
using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class Search
    {
        public class Query : IRequest<ResponseEnvelope<Dto.Item>>
        {
            public string NameLike { get; set; }
            public int Offset { get; set; } = 0;
            public int Limit { get; set; } = 5;
            public string OrderBy { get; set; } = nameof(Domain.Item.Name);
            public bool OrderByDesc { get; set; } = false;
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(c => c.NameLike).MaximumLength(50);
                RuleFor(c => c.OrderBy).MaximumLength(10);
                RuleFor(c => c.Offset).GreaterThanOrEqualTo(0);
                RuleFor(c => c.Limit).GreaterThanOrEqualTo(0);
            }
        }

        public class Handler : IRequestHandler<Query, ResponseEnvelope<Dto.Item>>
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
            public Task<ResponseEnvelope<Dto.Item>> Handle(Query request, CancellationToken cancellationToken)
            {
                return null;
                //Domain.Item dbResponse = _unitOfWork.ItemRepo.Find()
                //return Task.FromResult(_mapperHelper.Map<Domain.Item, Dto.Item>(dbResponse));
            }
            #endregion
        }
    }
}