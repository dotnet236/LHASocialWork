namespace LHASocialWork.Core.Mapping
{
    public static class MappingConfiguration
    {
        public static void Configure()
        {
            AccountMappingConfiguration.Configure();
            AdminAccountMappingConfiguration.Configure();
            PositionMappingConfiguration.Configure();
            EventMappingConfiguration.Configure();
        }
    }
}