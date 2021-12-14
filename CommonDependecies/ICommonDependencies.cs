﻿using aes.Models.Datatables;
using aes.Repository.UnitOfWork;

namespace aes.CommonDependecies
{
    public interface ICommonDependencies
    {
        IDatatablesGenerator DatatablesGenerator { get; }
        IDatatablesSearch DatatablesSearch { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}