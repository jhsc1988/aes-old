﻿using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using aes.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ApartmentRepository : Repository<Stan>, IApartmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApartmentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Stan>> GetApartments()
        {
            // HACK: apartment id 25265 : dummy entity
            return await _unitOfWork.Apartment.Find(e => e.Id != 25265);
        }

        public async Task<IEnumerable<Stan>> GetApartmentsWithoutODSOmm()
        {
            IEnumerable<Stan> apartmentList = _context.Stan
                .FromSqlRaw("select * from Stan where Id not in (select StanId from Ods)")
                .ToList();
            return apartmentList;
        }
    }
}