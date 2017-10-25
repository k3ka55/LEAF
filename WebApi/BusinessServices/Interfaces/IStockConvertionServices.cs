using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface IStockConvertionServices
    {
        List<StockEntity> getStocks(DateTime date, String dc_Code);
        bool Convert_Stock(StockEntity stkEntity);
        List<StockEntity> getDatewiseStocks(DateTime date, String dc_Code);
        List<StockConvertionEntity> StockConvertedSummary(int dcid, DateTime createdDate);
    }
}
