using AppPFashions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppPFashions.ViewModels
{
    public class ViewModel
    {
        public List<DashboardEficiencia> Data { get; set; }

        public ViewModel()
        {
            Data = new List<DashboardEficiencia>()
            {
                new DashboardEficiencia { fprdia = "David", pefici = 180 },
                new DashboardEficiencia { fprdia = "Michael",  pefici= 170 },
                new DashboardEficiencia { fprdia = "Steve", pefici = 160 },
                new DashboardEficiencia { fprdia = "Joel", pefici = 182 }
            };
        }
    }
}
