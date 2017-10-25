using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class SaleIndentServices : ISaleIndentServices
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public SaleIndentServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public string CreateSaleIndent(SaleIndentEntity saleIndentEntity)
        {
            using (var scope = new TransactionScope())
            {
                string locationID = saleIndentEntity.DC_Code;
                SI_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                string locationId = autoIncNumber.DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                string prefix = rm.GetString("SI");
                int? incNumber = autoIncNumber.SI_Last_Number;
                string siNumber = prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);

                int? incrementedValue = incNumber + 1;

                var SIincrement = DB.SI_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                if (SIincrement != null)
                {
                    SIincrement.SI_Last_Number = incrementedValue;
                    _unitOfWork.SaleIndentAutoIncrementRepository.Update(SIincrement);
                    _unitOfWork.Save();

                }
                else
                {
                    return "false";
                }

                var saleIndent = new Sale_Indent
                {
                    SI_Number = siNumber,
                    DC_Code = saleIndentEntity.DC_Code,
                    SI_RLS_date = saleIndentEntity.SI_RLS_date,
                    Material_Source = saleIndentEntity.Material_Source,
                    Material_Source_id = saleIndentEntity.Material_Source_id,
                    Customer_Id = saleIndentEntity.Customer_Id,
                    Customer_Code = saleIndentEntity.Customer_Code,
                    Customer_name = saleIndentEntity.Customer_name,
                    SI_Type = saleIndentEntity.SI_Type,
                    Delivery_Date = saleIndentEntity.Delivery_Date,
                    SI_raised_by = saleIndentEntity.SI_raised_by,
                    CreatedDate = DateTime.Now,
                    SI_Approval_Flag = false,
                    CreatedBy = saleIndentEntity.CreatedBy,
                    is_Deleted = false
                };

                _unitOfWork.SaleIndentRepository.Insert(saleIndent);
                _unitOfWork.Save();


                string gsiNumber = saleIndent.SI_Number;
                int? siId = saleIndent.SI_id;

                var model = new SI_Line_item();
                foreach (SI_LineItems_Entity sSub in saleIndentEntity.LineItems)
                {
                    model.SI_Number = gsiNumber;
                    model.SI_id = siId;
                    model.SKU_ID = sSub.SKU_ID;
                    model.SKU_Code = sSub.SKU_Code;
                    model.SKU_Name = sSub.SKU_Name;
                    model.UOM = sSub.UOM;
                    model.Indent_Qty = sSub.Indent_Qty;
                    model.Material_Type = sSub.Material_Type;
                    model.Total_Qty = sSub.Total_Qty;
                    model.Remark = sSub.Remark;
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = sSub.CreatedBy;

                    _unitOfWork.SaleIndentSubRepository.Insert(model);
                    _unitOfWork.Save();

                }                

                scope.Complete();
                return saleIndent.SI_Number;
            }
        }

        public SI_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.SI_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new SI_NUM_Generation
            {
                SI_Num_Gen_Id = autoinc.SI_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                SI_Last_Number = autoinc.SI_Last_Number
            };

            return model;
        }


    }
}
