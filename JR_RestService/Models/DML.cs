using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JR_RestService.Models
{
    public class JR_Emp_Dtl
    {
        [Key]
        public Nullable<int> Id { get; set; }

        [Required(ErrorMessage ="JR Header ID's length is not more than 18 characters")]
        public int F_JR_Emp_Hdr { get; set; }
        public string JR_Detail { get; set; }
        public string Sub_Point { get; set; }
        public string Sub_PointOfPoint { get; set; }
        public Nullable<int> PointNo { get; set; }
        public Nullable<int> SubPointNo { get; set; }
    }


    public class JR_Emp_Hdr
    {
        [Key]
        public Nullable<int> Id { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Employee Code's length is not more than 7 Character")]
        public string Emp_Cd { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Employee Name's length is not more than 50 Character")]
        public string Emp_Nm { get; set; }

        [Required]
        public DateTime DOJ { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Department Code's length is not more than 7 Character")]
        public string Dept_Cd { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Department Name's length is not more than 50 Character")]
        public string Dept_Nm { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Designation Code's length is not more than 7 Character")]
        public string Desig_Cd { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Designation Name's length is not more than 50 Character")]
        public string Desig_Nm { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Category Code's length is not more than 7 Character")]
        public string Catg_Cd { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category Name's length is not more than 50 Character")]
        public string Catg_Nm { get; set; }
        [Required(ErrorMessage = ("Revision Number is required"))]
        public int Revision_No { get; set; }

        [Required(ErrorMessage =("Revision Date is required"))]
        public DateTime Revision_Date { get; set; }
        public int Supersede_No { get; set; }
        public string Reason { get; set; }
        public string luser_Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastUpdateOn { get; set; }
        public int IsCurrent { get; set; }
        public string status { get; set; }
        public DateTime Emp_app_dt { get; set; }
        public DateTime End_Date { get; set; }
        public string final_Auth_Id { get; set; }
    }


    public class JR_MenusAccess
    {
        [Key]
        public Nullable<int> Id { get; set; }

        [Required(ErrorMessage ="Role ID is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Menu ID is required")]
        public int MenuId { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Employee Code's length is not more than 7 Character")]
        public string Emp_Cd { get; set; }
    }


    public class JR_Menus
    {
        [Key]
        public Nullable<int> MenuId { get; set; }

        public int ParentMenuID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title's length is not more than 100 Character")]
        public string Title { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "URL's length is not more than 200 Character")]
        public string Url { get; set; }
        public int isActive { get; set; }
        public int srno { get; set; }
        public string icon { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "URL's length is not more than 200 Character")]
        public string UrlNew { get; set; }
    }

    public class JR_rights
    {
        [Key]
        public Nullable<int> Id { get; set; }

        [Required(ErrorMessage ="Role Id is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Create Right is required")]
        public int createRight { get; set; }

        [Required(ErrorMessage = "Read Right is required")]
        public int readRight { get; set; }

        [Required(ErrorMessage = "Edit Right is required")]
        public int editRight { get; set; }

        [Required(ErrorMessage = "Final Approval is required")]
        public int finalApproval { get; set; }

        [Required(ErrorMessage = "Forward is required")]
        public int forward { get; set; }

        [Required(ErrorMessage = "HR Approval is required")]
        public int hrApproval { get; set; }
    }

    public class JR_Roles
    {
        [Key]
        public Nullable<int> Id { get; set; }
        public string Roles { get; set; }

        [Required(ErrorMessage = "IsActive must be checked")]
        public int isActive { get; set; }
    }

    public class JR_Status
    {
        [Key]
        public Int32? statusID { get; set; }
        public int Sta { get; set; }
        public string Tra { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Status Name's length is not more than 100 Character")]
        public string status_nm { get; set; }
    }



    public class JRInbox
    {
        [Key]
        public Int32? Id { get; set; }
        public Nullable<Int32> JR_Hdr_Id { get; set; }

        [StringLength(7, ErrorMessage = "Employee Code's length is not more than 7 Character")]
        public string Emp_Cd { get; set; }
        public Nullable<System.DateTime> Emp_app_dt { get; set; }

        [StringLength(7, ErrorMessage = "First Approval ID's length is not more than 7 Character")]
        public string firstapp_id { get; set; }
        public Nullable<System.DateTime> firstapp_dt { get; set; }

        [StringLength(7, ErrorMessage = "Final Approval ID's length is not more than 7 Character")]
        public string finalapp_id { get; set; }
        public Nullable<System.DateTime> finalapp_dt { get; set; }

        [StringLength(7, ErrorMessage = "First HR Approval ID's length is not more than 7 Character")]
        public string hr_finalapp_id { get; set; }
        public Nullable<System.DateTime> hr_finalapp_dt { get; set; }

        [StringLength(100, ErrorMessage = "Remark's length is not more than 100 Character")]
        public string Remark { get; set; }
        public int HR_RoleID { get; set; }
        public Int32 statusID { get; set; }
    }

    public class JR_Audit_Trails
    {
        [Key]
        public Int32? AT_Id { get; set; }

        [StringLength(1000,ErrorMessage ="Reason's Length is not more than 1000 characters")]
        public string AT_Reason { get; set; }
        public string AT_Activity { get; set; }
        [StringLength(7, ErrorMessage = "UpdateBy's length is not more than 7 Character")]
        public string AT_UpdatedBy { get; set; }
        public System.DateTime AT_UpdatedOn { get; set; }
        public Int32 JR_Hdr_Id { get; set; }
    }

    public class JRLogin
   {   
        [Key]
        public Int32? Id { get; set; }

        [StringLength(2, ErrorMessage = "Company Code's length is not more than 2 Character")]
        public Int32 comp_cd { get; set; }

        [StringLength(2, ErrorMessage = "Location Code's length is not more than 2 Character")]
        public Int32 locn_cd { get; set; }

        [StringLength(6, ErrorMessage = "Employee Code's length is not more than 6 Character")]
        public string emp_cd { get; set; }

        [StringLength(5, ErrorMessage = "Department Code's length is not more than 5 Character")]
        public Int32 dept_cd { get; set; }

        [Required(ErrorMessage ="Present Status is required")]
        public string present { get; set; } //] [char] (1) NOT NULL,

        [Required(ErrorMessage = "User Password is required")]
        [StringLength(200, ErrorMessage = "User's Password's length is not more than 200 Character")]
        public string userpass { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }

        [Required(ErrorMessage ="End Date is required")]       
        public System.DateTime end_date { get; set; }
    }   

}
