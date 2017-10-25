
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class loginReposnse
{
    public int userId { get; set; }
    public int status { get; set; }
    public string message { get; set; }
     public string userName { get; set; }
    public List<subMenuEntity> permissions { get; set; }
    public List<DCLocations> location { get; set; }
    public List<RolesEntity> roles { get; set; }
    public List<DCLocationsr> dc { get; set; }
    //public string token { get; set; }
}

public class DCLocationsr
{
    public string Region_Name { get; set; }
    public Nullable<int> Region_Id { get; set; }
    public string Region_Code { get; set; }

    public List<reg> dcDetails { get; set; }
}

public class reg
{
    public int Dc_Id { get; set; }
    public string DC_Code { get; set; }
    public string Dc_Name { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public int? Pincode { get; set; }
    public string UserType { get; set; }
    public Nullable<bool> Offline_Flag { get; set; }
   
}

public class DCLocations
{
    public int Dc_Id { get; set; }
    public string DC_Code { get; set; }
    public string Dc_Name { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public int? Pincode {get;set;}
    public string UserType { get; set; }
    public string Region_Name { get; set; }
    public Nullable<int> Region_Id { get; set; }
    public string Region_Code { get; set; }
    public Nullable<bool> Offline_Flag { get; set; }
    //Address1 = r.Address1,
    //                                     Address2 = r.Address2,
    //                                     County = r.County,
    //                                     State = r.State,
    //                                     PinCode = r.PinCode,
    //                                     FSSAI_No = r.FSSAI_No,
    //                                     CIN_No = r.CIN_No,
    //                                     CST_No = r.CST_No,
    //                                     PAN_No = r.PAN_No,
    //                                     TIN_No = r.TIN_No
}

public class RolesEntity
{
    public string rolename { get; set; }
    public int roleid { get; set; }
}
