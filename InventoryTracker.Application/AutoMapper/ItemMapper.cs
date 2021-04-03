using AutoMapper;

namespace InventoryTracker.Application
{
    class ItemMapper : Profile
    {
        #region Constructor
        public ItemMapper()
        {
            Map<string, string>().ConvertUsing(str => string.IsNullOrWhiteSpace(str) ? str : str.Trim());
            Map<Save.Command, Domain.Item>();
            Map<SaveList.Command.Item, Domain.Item>();
            Map<Domain.Item, Dto.Item>();
        }
        #endregion


        #region Methods
        public IMappingExpression<source, dest> Map<source, dest>()
        {
            return CreateMap<source, dest>();
        }
        #endregion       
    }
}
