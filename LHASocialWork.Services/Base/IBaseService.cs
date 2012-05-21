using System.Collections.Generic;
using LHASocialWork.Entities.Core;

namespace LHASocialWork.Services.Base
{
    public interface IBaseService
    {
        T ValidateAndSave<T>(T entity) where T : ValidationRequiredEntity;
        IList<T> ValidateAndSave<T>(IList<T> entities) where T : ValidationRequiredEntity;
    }
}