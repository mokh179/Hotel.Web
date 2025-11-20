using AutoMapper;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CityDTO>> GetAllAsync()
        {
            var cities = await _unitOfWork.Cities.Query()
                .Include(c => c.Country)
                .ToListAsync();
            return _mapper.Map<List<CityDTO>>(cities);
        }

        public async Task<List<CityDTO>> GetByCountryAsync(Guid countryId)
        {
            var cities = await _unitOfWork.Cities.Query()
                .Where(c => c.CountryId == countryId)
                .Include(c => c.Country)
                .ToListAsync();
            return _mapper.Map<List<CityDTO>>(cities);
        }

        public async Task<CityDTO> GetByIdAsync(Guid id)
        {
            var city = await _unitOfWork.Cities.Query()
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CityDTO>(city);
        }

        public async Task<CityDTO> CreateAsync(CreateCityDTO dto)
        {
            var entity = _mapper.Map<City>(dto);
            await _unitOfWork.Cities.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CityDTO>(entity);
        }

        public async Task<CityDTO> UpdateAsync(UpdateCityDTO dto)
        {
            var entity = await _unitOfWork.Cities.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            _unitOfWork.Cities.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CityDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Cities.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
