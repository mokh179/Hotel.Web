using Hotel.Application.DTOs.Admin;
using Hotel.Application.Interfaces.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Dashboard
{
    // [Authorize(Roles = "Admin")]
    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;

        public IndexModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // Dashboard Data
        public StatisticsDTO Stats { get; set; }

        public async Task OnGetAsync()
        {
            // Load ALL Dashboard stats (cached)
            Stats = await _statisticsService.GetDashboardAsync();
        }
    }
}
