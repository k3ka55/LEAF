#region Using Namespaces...

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity.Validation;
using DataModel.GenericRepository;

#endregion

namespace DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private LEAFDBEntities _context = null;
        private GenericRepository<Customer_Registration> _customerregistrationRepository;
        private GenericRepository<User_Details> _userRepository;
        //private GenericRepository<Role_Access> _menuRepository;
        private GenericRepository<Menu_Master> _menulistRepository;
        private GenericRepository<SKU_Master> _skuRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<Supplier> _supplierRepository;
        private GenericRepository<DC_Master> _locationRepository;
        private GenericRepository<Role_Master> _roleRepository;
        private GenericRepository<Location_Master> _locationMaster;
        private GenericRepository<Role_Menu_Access> _roleMenuAccess;
        private GenericRepository<Purchase_Order> _purchaseOrderRepository;
        private GenericRepository<Purchase_Order_Line_item> _purchaseSubRepository;
        private GenericRepository<PO_Rate_Change_Audit> _poRateRepository;
        private GenericRepository<Country> _countryRepository;
        private GenericRepository<Stock_Transfer_Indent> _stockTransferRepository;
        private GenericRepository<STI_Line_item> _stockSubRepository;
        private GenericRepository<CSI_Line_item> _saleSubRepository;
        private GenericRepository<CSI_NUM_Generation> _CSIautoIncrementRepository;
        private GenericRepository<Customer_Sale_Indent> _saleRepository;
        private GenericRepository<PO_NUM_Generation> _autoIncrementRepository;
        private GenericRepository<ST_NUM_Generation> _STautoIncrementRepository;
        private GenericRepository<Wastage_Auto_Num_Gen> _wastageNumIncrementRepository;

        private GenericRepository<GRN_NUM_Generation> _grnautoIncrementRepository;
        private GenericRepository<GRN_Creation> _grnRepository;
        private GenericRepository<GRN_Line_item> _grnSubRepository;
        private GenericRepository<DC_Customer_Mapping> _customerSubRepository;
        private GenericRepository<SKU_Main_Sub_Mapping> _skuMappingRepository;
        private GenericRepository<DC_Supplier_Mapping> _supplierSubRepository;
        private GenericRepository<Wastage_Line_item> _wastageSubRepository;
        private GenericRepository<Physical_Stock> _physicalstockRepository;
        //-------------------Dispatch Repositories-------------------
        private GenericRepository<Dispatch_Creation> _dispatchCreationRepository;
        private GenericRepository<Dispatch_Line_item> _dispatchSubRepository;
        private GenericRepository<Customer_Dispatch_Num_Gen> _custdispatchNumGenRepository;
        private GenericRepository<Stock_Xfer_Dispatch_Num_Gen> _stockxferNumGenRepository;
        private GenericRepository<Dispatch_Supplier_Track> _dispatchSupplierTrackRepository;
        private GenericRepository<Stock_Transaction> _stockTransactionRepository;
        private GenericRepository<Invoice_Creation> _invoiceCreationRepository;
        private GenericRepository<Invoice_Line_item> _invoiceLineItemsRepository;
        private GenericRepository<Invoice_NUM_Generation> _invoiceAutoNumGenRepository;
        private GenericRepository<Wastage_Creation> _wastageRepository;
        private GenericRepository<Wastage_Supplier_Info> _wastageSupplierRepository;
        private GenericRepository<Printed_Barcode_Details> _printbarcodeRepository;
        private GenericRepository<Multiple_CSI_tracking> _mulitipleTrackingCsiRepository;

        private GenericRepository<Label_Fields> _labelRepository;
        private GenericRepository<Customer_Label_Template> _customerLabelTemplate;
        private GenericRepository<Customer_LabelTemplate_Mapping> _customerLabelTemplateMapping;
        private GenericRepository<Customer_SKU_Mapping> _customerSKUMapping;
        //-------------------Stock Rpository---------------------------
        private GenericRepository<Stock> _stockRepository;
        private GenericRepository<Stock_Code_Num_Gen> _stockNumGenRepository;

        private GenericRepository<Customer_Label_Auto_Num_Gen> _customerLabel_NUMRepository;

        //------------------stock convertion repository----------------
        private GenericRepository<Stock_Convertion_Tracking> _stockConvertion_Repository;

        //------------------location master----------------------------
        private GenericRepository<Location_Master> _location_Reposiory;
        private GenericRepository<User_Location_Access> _user_location_Repositpory;

        //-----------------rate and customer indent template --------------------
        private GenericRepository<Rate_Template> _rateTemplateRepository;
        private GenericRepository<Rate_Template_Line_item> _rateTemplateLineItemRepository;
        private GenericRepository<Customer_Indent> _customerIndentRepository;
        private GenericRepository<Customer_Indent_Line_item> _customerIndentLineItemsRepository;
        private GenericRepository<Customer_Rate_Template_Mapping> _customerIndentMappingRepository;
        private GenericRepository<Region_Master> _regionRepository;
        private GenericRepository<Material_Master> _materialRepository;
        private GenericRepository<Supplier_Code_Num_Gen> _supplierNumGenRepository;
        private GenericRepository<Customer_Code_Num_Gen> _customerNumGenRepository;
        private GenericRepository<DeliveryAddress> _delieveryAddressRepository;
        private GenericRepository<Customer_Indent_Num_Gen> _CustomerIndentNumRepository;
        private GenericRepository<Rate_Template_Num_Gen> _RateTemplateNumRepository;
        private GenericRepository<Material_Master_Num_Gen> _MaterialMasterNumGenRepository;
        private GenericRepository<Region_Master> _regionMasterRepository;
        private GenericRepository<Role_User_Access> _roleUserAccessRepository;
        private GenericRepository<User_DC_Access> _user_dc_Repositpory;
        private GenericRepository<GRN_SKU_Line_Items> _grnSKULineItemRepository;
        private GenericRepository<Dispatch_SKU_Line_Items> _dispatchSKULineItemRepository;
        private GenericRepository<Dispatch_Consumables> _dispatchConsumablesRepository;
       private GenericRepository<GRN_Consumables> _grnConsumablesRepository;
       private GenericRepository<Customer_Indent_Template_Mapping> _customerIndentTemplateMappingRepository;
       private GenericRepository<TALLY_MAPPING> _tallyMappingRepository;
       private GenericRepository<Route_Master> _routeRepository;
       private GenericRepository<Route_Master_Auto_Num_Gen> _routenumGenRepository;
       private GenericRepository<Sales_Route_Mapping> _salesrouteMappingRepository;
       private GenericRepository<Strinkage_Stock_Adj_Num_Gen> _strinkageStockAdjNumGenRepository;
       private GenericRepository<Strinkage_Stock_Adjustement> _strinkageStockAdjRepository;
       private GenericRepository<Stirnkage_Summary> _strinkagesummaryRepository;
       private GenericRepository<Route_Area_Master> _routeareaRepository;
       private GenericRepository<Estimated_Stock> _estimatedstockRepository;
       private GenericRepository<Estimated_Stock_NUM_Generation> _estimatedstocknumgenRepository;
       private GenericRepository<Invoice_Collection> _invoicecollectionRepository;
       private GenericRepository<Invoice_Collection_Tracking> _invoicecollectiontrackingRepository;
       private GenericRepository<Invoice_Collection_Excel_Tracking> _invoicecollectionExceltrackingRepository;
       private GenericRepository<Invoice_Collection_Num_Gen> _invoicecollectionnumgenRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new LEAFDBEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        /// 
        public GenericRepository<Invoice_Collection_Excel_Tracking> InvoiceCollectionExcelTrackingRepository
        {
            get
            {
                if (this._invoicecollectionExceltrackingRepository == null)
                    this._invoicecollectionExceltrackingRepository = new GenericRepository<Invoice_Collection_Excel_Tracking>(_context);
                return _invoicecollectionExceltrackingRepository;
            }
        }
        public GenericRepository<Invoice_Collection_Tracking> InvoiceCollectionTrackingRepository
        {
            get
            {
                if (this._invoicecollectiontrackingRepository == null)
                    this._invoicecollectiontrackingRepository = new GenericRepository<Invoice_Collection_Tracking>(_context);
                return _invoicecollectiontrackingRepository;
            }
        }
        public GenericRepository<Invoice_Collection_Num_Gen> InvoiceCollectionNumGenRepository
        {
            get
            {
                if (this._invoicecollectionnumgenRepository == null)
                    this._invoicecollectionnumgenRepository = new GenericRepository<Invoice_Collection_Num_Gen>(_context);
                return _invoicecollectionnumgenRepository;
            }
        }
        public GenericRepository<Invoice_Collection> InvoiceCollectionRepository
        {
            get
            {
                if (this._invoicecollectionRepository == null)
                    this._invoicecollectionRepository = new GenericRepository<Invoice_Collection>(_context);
                return _invoicecollectionRepository;
            }
        }
        public GenericRepository<Estimated_Stock_NUM_Generation> EstimatedStockNumGenRepository
        {
            get
            {
                if (this._estimatedstocknumgenRepository == null)
                    this._estimatedstocknumgenRepository = new GenericRepository<Estimated_Stock_NUM_Generation>(_context);
                return _estimatedstocknumgenRepository;
            }
        }
        public GenericRepository<Estimated_Stock> EstimatedStockRepository
        {
            get
            {
                if (this._estimatedstockRepository == null)
                    this._estimatedstockRepository = new GenericRepository<Estimated_Stock>(_context);
                return _estimatedstockRepository;
            }
        }
        public GenericRepository<Customer_Registration> CustomerRegistrationRepository
        {
            get
            {
                if (this._customerregistrationRepository == null)
                    this._customerregistrationRepository = new GenericRepository<Customer_Registration>(_context);
                return _customerregistrationRepository;
            }
        }
        public GenericRepository<Route_Area_Master> RouteAreaRepository
        {
            get
            {
                if (this._routeareaRepository == null)
                    this._routeareaRepository = new GenericRepository<Route_Area_Master>(_context);
                return _routeareaRepository;
            }
        }
        public GenericRepository<Stirnkage_Summary> StrinkagesummaryRepository
        {
            get
            {
                if (this._strinkagesummaryRepository == null)
                    this._strinkagesummaryRepository = new GenericRepository<Stirnkage_Summary>(_context);
                return _strinkagesummaryRepository;
            }
        }
        public GenericRepository<Strinkage_Stock_Adjustement> StrinkageStockAdjRepository
        {
            get
            {
                if (this._strinkageStockAdjRepository == null)
                    this._strinkageStockAdjRepository = new GenericRepository<Strinkage_Stock_Adjustement>(_context);
                return _strinkageStockAdjRepository;
            }
        }
        public GenericRepository<Strinkage_Stock_Adj_Num_Gen> StrinkageStockAdjNumGenRepository
        {
            get
            {
                if (this._strinkageStockAdjNumGenRepository == null)
                    this._strinkageStockAdjNumGenRepository = new GenericRepository<Strinkage_Stock_Adj_Num_Gen>(_context);
                return _strinkageStockAdjNumGenRepository;
            }
        }
        public GenericRepository<Sales_Route_Mapping> SalesRouteMappingRepository
        {
            get
            {
                if (this._salesrouteMappingRepository == null)
                    this._salesrouteMappingRepository = new GenericRepository<Sales_Route_Mapping>(_context);
                return _salesrouteMappingRepository;
            }
        }

        public GenericRepository<Route_Master> RouteRepository
        {
            get
            {
                if (this._routeRepository == null)
                    this._routeRepository = new GenericRepository<Route_Master>(_context);
                return _routeRepository;
            }
        }
        public GenericRepository<Route_Master_Auto_Num_Gen> RouteMasterNumGenRepository
        {
            get
            {
                if (this._routenumGenRepository == null)
                    this._routenumGenRepository = new GenericRepository<Route_Master_Auto_Num_Gen>(_context);
                return _routenumGenRepository;
            }
        }

        public GenericRepository<Customer_Indent_Template_Mapping> CustomerIndentTemplateMappingRepository
        {
            get
            {
                if (this._customerIndentTemplateMappingRepository == null)
                    this._customerIndentTemplateMappingRepository = new GenericRepository<Customer_Indent_Template_Mapping>(_context);
                return _customerIndentTemplateMappingRepository;
            }
        }
        public GenericRepository<GRN_Consumables> GrnConsumablesRepository
        {
            get
            {
                if (this._grnConsumablesRepository == null)
                    this._grnConsumablesRepository = new GenericRepository<GRN_Consumables>(_context);
                return _grnConsumablesRepository;
            }
        }
        public GenericRepository<Multiple_CSI_tracking> MultipleTrackingCSIRepository
        {
            get
            {
                if (this._mulitipleTrackingCsiRepository == null)
                    this._mulitipleTrackingCsiRepository = new GenericRepository<Multiple_CSI_tracking>(_context);
                return _mulitipleTrackingCsiRepository;
            }
        }

        public GenericRepository<Dispatch_Consumables> DispatchConsumablesRepository
        {
            get
            {
                if (this._dispatchConsumablesRepository == null)
                    this._dispatchConsumablesRepository = new GenericRepository<Dispatch_Consumables>(_context);
                return _dispatchConsumablesRepository;
            }
        }

        public GenericRepository<Dispatch_SKU_Line_Items> DispatchSKULineItemRepository
        {
            get
            {
                if (this._dispatchSKULineItemRepository == null)
                    this._dispatchSKULineItemRepository = new GenericRepository<Dispatch_SKU_Line_Items>(_context);
                return _dispatchSKULineItemRepository;
            }
        }

        public GenericRepository<GRN_SKU_Line_Items> GrnSKULineItemRepository
        {
            get
            {
                if (this._grnSKULineItemRepository == null)
                    this._grnSKULineItemRepository = new GenericRepository<GRN_SKU_Line_Items>(_context);
                return _grnSKULineItemRepository;
            }
        }


        public GenericRepository<Region_Master> RegionMasterRepository
        {
            get
            {
                if (this._regionMasterRepository == null)
                    this._regionMasterRepository = new GenericRepository<Region_Master>(_context);
                return _regionMasterRepository;
            }
        }
        public GenericRepository<Role_User_Access> RoleUserAccessRepository
        {
            get
            {
                if (this._roleUserAccessRepository == null)
                    this._roleUserAccessRepository = new GenericRepository<Role_User_Access>(_context);
                return _roleUserAccessRepository;
            }
        }

        public GenericRepository<User_DC_Access> UserDCMappingRepository
        {
            get
            {
                if (this._user_dc_Repositpory == null)
                    this._user_dc_Repositpory = new GenericRepository<User_DC_Access>(_context);
                return _user_dc_Repositpory;
            }
        }
        public GenericRepository<DeliveryAddress> DelieveryAddressRepository
        {
            get
            {
                if (this._delieveryAddressRepository == null)
                    this._delieveryAddressRepository = new GenericRepository<DeliveryAddress>(_context);
                return _delieveryAddressRepository;
            }
        }

        public GenericRepository<Rate_Template_Num_Gen> RateTemplateNumGenRepository
        {
            get
            {
                if (this._RateTemplateNumRepository == null)
                    this._RateTemplateNumRepository = new GenericRepository<Rate_Template_Num_Gen>(_context);
                return _RateTemplateNumRepository;
            }
        }

        public GenericRepository<Material_Master_Num_Gen> MaterialMasterNumGenRepository
        {
            get
            {
                if (this._MaterialMasterNumGenRepository == null)
                    this._MaterialMasterNumGenRepository = new GenericRepository<Material_Master_Num_Gen>(_context);
                return _MaterialMasterNumGenRepository;
            }
        }


        public GenericRepository<Customer_Indent_Num_Gen> CustomerIndentNumGenRepository
        {
            get
            {
                if (this._CustomerIndentNumRepository == null)
                    this._CustomerIndentNumRepository = new GenericRepository<Customer_Indent_Num_Gen>(_context);
                return _CustomerIndentNumRepository;
            }
        }

        public GenericRepository<Customer_Code_Num_Gen> CustomerNumGenRepository
        {
            get
            {
                if (this._customerNumGenRepository == null)
                    this._customerNumGenRepository = new GenericRepository<Customer_Code_Num_Gen>(_context);
                return _customerNumGenRepository;
            }
        }

        public GenericRepository<Supplier_Code_Num_Gen> SupplierNumGenRepository
        {
            get
            {
                if (this._supplierNumGenRepository == null)
                    this._supplierNumGenRepository = new GenericRepository<Supplier_Code_Num_Gen>(_context);
                return _supplierNumGenRepository;
            }
        }

        public GenericRepository<Material_Master> MaterialMasterRepository
        {
            get
            {
                if (this._materialRepository == null)
                    this._materialRepository = new GenericRepository<Material_Master>(_context);
                return _materialRepository;
            }
        }

        public GenericRepository<Location_Master> LocationMasterRepository
        {
            get
            {
                if (this._locationMaster == null)
                    this._locationMaster = new GenericRepository<Location_Master>(_context);
                return _locationMaster;
            }
        }

        public GenericRepository<Role_Menu_Access> RoleMenuAccessRepository
        {
            get
            {
                if (this._roleMenuAccess == null)
                    this._roleMenuAccess = new GenericRepository<Role_Menu_Access>(_context);
                return _roleMenuAccess;
            }
        }

        public GenericRepository<Region_Master> RegionRepository
        {
            get
            {
                if (this._regionRepository == null)
                    this._regionRepository = new GenericRepository<Region_Master>(_context);
                return _regionRepository;
            }
        }

        public GenericRepository<Customer_Rate_Template_Mapping> CustomerIndentMappingRepository
        {
            get
            {
                if (this._customerIndentMappingRepository == null)
                    this._customerIndentMappingRepository = new GenericRepository<Customer_Rate_Template_Mapping>(_context);
                return _customerIndentMappingRepository;
            }
        }
        public GenericRepository<Customer_Indent_Line_item> CustomerIndentLineItemRepository
        {
            get
            {
                if (this._customerIndentLineItemsRepository == null)
                    this._customerIndentLineItemsRepository = new GenericRepository<Customer_Indent_Line_item>(_context);
                return _customerIndentLineItemsRepository;
            }
        }
        public GenericRepository<Customer_Indent> CustomerIndentRepository
        {
            get
            {
                if (this._customerIndentRepository == null)
                    this._customerIndentRepository = new GenericRepository<Customer_Indent>(_context);
                return _customerIndentRepository;
            }
        }
        public GenericRepository<Rate_Template_Line_item> RateTemplateLineItemRepository
        {
            get
            {
                if (this._rateTemplateLineItemRepository == null)
                    this._rateTemplateLineItemRepository = new GenericRepository<Rate_Template_Line_item>(_context);
                return _rateTemplateLineItemRepository;
            }
        }
        public GenericRepository<Rate_Template> RateTemplateRepository
        {
            get
            {
                if (this._rateTemplateRepository == null)
                    this._rateTemplateRepository = new GenericRepository<Rate_Template>(_context);
                return _rateTemplateRepository;
            }
        }

        public GenericRepository<Location_Master> LocationsMasterRepository
        {
            get
            {
                if (this._location_Reposiory == null)
                    this._location_Reposiory = new GenericRepository<Location_Master>(_context);
                return _location_Reposiory;
            }
        }

        public GenericRepository<User_Location_Access> UserLocationMappingRepository
        {
            get
            {
                if (this._user_location_Repositpory == null)
                    this._user_location_Repositpory = new GenericRepository<User_Location_Access>(_context);
                return _user_location_Repositpory;
            }
        }

        public GenericRepository<Invoice_Creation> InvoiceRepository
        {
            get
            {
                if (this._invoiceCreationRepository == null)
                    this._invoiceCreationRepository = new GenericRepository<Invoice_Creation>(_context);
                return _invoiceCreationRepository;
            }
        }

        public GenericRepository<Stock_Convertion_Tracking> StockConvertionRepository
        {
            get
            {
                if (this._stockConvertion_Repository == null)
                    this._stockConvertion_Repository = new GenericRepository<Stock_Convertion_Tracking>(_context);
                return _stockConvertion_Repository;
            }
        }

        public GenericRepository<Label_Fields> LabelRepository
        {
            get
            {
                if (this._labelRepository == null)
                    this._labelRepository = new GenericRepository<Label_Fields>(_context);
                return _labelRepository;
            }
        }
        public GenericRepository<Customer_Label_Template> CustomerLabelTemplateRepository
        {
            get
            {
                if (this._customerLabelTemplate == null)
                    this._customerLabelTemplate = new GenericRepository<Customer_Label_Template>(_context);
                return _customerLabelTemplate;
            }
        }
        public GenericRepository<Customer_LabelTemplate_Mapping> CustomerLabelTemplateMappingRepository
        {
            get
            {
                if (this._customerLabelTemplateMapping == null)
                    this._customerLabelTemplateMapping = new GenericRepository<Customer_LabelTemplate_Mapping>(_context);
                return _customerLabelTemplateMapping;
            }
        }

        public GenericRepository<Printed_Barcode_Details> PrintBarcodeRepository
        {
            get
            {
                if (this._printbarcodeRepository == null)
                    this._printbarcodeRepository = new GenericRepository<Printed_Barcode_Details>(_context);
                return _printbarcodeRepository;
            }
        }
        public GenericRepository<Customer_SKU_Mapping> CustomerSKUMappingRepository
        {
            get
            {
                if (this._customerSKUMapping == null)
                    this._customerSKUMapping = new GenericRepository<Customer_SKU_Mapping>(_context);
                return _customerSKUMapping;
            }
        }

        public GenericRepository<Invoice_Line_item> InvoiceSubRepository
        {
            get
            {
                if (this._invoiceLineItemsRepository == null)
                    this._invoiceLineItemsRepository = new GenericRepository<Invoice_Line_item>(_context);
                return _invoiceLineItemsRepository;
            }
        }

        public GenericRepository<Invoice_NUM_Generation> InvoiceAutoIncrementRepository
        {
            get
            {
                if (this._invoiceAutoNumGenRepository == null)
                    this._invoiceAutoNumGenRepository = new GenericRepository<Invoice_NUM_Generation>(_context);
                return _invoiceAutoNumGenRepository;
            }
        }

        public GenericRepository<Dispatch_Creation> DispatchRepository
        {
            get
            {
                if (this._dispatchCreationRepository == null)
                    this._dispatchCreationRepository = new GenericRepository<Dispatch_Creation>(_context);
                return _dispatchCreationRepository;
            }
        }

        public GenericRepository<Dispatch_Line_item> DispatchSubRepository
        {
            get
            {
                if (this._dispatchSubRepository == null)
                    this._dispatchSubRepository = new GenericRepository<Dispatch_Line_item>(_context);
                return _dispatchSubRepository;
            }
        }

        public GenericRepository<Customer_Dispatch_Num_Gen> CustomerDispatchAutoIncrementRepository
        {
            get
            {
                if (this._custdispatchNumGenRepository == null)
                    this._custdispatchNumGenRepository = new GenericRepository<Customer_Dispatch_Num_Gen>(_context);
                return _custdispatchNumGenRepository;
            }
        }

        public GenericRepository<Stock_Xfer_Dispatch_Num_Gen> StockXferDispatchAutoIncrementRepository
        {
            get
            {
                if (this._stockxferNumGenRepository == null)
                    this._stockxferNumGenRepository = new GenericRepository<Stock_Xfer_Dispatch_Num_Gen>(_context);
                return _stockxferNumGenRepository;
            }
        }

        public GenericRepository<Dispatch_Supplier_Track> DispatchSupplierTrackRepository
        {
            get
            {
                if (this._dispatchSupplierTrackRepository == null)
                    this._dispatchSupplierTrackRepository = new GenericRepository<Dispatch_Supplier_Track>(_context);
                return _dispatchSupplierTrackRepository;
            }
        }


        public GenericRepository<Stock_Transaction> StockTransationRepository
        {
            get
            {
                if (this._stockTransactionRepository == null)
                    this._stockTransactionRepository = new GenericRepository<Stock_Transaction>(_context);
                return _stockTransactionRepository;
            }
        }

        public GenericRepository<Stock> StockRepository
        {
            get
            {
                if (this._stockRepository == null)
                    this._stockRepository = new GenericRepository<Stock>(_context);
                return _stockRepository;
            }
        }

        public GenericRepository<Stock_Code_Num_Gen> StockAutoIncrementRepository
        {
            get
            {
                if (this._stockNumGenRepository == null)
                    this._stockNumGenRepository = new GenericRepository<Stock_Code_Num_Gen>(_context);
                return _stockNumGenRepository;
            }
        }


        public GenericRepository<User_Details> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User_Details>(_context);
                return _userRepository;
            }
        }
        public GenericRepository<ST_NUM_Generation> ST_NUMRepository
        {
            get
            {
                if (this._STautoIncrementRepository == null)
                    this._STautoIncrementRepository = new GenericRepository<ST_NUM_Generation>(_context);
                return _STautoIncrementRepository;
            }
        }

        public GenericRepository<SKU_Main_Sub_Mapping> SkuMappingRepository
        {
            get
            {
                if (this._skuMappingRepository == null)
                    this._skuMappingRepository = new GenericRepository<SKU_Main_Sub_Mapping>(_context);
                return _skuMappingRepository;
            }
        }

        public GenericRepository<DC_Customer_Mapping> CustomerSubRepository
        {
            get
            {
                if (this._customerSubRepository == null)
                    this._customerSubRepository = new GenericRepository<DC_Customer_Mapping>(_context);
                return _customerSubRepository;
            }
        }

        public GenericRepository<DC_Supplier_Mapping> SupplierSubRepository
        {
            get
            {
                if (this._supplierSubRepository == null)
                    this._supplierSubRepository = new GenericRepository<DC_Supplier_Mapping>(_context);
                return _supplierSubRepository;
            }
        }

        public GenericRepository<STI_Line_item> StockSubRepository
        {
            get
            {
                if (this._stockSubRepository == null)
                    this._stockSubRepository = new GenericRepository<STI_Line_item>(_context);
                return _stockSubRepository;
            }
        }

        public GenericRepository<Stock_Transfer_Indent> StockTransferRepository
        {
            get
            {
                if (this._stockTransferRepository == null)
                    this._stockTransferRepository = new GenericRepository<Stock_Transfer_Indent>(_context);
                return _stockTransferRepository;
            }
        }
        public GenericRepository<Customer_Sale_Indent> SaleRepository
        {
            get
            {
                if (this._saleRepository == null)
                    this._saleRepository = new GenericRepository<Customer_Sale_Indent>(_context);
                return _saleRepository;
            }
        }

        public GenericRepository<Customer_Label_Auto_Num_Gen> CustomerLabel_NUMRepository
        {
            get
            {
                if (this._customerLabel_NUMRepository == null)
                    this._customerLabel_NUMRepository = new GenericRepository<Customer_Label_Auto_Num_Gen>(_context);
                return _customerLabel_NUMRepository;
            }
        }

        public GenericRepository<CSI_NUM_Generation> CSI_NUMRepository
        {
            get
            {
                if (this._CSIautoIncrementRepository == null)
                    this._CSIautoIncrementRepository = new GenericRepository<CSI_NUM_Generation>(_context);
                return _CSIautoIncrementRepository;
            }
        }
        public GenericRepository<CSI_Line_item> SaleSubRepository
        {
            get
            {
                if (this._saleSubRepository == null)
                    this._saleSubRepository = new GenericRepository<CSI_Line_item>(_context);
                return _saleSubRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        //public GenericRepository<Role_Access> MenuRepository
        //{
        //    get
        //    {
        //        if (this._menuRepository == null)
        //            this._menuRepository = new GenericRepository<Role_Access>(_context);
        //        return _menuRepository;
        //    }
        //}

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Menu_Master> MenuListRepository
        {
            get
            {
                if (this._menulistRepository == null)
                    this._menulistRepository = new GenericRepository<Menu_Master>(_context);
                return _menulistRepository;
            }
        }

        public GenericRepository<SKU_Master> SKURepository
        {
            get
            {
                if (this._skuRepository == null)
                    this._skuRepository = new GenericRepository<SKU_Master>(_context);
                return _skuRepository;
            }
        }

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                    this._customerRepository = new GenericRepository<Customer>(_context);
                return _customerRepository;
            }
        }
        public GenericRepository<TALLY_MAPPING> TallyMappingRepository
        {
            get
            {
                if (this._tallyMappingRepository == null)
                    this._tallyMappingRepository = new GenericRepository<TALLY_MAPPING>(_context);
                return _tallyMappingRepository;
            }
        }

        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this._supplierRepository == null)
                    this._supplierRepository = new GenericRepository<Supplier>(_context);
                return _supplierRepository;
            }
        }


        public GenericRepository<DC_Master> LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                    this._locationRepository = new GenericRepository<DC_Master>(_context);
                return _locationRepository;
            }
        }

        public GenericRepository<Role_Master> RoleRepository
        {
            get
            {
                if (this._roleRepository == null)
                    this._roleRepository = new GenericRepository<Role_Master>(_context);
                return _roleRepository;
            }
        }
        public GenericRepository<Wastage_Creation> WastageRepository
        {
            get
            {
                if (this._wastageRepository == null)
                    this._wastageRepository = new GenericRepository<Wastage_Creation>(_context);
                return _wastageRepository;
            }
        }
        public GenericRepository<Wastage_Supplier_Info> WastageSupplierRepository
        {
            get
            {
                if (this._wastageSupplierRepository == null)
                    this._wastageSupplierRepository = new GenericRepository<Wastage_Supplier_Info>(_context);
                return _wastageSupplierRepository;
            }
        }
        public GenericRepository<Wastage_Line_item> WastageSubRepository
        {
            get
            {
                if (this._wastageSubRepository == null)
                    this._wastageSubRepository = new GenericRepository<Wastage_Line_item>(_context);
                return _wastageSubRepository;
            }
        }
        public GenericRepository<Physical_Stock> PhysicalStockRepository
        {
            get
            {
                if (this._physicalstockRepository == null)
                    this._physicalstockRepository = new GenericRepository<Physical_Stock>(_context);
                return _physicalstockRepository;
            }
        }

        public GenericRepository<Purchase_Order> PurchaseOrderRepository
        {
            get
            {
                if (this._purchaseOrderRepository == null)
                    this._purchaseOrderRepository = new GenericRepository<Purchase_Order>(_context);
                return _purchaseOrderRepository;
            }
        }
        public GenericRepository<PO_Rate_Change_Audit> PORateAuditRepository
        {
            get
            {
                if (this._poRateRepository == null)
                    this._poRateRepository = new GenericRepository<PO_Rate_Change_Audit>(_context);
                return _poRateRepository;
            }
        }

        public GenericRepository<Purchase_Order_Line_item> PurchaseSubRepository
        {
            get
            {
                if (this._purchaseSubRepository == null)
                    this._purchaseSubRepository = new GenericRepository<Purchase_Order_Line_item>(_context);
                return _purchaseSubRepository;
            }
        }

        public GenericRepository<GRN_Creation> GrnRepository
        {
            get
            {
                if (this._grnRepository == null)
                    this._grnRepository = new GenericRepository<GRN_Creation>(_context);
                return _grnRepository;
            }
        }

        public GenericRepository<GRN_Line_item> GrnSubRepository
        {
            get
            {
                if (this._grnSubRepository == null)
                    this._grnSubRepository = new GenericRepository<GRN_Line_item>(_context);
                return _grnSubRepository;
            }
        }

        public GenericRepository<Country> CountryRepository
        {
            get
            {
                if (this._countryRepository == null)
                    this._countryRepository = new GenericRepository<Country>(_context);
                return _countryRepository;
            }
        }

        public GenericRepository<Wastage_Auto_Num_Gen> WastageNumIncrementRepository
        {
            get
            {
                if (this._wastageNumIncrementRepository == null)
                    this._wastageNumIncrementRepository = new GenericRepository<Wastage_Auto_Num_Gen>(_context);
                return _wastageNumIncrementRepository;
            }
        }
       

        public GenericRepository<PO_NUM_Generation> AutoIncrementRepository
        {
            get
            {
                if (this._autoIncrementRepository == null)
                    this._autoIncrementRepository = new GenericRepository<PO_NUM_Generation>(_context);
                return _autoIncrementRepository;
            }
        }

        public GenericRepository<GRN_NUM_Generation> AutoIncrementRepositoryGrn
        {
            get
            {
                if (this._grnautoIncrementRepository == null)
                    this._grnautoIncrementRepository = new GenericRepository<GRN_NUM_Generation>(_context);
                return _grnautoIncrementRepository;
            }
        }

        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}