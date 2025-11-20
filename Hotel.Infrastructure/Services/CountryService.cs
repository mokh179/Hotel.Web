using AutoMapper;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CountryDTO>> GetAllAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync();
            return _mapper.Map<List<CountryDTO>>(countries);
        }

        public async Task<CountryDTO> GetByIdAsync(Guid id)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);
            return _mapper.Map<CountryDTO>(country);
        }

        public async Task<CountryDTO> CreateAsync(CreateCountryDTO dto)
        {
            var entity = _mapper.Map<Country>(dto);
            await _unitOfWork.Countries.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CountryDTO>(entity);
        }

        public async Task<CountryDTO> UpdateAsync(UpdateCountryDTO dto)
        {
            var entity = await _unitOfWork.Countries.GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            _unitOfWork.Countries.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CountryDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Countries.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
