using DemoProject.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Hubs
{
    public class SchoolHub : Hub
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        public SchoolHub(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }
        public async Task SendPlaceOptions(int province)
        {
            var districts = await GetDistrictByProvince(province);
            Console.WriteLine("Sending districts:", districts.Select(d => new { d.DistricId, d.DistricName }));           
            await Clients.Caller.SendAsync("ReceiveDistrictOptions", districts.Select(d => new {
                districtId = d.DistricId,
                districtName = d.DistricName
            }));
        }

        private async Task<List<District>> GetDistrictByProvince(int provinceId)
        {
            var districts = await _context.Districts
                .Where(d => d.ProvinceId == provinceId)
                .ToListAsync();
            return districts;
        }
    }
}
