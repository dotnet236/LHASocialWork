using System;
using FluentNHibernate.Automapping;

namespace LHASocialWork.Core.FluentNHibernate
{
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "LHASocialWork.Entities.MappedEntities";
        }
    }
}