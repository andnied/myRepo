using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Contracts
{
    public interface IDirtyCapable
    {
        bool IsDirty { get; }
        bool IsAnyDirty();
        IEnumerable<IDirtyCapable> GetDirtyObjects();
        void CleanAllDirty();
    }
}
