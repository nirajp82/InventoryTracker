using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryTracker.Application
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Dto.Item>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Dto.Item>>
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
            public Task<IEnumerable<Dto.Item>> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Item> dbResponse = _unitOfWork.ItemRepo.GetAll();
                return Task.FromResult(_mapperHelper.MapList<Domain.Item, Dto.Item>(dbResponse));
            }
            #endregion
        }
    }
}