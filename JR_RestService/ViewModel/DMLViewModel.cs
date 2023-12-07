using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_RestService.ViewModel
{

    public class PRP_JRLogin
    {        
        public string comp_cd { get; set; }        
        public Int64 locn_cd { get; set; }        
        public string emp_cd { get; set; }        
        public Int64 dept_cd { get; set; }        
        public Int64 present { get; set; }        
        public string userpass { get; set; }        
        public string dec_userpass { get; set; }        
        public string enc_userpass { get; set; }        
        public string start_date { get; set; }        
        public string end_date { get; set; }        
        public Int64 outres { get; set; }
    }

    public class PRP_JRRole
    {

        public string emp_cd { get; set; }

        public string emp_nm { get; set; }

        public Int64 RoleId { get; set; }

        public string Roles { get; set; }
    }

    public class PRP_JRInbox
    {

        public Int64 Id { get; set; }

        public string emp_cd { get; set; }

        public string emp_nm { get; set; }

        public Int64 Dept_Cd { get; set; }

        public string Dept_Nm { get; set; }

        public string Desig_Nm { get; set; }

        public string final_auth_cd_dept { get; set; }

        public string final_auth_cd_hr { get; set; }
    }

    public class PRP_JR_Audit_Trails
    {

        public Int64 AT_Id { get; set; }

        public string AT_Reason { get; set; }

        public string AT_Activity { get; set; }

        public string AT_UpdatedBy { get; set; }

        public string AT_UpdatedOn { get; set; }
    }

    public class PRP_JR_Dashboard
    {

        public Int64 JR_count { get; set; }

        public string tabname { get; set; }

        public string url { get; set; }

        public string color { get; set; }

        public string angularUrl { get; set; }
    }

    public class PRP_JRMenu : PRP_JRRole
    {

        public Int64 MenuId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string icon { get; set; }

        public List<PRP_JRMenu> submenus { get; set; }

        public string UrlNew { get; set; }
    }

    public class Prp_Breadcrumb
    {

        public string menuHeader { get; set; }

        public string mainMenuLabel { get; set; }

        public string menuLabel { get; set; }
    }

    public class PRP_users
    {

        public Int64 TotalRecords { get; set; }

        public string empcd { get; set; }

        public string empnm { get; set; }

        public Int64 outres { get; set; }

        public string token { get; set; }
    }

    public class QueryResponse<T>
    {

        public Int64 response { get; set; }

        public string responseMsg { get; set; }

        public System.Data.DataSet responseSet { get; set; }

        public System.Data.DataTable responseTableList { get; set; }

        public List<T> responseObjectList { get; set; }

        public T responseObject { get; set; }

        public Int64 CheckID { get; set; }

        public Int64 slipNo { get; set; }

        public double preStock { get; set; }

        public Int64 currentMivNo { get; set; }
    }

    public class PRP_JRDtl
    {

        public string F_JR_Emp_Hdr { get; set; }

        public string JR_Detail { get; set; }

        public string Sub_Point { get; set; }

        public string Sub_PointOfPoint { get; set; }

        public string PointNo { get; set; }

        public string SubPointNo { get; set; }
    }

    public class PRP_JRHdr
    {

        public Int64 TotalRecords { get; set; }

        public string move { get; set; }

        public Int64 sta { get; set; }

        public string tra { get; set; }

        public string btnaction { get; set; }

        public string btnsave { get; set; }

        public Int64 RoleId { get; set; }

        public Int64 EntryExists { get; set; }

        public string firstapp_id { get; set; }

        public string firstapp_nm { get; set; }

        public string firstapp_dt { get; set; }

        public string firstapp_tm { get; set; }

        public string finalapp_id { get; set; }

        public string finalapp_nm { get; set; }

        public string finalapp_dt { get; set; }

        public string finalapp_tm { get; set; }

        public string hr_finalapp_id { get; set; }

        public string hr_finalapp_nm { get; set; }

        public string hr_finalapp_dt { get; set; }

        public string hr_finalapp_tm { get; set; }

        public string prepapp_id { get; set; }

        public string prepapp_nm { get; set; }

        public string prepapp_dt { get; set; }

        public string prepapp_tm { get; set; }

        public string emp_cd { get; set; }

        public string emp_nm { get; set; }

        public string Emp_app_dt { get; set; }

        public string Emp_att_tm { get; set; }

        public string dept_cd { get; set; }

        public string dept_nm { get; set; }

        public string desig_cd { get; set; }

        public string desig_nm { get; set; }

        public string catg_cd { get; set; }

        public string catg_nm { get; set; }

        public string doj { get; set; }

        public string revision_no { get; set; }

        public string revision_date { get; set; }

        public string supersede_no { get; set; }

        public string reason { get; set; }

        public string jr_detail { get; set; }

        public Int64 isCurrent { get; set; }

        public Int64 f_hdr_id { get; set; }

        public Int64 Id { get; set; }

        public string userpass { get; set; }

        public string luserId { get; set; }
    }

    public class PRP_EmployeeDetail
    {

        public string emp_cd { get; set; }

        public string emp_nm { get; set; }

        public string dept_cd { get; set; }

        public string dept_nm { get; set; }

        public string desig_cd { get; set; }

        public string desig_nm { get; set; }

        public string catg_cd { get; set; }

        public string catg_nm { get; set; }

        public string dt_join { get; set; }
    }

    public class PRP_Department
    {

        public string dept_cd { get; set; }

        public string dept_nm { get; set; }

        public string empcd { get; set; }
    }

    public class PRP_User_Master
    {
        public string isDTC { get; set; }

        public string emp_cd { get; set; }

        public string dept_nm { get; set; }

        public string desig_nm { get; set; }

        public Int64 catg_cd { get; set; }

        public string catg_nm { get; set; }

        public string std_qual_desc { get; set; }

        public string emp_nm { get; set; }

        public string emp { get; set; }

        public string las_usr_cod { get; set; }

        public string las_usr_dat { get; set; }

        public short sta { get; set; }

        public long u_id { get; set; }

        public Int64 dept_cd { get; set; }

        public Int64 desig_cd { get; set; }

        public DateTime dt_join { get; set; }

        public Int64 loc_cod { get; set; }

        public Int64 pre_exp_mon { get; set; }

        public Int64 pre_exp_yrs { get; set; }

        public short rev_num { get; set; }

        public Int64 std_qual_cd { get; set; }

        public string las_usr_pas { get; set; }
    }

    public class PRP_Hit_Counter
    {

        public long hit_cou { get; set; }
    }

    public class PRP_Employee
    {

        public string emp_cd { get; set; }

        public string emp_nm { get; set; }
    }

    public class PRP_JRAccessRights
    {

        public string accessRights { get; set; }


        public bool isAdmin { get; set; }


        public bool isHR { get; set; }


        public bool isFinalAuth { get; set; }


        public bool isFirstAuth { get; set; }


        public bool isUser { get; set; }
    }
}
