using Hotel.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services.Admin
{
    public interface IStatisticsService
    {
        Task<StatisticsDTO> GetSnapshotAsync();
        Task<StatisticsDTO> GetDashboardAsync();

        void ClearCache();
    }
}
