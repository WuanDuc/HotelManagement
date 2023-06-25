using HotelManagement.DTOs;
using HotelManagement.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.Model.Services
{
    public partial class OverviewStatisticService
    {
        private OverviewStatisticService() { }
        private static OverviewStatisticService _ins;
        public static OverviewStatisticService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new OverviewStatisticService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        public async Task<int> GetBillQuantity(int year, int month)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    
                    
                        return await context.Bills.Where(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month).CountAsync();
                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<double>, double ServiceRevenue,  double RentalRevenue, string RentalRateStr)> GetRevenueByYear(int year)
        {
            List<double> MonthlyRevenueList = new List<double>(new double[12]);

            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var billList = context.Bills
                    .Where(b => b.CreateDate.Value.Year == year);

                    (double ServiceRevenue, double RentalRevenue) = await GetFullRevenue(context, year);

                    var MonthlyRevenue = billList
                             .GroupBy(b => b.CreateDate.Value.Month)
                             .Select(gr => new
                             {
                                 Month = gr.Key,
                                 Income = gr.Sum(b => (double?)b.TotalPrice) ?? 0
                             }).ToList();

                    foreach (var re in MonthlyRevenue)
                    {
                        MonthlyRevenueList[re.Month - 1] = (float)re.Income;
                    }

                    (double lastServiceRevenue, double lastRentalRevenue) = await GetFullRevenue(context, year - 1);
                    double lastRevenueTotal = lastServiceRevenue + lastRentalRevenue;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                        if (ServiceRevenue + RentalRevenue == 0) RevenueRateStr = "-3";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ServiceRevenue + RentalRevenue) / lastRevenueTotal) - 1);
                    }

                    return (MonthlyRevenueList, (double)ServiceRevenue, (double)RentalRevenue, RevenueRateStr);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<(double, double)> GetFullRevenue(HotelManagementEntities context, int year, int month = 0)
        {
            try
            {

                if (month != 0)
                {
                    year = DateTime.Now.Year;
                    double ServiceRevenue = await context.Bills.Where(x => x.CreateDate.Value.Year == year && x.CreateDate.Value.Month == month).SumAsync(x => x.ServicePrice) ?? 0;
                    double TroublePrice = await context.Bills.Where(x => x.CreateDate.Value.Year == year && x.CreateDate.Value.Month == month).SumAsync(x => x.TroublePrice) ?? 0;
                    double TotalPrice = await context.Bills.Where(x => x.CreateDate.Value.Year == year && x.CreateDate.Value.Month == month).SumAsync(x => x.TotalPrice) ?? 0;
                    double RentalRevenue = TotalPrice - TroublePrice - ServiceRevenue;
                    return (ServiceRevenue, RentalRevenue);
                }
                else
                {
                    double ServiceRevenue = await context.Bills.Where(x => x.CreateDate.Value.Year == year).SumAsync(x => x.ServicePrice) ?? 0;
                    double TroublePrice = await context.Bills.Where(x => x.CreateDate.Value.Year == year).SumAsync(x => x.TroublePrice) ?? 0;
                    double TotalPrice = await context.Bills.Where(x => x.CreateDate.Value.Year == year).SumAsync(x=> x.TotalPrice) ?? 0;
                    double RentalRevenue = TotalPrice - TroublePrice - ServiceRevenue;
                    return ( ServiceRevenue,RentalRevenue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<double> MonthlyExpense, double ServiceExpense, double RepairCost, double FurnitureExpense, string ExpenseRate)> GetExpenseByYear(int year)
        {
            List<double> MonthlyExpense = new List<double>(new double[12]);
            double ServiceExpenseTotal = 0;
            double RepairCostTotal = 0;
            double FurnitureExpenseTotal = 0;

            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var MonthlyServiceExpense = await context.GoodsReceipts
                     .Where(b => b.CreateAt.Value.Year == year)
                     .GroupBy(b => b.CreateAt.Value.Month)
                     .Select(gr => new
                     {
                         Month = gr.Key,
                         Outcome = gr.Sum(b => (double?)b.ImportPrice * b.Quantity) ?? 0
                     }).ToListAsync();

                    var MonthlyFurnitureExpense = await context.FurnitureReceipts
                        .Where(x=> x.CreateAt.Value.Year == year)
                        .GroupBy(x=> x.CreateAt.Value.Month)
                         .Select(gr => new
                         {
                             Month = gr.Key,
                             Outcome = gr.Sum(b => (double?)b.ImportPrice * b.Quantity) ?? 0
                         }).ToListAsync();
                    var MonthRepairCostByCustomer = await context.TroubleByCustomers
                         .Where(t => t.Trouble.FinishDate != null && t.Trouble.FinishDate.Value.Year == year)
                         .GroupBy(t => t.Trouble.FinishDate.Value.Month)
                         .Select(gr =>
                         new
                         {
                             Month = gr.Key,
                             Outcome = gr.Sum(t => t.PredictedPrice) ?? 0
                         }).ToListAsync();
                    double[] MonthRepairCostByCustomerEx = new double[13];
                    for (int i = 0; i < 13; i++) MonthRepairCostByCustomerEx[i] = 0;
                    foreach (var item in MonthRepairCostByCustomer)
                    {
                        MonthRepairCostByCustomerEx[item.Month] = item.Outcome;
                    }
                    var MonthlyRepairCost = await context.Troubles
                         .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year)
                         .GroupBy(t => t.FinishDate.Value.Month)
                         .Select(gr =>
                         new
                         {
                             Month = gr.Key,
                             Outcome = (gr.Sum(t => t.Price)) 
                         }).ToListAsync();

                    

                    //Accumulate
                    foreach (var ex in MonthlyServiceExpense)
                    {
                        MonthlyExpense[ex.Month - 1] += (double?)ex.Outcome ?? 0;
                        ServiceExpenseTotal += (double?)ex.Outcome ?? 0;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        MonthlyExpense[ex.Month - 1] += (double)ex.Outcome - MonthRepairCostByCustomerEx[ex.Month];
                        RepairCostTotal += (double?)ex.Outcome ??  0 ;
                    }
                    foreach (var ex in MonthlyFurnitureExpense)
                    {
                        MonthlyExpense[ex.Month - 1] += (double?)ex.Outcome ?? 0;
                        FurnitureExpenseTotal += (double?)ex.Outcome ?? 0;
                    }

                    double lastExpenseTotal = await GetFullExpenseLastTime(context, year - 1);

                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                        if (ServiceExpenseTotal + RepairCostTotal + FurnitureExpenseTotal == 0) ExpenseRateStr = "-3";
                    }

                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ServiceExpenseTotal + RepairCostTotal + FurnitureExpenseTotal / lastExpenseTotal) - 1));
                    }


                    return (MonthlyExpense, (double)ServiceExpenseTotal, (double)RepairCostTotal, (double)FurnitureExpenseTotal,ExpenseRateStr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<double> GetFullExpenseLastTime(HotelManagementEntities context, int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    //Service Receipt
                    var LastYearServiceExpense = await context.GoodsReceipts
                             .Where(pr => pr.CreateAt != null && pr.CreateAt.Value.Year == year).ToListAsync();
                    double lastYearServiceExpense = 0;
                    foreach (var item in LastYearServiceExpense)
                    {
                        if (item != null)
                        {
                            if (item.ImportPrice != null && item.Quantity != null)
                            {
                                lastYearServiceExpense += (double)item.ImportPrice * (int)item.Quantity;
                            }
                        }
                        
                    }


                    //Repair Cost
                    var LastYearRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year).ToListAsync();
                    double lastYearRepairCost = 0;
                    foreach (var item in LastYearRepairCost)
                    {
                        if (item != null)
                        {
                            if (item.Price!=null)
                            {
                                lastYearRepairCost += (double)item.Price;
                            }
                        }

                    }


                    var LastYearRepairCostByCustomer = await context.TroubleByCustomers
                             .Where(tr => tr.Trouble.FinishDate != null && tr.Trouble.FinishDate.Value.Year == year).ToListAsync();
                    double lastYearRepairCostByCustomer = 0;
                    foreach (var item in LastYearRepairCostByCustomer)
                    {
                        if (item != null)
                        {
                            if (item.PredictedPrice!=null)
                            {
                                lastYearRepairCostByCustomer += (double)item.PredictedPrice;
                            }
                        }

                    }
                    var LastYearFurinitureExpenses = await context.FurnitureReceipts
                        .Where(fn => fn.CreateAt != null && fn.CreateAt.Value.Year == year).ToListAsync();
                    double lastYearFurinitureExpenses = 0;
                    foreach (var item in LastYearFurinitureExpenses)
                    {
                        if (item != null)
                        {
                            if (item.ImportPrice != null && item.Quantity != null)
                            {
                                lastYearFurinitureExpenses += (double)item.ImportPrice * (int)item.Quantity;
                            }
                        }

                    }

                    return (lastYearServiceExpense + lastYearRepairCost - lastYearRepairCostByCustomer + lastYearFurinitureExpenses);
                }
                else
                {
                    //Service Receipt
                    var LastMonthServiceExpense = await context.GoodsReceipts
                             .Where(pr => pr.CreateAt != null && pr.CreateAt.Value.Year == year && pr.CreateAt.Value.Month == month).ToListAsync();
                    double lastMonthServiceExpense = 0;
                    foreach (var item in LastMonthServiceExpense)
                    {
                        if (item != null)
                        {
                            if (item.ImportPrice != null && item.Quantity != null)
                            {
                                lastMonthServiceExpense += (double)item.ImportPrice * (int)item.Quantity;
                            }
                        }

                    }

                    //Repair Cost
                    var LastMonthRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year && tr.FinishDate.Value.Month == month).ToListAsync();

                    double lastMonthRepairCost = 0;
                    foreach (var item in LastMonthRepairCost)
                    {
                        if (item != null)
                        {
                            if (item.Price != null)
                            {
                                lastMonthRepairCost += (double)item.Price;
                            }
                        }

                    }


                    var LastMonthRepairCostByCustomer = await context.TroubleByCustomers
                             .Where(tr => tr.Trouble.FinishDate != null && tr.Trouble.FinishDate.Value.Year == year && tr.Trouble.FinishDate.Value.Month == month).ToListAsync();
                    double lastMonthRepairCostByCustomer = 0;
                    foreach (var item in LastMonthRepairCostByCustomer)
                    {
                        if (item != null)
                        {
                            if (item.PredictedPrice != null)
                            {
                                lastMonthRepairCostByCustomer += (double)item.PredictedPrice;
                            }
                        }

                    }

                    var LastMonthFurinitureExpenses = await context.FurnitureReceipts
                       .Where(fn => fn.CreateAt != null && fn.CreateAt.Value.Year == year && fn.CreateAt.Value.Month == month).ToListAsync();
                    double lastMonthFurinitureExpenses = 0;
                    foreach (var item in LastMonthFurinitureExpenses)
                    {
                        if (item != null)
                        {
                            if (item.ImportPrice != null && item.Quantity != null)
                            {
                                lastMonthFurinitureExpenses += (double)item.ImportPrice * (int)item.Quantity;
                            }
                        }

                    }
                    return (lastMonthServiceExpense + lastMonthRepairCost - lastMonthRepairCostByCustomer + lastMonthFurinitureExpenses);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<double>, double ServiceRevenue, double RentalRevenue, string RevenueRate)> GetRevenueByMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            List<double> DailyReveList = new List<double>(new double[days]);

            try
            {

                using (var context = new HotelManagementEntities())
                {
                    var billList = context.Bills
                     .Where(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month);

                    (double ServiceRevenue, double RentalRevenue) = await GetFullRevenue(context, year, month);

                    var dailyRevenue = await billList
                                .GroupBy(b => b.CreateDate.Value.Day)
                                 .Select(gr => new
                                 {
                                     Day = gr.Key,
                                     Income = gr.Sum(b => b.TotalPrice),
                                     DiscountPrice = gr.Sum(b => (double?)b.DiscountPrice) ?? 0,
                                 }).ToListAsync();

                    foreach (var re in dailyRevenue)
                    {
                        DailyReveList[re.Day - 1] = (double)re.Income;
                    }

                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }
                    (double lastServiceReve, double lastRentalReve) = await GetFullRevenue(context, year, month - 1);
                    double lastRevenueTotal = lastServiceReve + lastRentalReve;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                        if (ServiceRevenue + RentalRevenue == 0) RevenueRateStr = "-3";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ServiceRevenue + RentalRevenue) / lastRevenueTotal) - 1);
                    }
                    
                    return (DailyReveList, (double)ServiceRevenue, (double)RentalRevenue, RevenueRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<double> DailyExpense, double ServiceExpense, double RepairCost, double FurnitureExpense, string RepairRateStr)> GetExpenseByMonth(int year, int month)
        {

            int days = DateTime.DaysInMonth(year, month);
            List<double> DailyExpense = new List<double>(new double[days]);
            double ServiceExpenseTotal = 0;
            double RepairCostTotal = 0;
            double FurnitureExpenseTotal = 0;

            try
            {

                using (var context = new HotelManagementEntities())
                {
                    //Service Receipt
                    var MonthlyServiceExpense = await context.GoodsReceipts
                         .Where(b => b.CreateAt.Value.Year == year && b.CreateAt.Value.Month == month)
                         .GroupBy(b => b.CreateAt.Value.Day)
                         .Select(gr => new
                         {
                             Day = gr.Key,
                             Outcome = gr.Sum(b => (double?)b.ImportPrice * b.Quantity) ?? 0
                         }).ToListAsync();
                    //Repair Cost


                    var MonthRepairCostByCustomer = await context.TroubleByCustomers
                         .Where(t => t.Trouble.FinishDate != null && t.Trouble.FinishDate.Value.Year == year && t.Trouble.FinishDate.Value.Month == month)
                         .GroupBy(t => t.Trouble.FinishDate.Value.Day)
                         .Select(gr =>
                         new
                         {
                             Day = gr.Key,
                             Outcome = gr.Sum(t => t.PredictedPrice) ?? 0
                         }).ToListAsync();
                    double[] MonthRepairCostByCustomerEx = new double[32];
                    for (int i = 0; i < 32; i++) MonthRepairCostByCustomerEx[i] = 0;
                    foreach (var item in MonthRepairCostByCustomer)
                    {
                        MonthRepairCostByCustomerEx[item.Day] = item.Outcome;
                    }
                    var MonthlyRepairCost = await context.Troubles
                         .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year && t.FinishDate.Value.Month == month)
                         .GroupBy(t => t.FinishDate.Value.Day)
                         .Select(gr =>
                         new
                         {
                             Day = gr.Key,
                             Outcome = gr.Sum(t => (double?)t.Price) 
                         }).ToListAsync();

                    var MonthlyFurnitureExpense = await context.FurnitureReceipts
                        .Where(b => b.CreateAt.Value.Year == year && b.CreateAt.Value.Month == month)
                        .GroupBy(b => b.CreateAt.Value.Day)
                        .Select(gr => new
                        {
                            Day = gr.Key,
                            Outcome = gr.Sum(b => (double?)b.ImportPrice * b.Quantity) ?? 0
                        }).ToListAsync();

                    //context.
                    //Accumulate
                    foreach (var ex in MonthlyServiceExpense)
                    {
                        DailyExpense[ex.Day - 1] += (double?)ex.Outcome ?? 0;
                        ServiceExpenseTotal += ex.Outcome;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        DailyExpense[ex.Day - 1] += (double)ex.Outcome - MonthRepairCostByCustomerEx[ex.Day];
                        RepairCostTotal += (double?)ex.Outcome ?? 0;
                    }
                    foreach (var ex in MonthlyFurnitureExpense)
                    {
                        DailyExpense[ex.Day - 1] += (double?)ex.Outcome ?? 0;
                        FurnitureExpenseTotal += (double?)ex.Outcome ?? 0;
                    }
                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }


                    double lastProductExpenseTotal = await GetFullExpenseLastTime(context, year, month - 1);
                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastProductExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                        if (ServiceExpenseTotal + RepairCostTotal + FurnitureExpenseTotal == 0) ExpenseRateStr = "-3";
                    }
                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ServiceExpenseTotal + RepairCostTotal +FurnitureExpenseTotal / lastProductExpenseTotal) - 1));
                    }

                    return (DailyExpense, ServiceExpenseTotal, RepairCostTotal, FurnitureExpenseTotal, ExpenseRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<RoomTypeDTO>> GetListRoomTypeRevenue(int year, int month)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                        var list1 = await context.Bills.Where(x => x.CreateDate.Value.Year == year && x.CreateDate.Value.Month == month).ToListAsync();
                        var list2 = list1.GroupBy(b => b.RentalContract.Room.RoomType.RoomTypeName)
                            .Select(gr => new RoomTypeDTO
                            {
                                RoomTypeName = gr.First().RentalContract.Room.RoomType.RoomTypeName,
                                Revenue = (double)gr.Sum(x => x.TotalPrice - x.ServicePrice - x.TroublePrice)
                            }).ToList();
                        for (int i = 0; i < list2.Count; i++)
                        {
                            list2[i].STT = i + 1;
                        }
                        return list2;
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<ServiceTypeDTO>> GetListServiceTypeRevenue(int year, int month)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                        var listRentalContract = await context.Bills.Where(x => x.CreateDate.Value.Year == year && x.CreateDate.Value.Month == month).Select(x => x.RentalContractId).ToListAsync();

                        var list1 = await context.ServiceUsings.Where(x => listRentalContract.Contains(x.RentalContractId)).ToListAsync();
                        var list2 = list1.GroupBy(b => b.Service.ServiceType)
                            .Select(gr => new ServiceTypeDTO
                            {
                                ServiceType = gr.First().Service.ServiceType,
                                Revenue = (double)gr.Sum(x => x.Quantity * x.UnitPrice)
                            }).ToList();
                        for (int i = 0; i < list2.Count; i++)
                        {
                            list2[i].STT = i + 1;
                        }
                        return list2;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<SeriesCollection> GetDataRoomTypePieChart(int year, int month)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var listRoomType = await GetListRoomTypeRevenue(year, month);
                    SeriesCollection listRoomChart = new SeriesCollection();
                    foreach (var item in listRoomType)
                    {
                        PieSeries p = new PieSeries
                        {
                            Values = new ChartValues<double> { item.Revenue},
                            Title = item.RoomTypeName,
                        };
                        listRoomChart.Add(p);
                    }
                    return listRoomChart;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<SeriesCollection> GetDataServiceTypePieChart(int year, int month)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {

                    var listServiceType = await GetListServiceTypeRevenue(year, month);
                    SeriesCollection listRoomChart = new SeriesCollection();
                    foreach (var item in listServiceType)
                    {
                        PieSeries p = new PieSeries
                        {
                            Values = new ChartValues<double> { item.Revenue },
                            Title = item.ServiceType,
                        };
                        listRoomChart.Add(p);
                    }
                    return listRoomChart;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<string> GetListFilterYear()
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var listYear = context.Bills.Select(x => x.CreateDate.Value.Year).ToList();
                    if (listYear == null) listYear = new List<int>();
                    if (!listYear.Contains(DateTime.Now.Year))
                    {
                        listYear.Add(DateTime.Now.Year);
                    }
                    var listYearStr = listYear.Select(x => "Năm " + x.ToString()).ToList();
                    listYearStr.Reverse();
                    List<string> years = new List<string>();
                    foreach (var year in listYearStr)
                    {
                        if (!years.Contains(year)) years.Add(year);
                    }
                    return years;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
