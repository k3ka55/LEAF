//using System;
//using System.Data.Entity;
//using System.Globalization;
//using System.Web.Mvc;

//public class DecimalModelBinder : I
//{

//    public DbSet<P> Metrics { get; set; }

//    protected override void OnModelCreating(DbModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<PurchaseSubEntity>().Property(x => x.PPM).HasPrecision(4, 3);
//    }


//   }

//using System;
//using System.Globalization;
//using System.Web.Mvc;

//public class DecimalModelBinder : IModelBinder
//{
//    public object BindModel(ControllerContext controllerContext,
//        ModelBindingContext bindingContext)
//    {
//        ValueProviderResult valueResult = bindingContext.ValueProvider
//            .GetValue(bindingContext.ModelName);
//        ModelState modelState = new ModelState { Value = valueResult };
//        object actualValue = null;
//        try
//        {
//            actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
//                CultureInfo.CurrentCulture);
//        }
//        catch (FormatException e)
//        {
//            modelState.Errors.Add(e);
//        }

//        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
//        return actualValue;
//    }
//}