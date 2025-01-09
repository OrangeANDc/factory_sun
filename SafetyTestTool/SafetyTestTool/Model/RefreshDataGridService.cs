using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.Model
{
    public delegate void RefreshDataGridDelegate();
    public class RefreshDataGridService
    {
        public event RefreshDataGridDelegate OnRefreshDataGrid;

        public void RefreshDataGrid()
        {
            if (OnRefreshDataGrid != null)
                OnRefreshDataGrid();
        }
    }

    public static class RefreshDataGridInfo
    {
        public static RefreshDataGridService refreshDataGridService { get; set; } = new RefreshDataGridService();
    }
}
