using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JR_RestService.Models;
using JR_RestService.ViewModel;
using JR_RestService.Interfaces;
using JR_RestService.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace JR_RestService.Repository
{
    public class JR_Repository : IJRService
    {
        private readonly IConfiguration _configuration;
        private readonly JRContext _context;
        public JR_Repository(JRContext context, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._context = context;
        }


        public async Task<QueryResponse<string>> checkDuplicate(string empcd)
        {
            QueryResponse<string> res = new QueryResponse<string>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    string val = String.Empty;
                    DataSet ds = new DataSet();
                    var para = new DynamicParameters();
                    para.Add("@Par", 30);
                    para.Add("@emp_cd", empcd);
                    var rec = await con.QueryAsync<string>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item == null)
                    {
                        res.response = -1; res.responseMsg = "Not Duplicate Found";
                    }
                    else
                    {
                        res.response = 1; res.responseMsg = item.ToString();
                    }
                }
                catch (Exception ex)
                {

                    res.response = -1; res.responseMsg = ex.Message;
                }
                
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Employee>> checkJRAuth(string empcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    string val = String.Empty;
                    DataSet ds = new DataSet();
                    var para = new DynamicParameters();
                    para.Add("@Par", 27);
                    para.Add("@emp_cd", empcd);
                    var rec = await con.QueryAsync<PRP_Employee>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item == null)
                    {
                        res.response = -1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = 1; res.responseMsg = item.emp_cd;
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRLogin>> checkPassword(string usercd, string oldpwd)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    PRP_JRLogin k = new PRP_JRLogin();
                    k.emp_cd = usercd.ToString().Trim();
                    k.userpass = EncryptDecrypt.Encrypt(oldpwd.ToString().Trim());
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", k.emp_cd);
                    para.Add("@sec_cd", k.userpass);
                    para.Add("@empFound", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await con.ExecuteAsync("JR_Check_Password", para, null, 500, CommandType.StoredProcedure);
                    res.responseObject = new PRP_JRLogin();
                    res.responseObject.outres = para.Get<Int32>("@empFound");
                    res.response = res.responseObject.outres;
                    res.responseMsg = "";
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRLogin>> Check_Login(string usercd, string pwd)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", usercd);
                    para.Add("@sec_cd", EncryptDecrypt.Encrypt(pwd));
                    para.Add("@empFound", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var rec = await con.ExecuteAsync("JR_Check_User", para, null, 500, CommandType.StoredProcedure);
                    res.responseObject = new PRP_JRLogin();
                    res.responseObject.outres = para.Get<Int32>("@empFound");
                    res.response = res.responseObject.outres;
                    res.responseMsg = "";
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }


        public  string decryptQueryString(string str)
        {
            //throw new NotImplementedException();
            return System.Web.HttpUtility.UrlEncode(EncryptDecrypt.Decrypt(str.ToString().Trim())); ;
        }

        public string encryptQueryString(string str)
        {
            return  System.Web.HttpUtility.UrlEncode(EncryptDecrypt.Encrypt(str.ToString().Trim()));
        }

        public async Task<QueryResponse<PRP_Employee>> getAllEmployee_ExistinJRSys()
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 15);
                    var rec = await con.QueryAsync<PRP_Employee>("JR_Display",para,null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Employee>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Employee()
                            {
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm
                              });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {

                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Employee>> getAllEmployee_notExistinJRSys()
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 13);
                    var rec = await con.QueryAsync<PRP_Employee>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Employee>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Employee()
                            {
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<List<PRP_JR_Dashboard>> GetDashboardTabs(string empcd)
        {
            QueryResponse<PRP_JR_Dashboard> res = new QueryResponse<PRP_JR_Dashboard>();
            List<PRP_JR_Dashboard> s = new List<PRP_JR_Dashboard>();
            res = await new DAL().Display_Dashboard(_configuration, empcd);
            if (res.response==-1)
            {
                s = null;
            }
            else if (res.response==1)
            {
                s = res.responseObjectList;
            }
            return s;
        }

        public async Task<QueryResponse<PRP_Department>> getDepartment(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            //PRP_Department s = new PRP_Department();
            //s.empcd = empcd.ToString().Trim();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 1);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_Department>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Department>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Department()
                            {
                                dept_cd = item.dept_cd,
                                dept_nm = item.dept_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Department>> getDepartmentList(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 17);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_Department>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Department>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Department()
                            {
                                dept_cd = item.dept_cd,
                                dept_nm = item.dept_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Department>> getDepartmentListtoFirstAuthority(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 22);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_Department>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Department>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Department()
                            {
                                dept_cd = item.dept_cd,
                                dept_nm = item.dept_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Employee>> getEmployee(string deptcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 2);
                    para.Add("@dept_cd", deptcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_Employee>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Employee>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Employee()
                            {
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_EmployeeDetail>> GetEmployeeDetail(string empcd)
        {
            QueryResponse<PRP_EmployeeDetail> res = new QueryResponse<PRP_EmployeeDetail>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 3);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_EmployeeDetail>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        res.responseObject=new PRP_EmployeeDetail()
                        {
                            emp_cd = item.emp_cd,
                            emp_nm = item.emp_nm,
                            dept_nm = item.dept_nm,
                            desig_nm = item.desig_nm,
                            dept_cd = item.dept_cd,
                            catg_cd = item.catg_cd,
                            catg_nm = item.catg_nm,
                            dt_join = item.dt_join,
                            desig_cd = item.desig_cd
                        }; 
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Employee>> getEmployeeforJREntry(string deptcd, string empcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 21);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    para.Add("@dept_cd", deptcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_Employee>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_Employee>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_Employee()
                            {
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Hit_Counter>> getHIT_Counter()
        {
            QueryResponse<PRP_Hit_Counter> res = new QueryResponse<PRP_Hit_Counter>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 1);
                    var rec = await con.QueryAsync<PRP_Hit_Counter>("NIVS_Display_hit_cou", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        res.responseObject = new PRP_Hit_Counter()
                        {
                             hit_cou=item.hit_cou
                        }; 
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> getJRContents(string id)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    string empattdt = ""; string empatttm = ""; string hrfinalappdt = ""; string hrfinalapptm = ""; string finalappdt = ""; string finalapptm = "";
                    string prepappdt = ""; string prepapptm = ""; string firstappdt = ""; string firstapptm = "";

                    var para = new DynamicParameters();
                    para.Add("@Par", 18);
                    para.Add("@id", id);
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        string[] str_app_dt; string[] str_hrfinalapp_dt; string[] str_finalapp_dt; string[] str_prepapp_dt; string[] str_firstapp_dt;


                        if (!String.IsNullOrEmpty(item.Emp_app_dt)){str_app_dt = item.Emp_app_dt.Substring(0, 19).Split('/', ' '); empattdt = Convert.ToDateTime(str_app_dt[1] + "/" + str_app_dt[0] + "/" + str_app_dt[2]).ToString("dd-MMM-yyyy"); empatttm = Convert.ToDateTime(str_app_dt[3]).ToString("HH:mm:ss"); } else { empattdt = ""; empatttm = ""; }
                        if (!String.IsNullOrEmpty(item.hr_finalapp_dt)){ str_hrfinalapp_dt = item.hr_finalapp_dt.Substring(0, 19).Split('/', ' '); hrfinalappdt = Convert.ToDateTime(str_hrfinalapp_dt[1] + "/" + str_hrfinalapp_dt[0] + "/" + str_hrfinalapp_dt[2]).ToString("dd-MMM-yyyy"); hrfinalapptm = Convert.ToDateTime(str_hrfinalapp_dt[3]).ToString("HH:mm:ss"); } else { hrfinalappdt = ""; hrfinalapptm = ""; }
                        if (!String.IsNullOrEmpty(item.finalapp_dt)){str_finalapp_dt = item.finalapp_dt.Substring(0, 19).Split('/', ' '); finalappdt = Convert.ToDateTime(str_finalapp_dt[1] + "/" + str_finalapp_dt[0] + "/" + str_finalapp_dt[2]).ToString("dd-MMM-yyyy"); finalapptm = Convert.ToDateTime(str_finalapp_dt[3]).ToString("HH:mm:ss"); } else { finalappdt = ""; finalapptm = ""; }
                        if (!String.IsNullOrEmpty(item.prepapp_dt)) {str_prepapp_dt = item.prepapp_dt.Substring(0, 19).Split('/', ' '); prepappdt = Convert.ToDateTime(str_prepapp_dt[1] + "/" + str_prepapp_dt[0] + "/" + str_prepapp_dt[2]).ToString("dd-MMM-yyyy"); prepapptm = Convert.ToDateTime(str_prepapp_dt[3]).ToString("HH:mm:ss"); } else { prepappdt = ""; prepapptm = ""; }
                        if (!String.IsNullOrEmpty(item.firstapp_dt)) {str_firstapp_dt = item.firstapp_dt.Substring(0, 19).Split('/', ' '); firstappdt = Convert.ToDateTime(str_firstapp_dt[1] + "/" + str_firstapp_dt[0] + "/" + str_firstapp_dt[2]).ToString("dd-MMM-yyyy"); firstapptm = Convert.ToDateTime(str_firstapp_dt[3]).ToString("HH:mm:ss"); } else { firstappdt = ""; firstapptm = ""; }


                        //if (item.Emp_app_dt != "") { empattdt = Convert.ToDateTime(item.Emp_app_dt).ToString("dd-MMM-yyyy"); empatttm = Convert.ToDateTime(item.Emp_app_dt).ToString("HH:mm:ss"); } else { empattdt = ""; empatttm = ""; }
                        //if (item.hr_finalapp_dt != "") { hrfinalappdt = Convert.ToDateTime(item.hr_finalapp_dt).ToString("dd-MMM-yyyy"); hrfinalapptm = Convert.ToDateTime(item.hr_finalapp_dt).ToString("HH:mm:ss"); } else { hrfinalappdt = ""; hrfinalapptm = ""; }
                        //if (item.finalapp_dt != "") { finalappdt = Convert.ToDateTime(item.finalapp_dt).ToString("dd-MMM-yyyy"); finalapptm = Convert.ToDateTime(item.finalapp_dt).ToString("HH:mm:ss"); } else { finalappdt = ""; finalapptm = ""; }
                        //if (item.prepapp_dt != "") { prepappdt = Convert.ToDateTime(item.prepapp_dt).ToString("dd-MMM-yyyy"); prepapptm = Convert.ToDateTime(item.prepapp_dt).ToString("HH:mm:ss"); } else { prepappdt = ""; prepapptm = ""; }
                        //if (item.firstapp_dt != "") { firstappdt = Convert.ToDateTime(item.firstapp_dt).ToString("dd-MMM-yyyy"); firstapptm = Convert.ToDateTime(item.firstapp_dt).ToString("HH:mm:ss"); } else { firstappdt = ""; firstapptm = ""; }

                        res.responseObject = new PRP_JRHdr()
                        {
                            emp_cd = item.emp_cd,
                            emp_nm = item.emp_nm,
                            doj = item.doj,
                            Emp_app_dt = empattdt,
                            Emp_att_tm = empatttm,
                            hr_finalapp_id = item.hr_finalapp_id,
                            hr_finalapp_nm = item.hr_finalapp_nm,
                            hr_finalapp_dt = hrfinalappdt,
                            hr_finalapp_tm = hrfinalapptm,
                            finalapp_id = item.finalapp_id,
                            finalapp_nm = item.finalapp_nm,
                            finalapp_dt = finalappdt,
                            finalapp_tm = finalapptm,
                            tra = item.tra,
                            sta =item.sta,
                            prepapp_id = item.prepapp_id,
                            prepapp_nm = item.prepapp_nm,
                            prepapp_dt = prepappdt,
                            prepapp_tm = prepapptm,
                            firstapp_id = item.firstapp_id,
                            firstapp_nm = item.firstapp_nm,
                            firstapp_dt = firstappdt,
                            firstapp_tm = firstapptm,
                            EntryExists = item.EntryExists,
                            revision_no = item.revision_no,
                            revision_date = item.revision_date,
                            supersede_no = item.supersede_no,
                            reason = item.reason,
                            jr_detail = item.jr_detail,
                            f_hdr_id = Convert.ToInt64(item.Id)
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRInbox>> GetJRFinalApprovalInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 19);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRInbox>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRInbox>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRInbox()
                            {
                                Id = Convert.ToInt64(item.Id),
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                Dept_Cd = Convert.ToInt64(item.Dept_Cd),
                                Dept_Nm = item.Dept_Nm,
                                Desig_Nm = item.Desig_Nm,
                                final_auth_cd_dept = item.final_auth_cd_dept,
                                final_auth_cd_hr = item.final_auth_cd_hr
                            });
                        }
                        res.response = 1; res.responseMsg="";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> GetJRHdr(string empcd, string deptcd, string desigcd, string jrID)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    if (jrID != null)
                    {
                        if (Convert.ToInt64(jrID.ToString()) > 0)
                        {
                            res = await getJRContents(jrID);
                        }
                        else
                        {
                            res = await new DAL().View_JRHDR(_configuration,empcd,deptcd,desigcd);
                        }
                    }
                    else
                    {
                        res = await new DAL().View_JRHDR(_configuration, empcd, deptcd, desigcd);
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRInbox>> GetJRHODApprovalInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 20);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRInbox>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRInbox>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRInbox()
                            {
                                Id = item.Id,
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                Dept_Cd = item.Dept_Cd,
                                Dept_Nm = item.Dept_Nm,
                                Desig_Nm = item.Desig_Nm,
                                final_auth_cd_dept = item.final_auth_cd_dept,
                                final_auth_cd_hr = item.final_auth_cd_hr
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRInbox>> GetJRInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 10);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRInbox>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRInbox>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRInbox()
                            {
                                Id = item.Id,
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                Dept_Cd = item.Dept_Cd,
                                Dept_Nm = item.Dept_Nm,
                                Desig_Nm = item.Desig_Nm,
                                final_auth_cd_dept = item.final_auth_cd_dept,
                                final_auth_cd_hr = item.final_auth_cd_hr
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> getJRLists(string empcd, string tag)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    Int32 tg = Convert.ToInt32(tag.ToString());
                    var para = new DynamicParameters();
                    if (tg==1)
                    {
                        para.Add("@Par", 25);
                    }
                    else if (tg==2)
                    {
                        para.Add("@Par", 7);
                    }
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRHdr>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRHdr()
                            {
                                revision_no = item.revision_no,
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                dept_cd = item.dept_cd,
                                dept_nm = item.dept_nm,
                                desig_cd = item.desig_cd,
                                desig_nm = item.desig_nm
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<List<PRP_JRMenu>> getJRMenusAngular1(string usercd)
        {
            QueryResponse<PRP_JRMenu> res = new QueryResponse<PRP_JRMenu>();
            try
            {
                res = await new DAL().getJRMenusAngular(_configuration, usercd);
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
            return res.responseObjectList;
        }

        public async Task<QueryResponse<PRP_JRInbox>> getJROutBox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 12);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRInbox>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRInbox>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRInbox()
                            {
                                Id = item.Id,
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                Dept_Cd = item.Dept_Cd,
                                Dept_Nm = item.Dept_Nm,
                                Desig_Nm = item.Desig_Nm,
                                final_auth_cd_dept = item.final_auth_cd_dept,
                                final_auth_cd_hr = item.final_auth_cd_hr
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> GetJRReport(string deptcd, string datfro, string datTo)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 5);
                    para.Add("@dept_cd", deptcd);
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRHdr>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRHdr()
                            {
                                emp_nm = item.emp_nm,
                                emp_cd = item.emp_cd,
                                revision_no = item.revision_no,
                                revision_date = item.revision_date
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> GetJRReportPrint(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 6);
                    para.Add("@emp_cd", empcd);
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        res.responseObject=new PRP_JRHdr()
                        {
                            emp_cd = empcd,
                            emp_nm = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.emp_nm),
                            doj = item.doj,
                            dept_nm = item.dept_nm,
                            desig_nm = item.desig_nm,
                            catg_nm = item.catg_nm,
                            revision_no = item.revision_no,
                            revision_date = item.revision_date,
                            supersede_no = item.supersede_no,
                            reason = item.reason,
                            jr_detail = item.jr_detail
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> getJRRevision(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 9);
                    para.Add("@emp_cd", empcd);
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count() > 0)
                    {
                        res.responseObjectList = new List<PRP_JRHdr>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRHdr()
                            {
                                revision_no = item.revision_no,
                                revision_date = item.revision_date,
                                supersede_no = item.supersede_no,
                                reason = item.reason
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> GetJRRevisionHistoryPrint(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 9);
                    para.Add("@emp_cd", empcd);
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if(rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRHdr>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRHdr()
                            {
                                revision_no = item.revision_no,
                                revision_date = item.revision_date,
                                supersede_no = item.supersede_no,
                                reason = item.reason
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> GetJRRoles()
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 26);
                    var rec = await con.QueryAsync<PRP_JRRole>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if(rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRRole>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRRole()
                            {
                                RoleId = item.RoleId,
                                Roles = item.Roles
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> GetMenuRole(string usercd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            try
            {
                res = await new DAL().MenuRoleByUserCode(_configuration, usercd.ToString().Trim());
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> getPendingJR(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@par", 7);
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRHdr>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRHdr()
                            {
                                revision_no = item.revision_no,
                                emp_cd = item.emp_cd,
                                emp_nm = item.emp_nm,
                                dept_cd = item.dept_cd,
                                dept_nm = item.dept_nm,
                                desig_cd = item.desig_cd,
                                desig_nm = item.desig_nm
                            });
                        }
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> GetUserRole(string usercd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            try
            {
                res = await new DAL().UserRoleByUserCode(_configuration, usercd);
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRAccessRights>> JR_AccessRights()
        {
            QueryResponse<PRP_JRAccessRights> res = new QueryResponse<PRP_JRAccessRights>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    var rec = await con.QueryAsync<PRP_JRAccessRights>("JR_AccessRight", null, null, 500, CommandType.StoredProcedure);
                    if (rec.Count()>0)
                    {
                        res.responseObjectList = new List<PRP_JRAccessRights>();
                        foreach (var item in rec.ToList())
                        {
                            res.responseObjectList.Add(new PRP_JRAccessRights()
                            {
                                accessRights = item.accessRights,
                                isAdmin = item.isAdmin,
                                isFinalAuth = item.isFinalAuth,
                                isFirstAuth = item.isFinalAuth,
                                isHR = item.isHR,
                                isUser = item.isUser
                            });
                            res.response = 1; res.responseMsg = "";
                        }
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "Records not Found";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> JR_Approval(PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    con.Open();
                    var para = new DynamicParameters();
                    para.Add("@JR_Hdr_Id", p.f_hdr_id);
                    para.Add("@finalapp_id", p.finalapp_id);
                    para.Add("@RoleID", p.RoleId);
                    para.Add("@Revision_Date", p.revision_date);
                    para.Add("@response", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    para.Add("@responseMsg", dbType: DbType.String, direction: ParameterDirection.Output);
                    var rec = await con.ExecuteAsync("JR_UpdateInbox", para, null, 500, CommandType.StoredProcedure);
                    res.response = para.Get<Int32>("@response");
                    res.responseMsg = para.Get<string>("@responseMsg");
                    if (p.RoleId == 2) { res.responseMsg = "JR is Approved by User Successfully."; } else if (p.RoleId == 3) { res.responseMsg = "JR is Successfully Approved by User's First Authority."; } else if (p.RoleId == 5) { res.responseMsg = "JR is Successfully Approved by Final Authority and forwarded to HR department."; } else if (p.RoleId == 5) { res.responseMsg = "JR is Successfully Approved by HR department."; }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> JR_getUserRole(string empcd, string authcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    para.Add("@auth_cd", authcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRRole>("SELECT * FROM [dbo].[JR_getAuthRole](@emp_cd,@auth_cd)", para, null, 500, CommandType.Text);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        res.responseObject = new PRP_JRRole()
                        {
                            RoleId = item.RoleId
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> JR_MaxgetAuthRole(string empcd, string authcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    para.Add("@auth_cd", authcd.ToString().Trim());
                    var rec = await con.QueryAsync<PRP_JRRole>("SELECT * FROM [dbo].[JR_getAuthRole](@emp_cd,@auth_cd)", para, null, 500, CommandType.Text);
                    var item = rec.FirstOrDefault();
                    if (item != null)
                    {
                        res.responseObject = new PRP_JRRole()
                        {
                            RoleId = item.RoleId
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRRole>> JR_MaxgetUserRole(string empcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", empcd.ToString().Trim());
                    var rec =await con.QueryAsync<PRP_JRRole>("SELECT max(RoleID) as RoleID FROM [dbo].[JR_getUserRole](@emp_cd)", para, null, 500, CommandType.Text);
                    var item = rec.FirstOrDefault();
                    if (item!=null)
                    {
                        res.responseObject = new PRP_JRRole()
                        {
                            RoleId = item.RoleId
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> JR_MovetoInbox(PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@RoleID", p.RoleId);
                    para.Add("@move", p.move);
                    para.Add("@response", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    para.Add("@responseMsg", dbType: DbType.String, direction: ParameterDirection.Output);
                    var rec = await con.ExecuteAsync("JR_UpdateInbox", para, null, 500, CommandType.StoredProcedure);
                    res.response = para.Get<Int32>("@response");
                    res.responseMsg = para.Get<string>("@responseMsg");
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRHdr>> SaveJREntry(PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    if (p.btnsave == "Save")
                    {
                        con.Open();
                        var para = new DynamicParameters();
                        para.Add("@Id", 0);
                        para.Add("@Emp_Cd", p.emp_cd);
                        para.Add("@Emp_Nm", p.emp_nm);
                        para.Add("@DOJ",  p.doj);
                        para.Add("@Dept_Cd", p.dept_cd);
                        para.Add("@Dept_Nm",  p.dept_nm);
                        para.Add("@Desig_Cd",p.desig_cd);
                        para.Add("@Desig_Nm",  p.desig_nm);
                        para.Add("@Catg_Cd", p.catg_cd);
                        para.Add("@Catg_Nm", p.catg_nm);
                        para.Add("@Revision_No",  p.revision_no);
                        para.Add("@Revision_Date",  p.revision_date);
                        para.Add("@Supersede_No", p.supersede_no);
                        para.Add("@Reason", p.reason);
                        para.Add("@luser_Id",  p.luserId); 
                        para.Add("@IsCurrent", p.isCurrent);
                        para.Add("@JR_Detail",  p.jr_detail);
                        para.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        res.response= await con.ExecuteAsync("JR_Save_Hdr", para, null, 500, CommandType.StoredProcedure);
                        res.CheckID = para.Get<Int32>("@Id");
                        res.response = 1; res.responseMsg = "";
                    }
                    else if (p.btnsave == "Update")
                    {
                        con.Open();
                        var para1 = new DynamicParameters();
                        para1.Add("@Id", p.Id);
                        para1.Add("@Emp_Cd", p.emp_cd);
                        para1.Add("@Emp_Nm", p.emp_nm);
                        para1.Add("@DOJ",p.doj);
                        para1.Add("@Dept_Cd",  p.dept_cd);
                        para1.Add("@Dept_Nm", p.dept_nm);
                        para1.Add("@Desig_Cd", p.desig_cd);
                        para1.Add("@Desig_Nm", p.desig_nm);
                        para1.Add("@Catg_Cd", p.catg_cd);
                        para1.Add("@Catg_Nm", p.catg_nm);
                        para1.Add("@Revision_No",p.revision_no);
                        para1.Add("@Revision_Date", p.revision_date);
                        para1.Add("@Supersede_No", p.supersede_no);
                        para1.Add("@Reason",  p.reason);
                        para1.Add("@luser_Id", p.luserId);  //  (System.Web.HttpContext.Current.Session["usrnam"].ToString()));
                        para1.Add("@IsCurrent", p.isCurrent);
                        para1.Add("@JR_Detail", p.jr_detail);
                        para1.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        res.response = await con.ExecuteAsync("JR_Update_Hdr", para1, null, 500, CommandType.StoredProcedure);
                        res.response = 1; res.responseMsg = "";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_Hit_Counter>> save_Hit_Counter(PRP_Hit_Counter args)
        {
            QueryResponse<PRP_Hit_Counter> res = new QueryResponse<PRP_Hit_Counter>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@hit_cou", args.hit_cou);
                    res.response = await con.ExecuteAsync("NIVS_save_hit_cou", para, null, 500, CommandType.StoredProcedure);
                    res.responseMsg = "";
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_users>> Set_Welcome_Message(string empcd)
        {
            QueryResponse<PRP_users> res = new QueryResponse<PRP_users>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cndTacl")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@Par", 3);
                    para.Add("@usr_cod", empcd.ToString().Trim());
                    var rec = await con.QueryAsync<string>("NIS_Display_User", para, null, 500, CommandType.StoredProcedure);
                    var item = rec.FirstOrDefault();
                    if (item!=null)
                    {
                        res.responseObject = new PRP_users()
                        {
                            empnm = item.ToString()
                        };
                        res.response = 1; res.responseMsg = "";
                    }
                    else
                    {
                        res.response = -1; res.responseMsg = "";
                    }
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }

        public async Task<QueryResponse<PRP_JRLogin>> UpdatePassword(PRP_JRLogin p)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@emp_cd", p.emp_cd);
                    para.Add("@sec_cd", EncryptDecrypt.Encrypt(p.userpass.ToString().Trim()));
                    res.response = await con.ExecuteAsync("JR_Change_Password", para, null, 500, CommandType.StoredProcedure);
                    res.responseMsg = "Password is Changed Succesfully";
                }
                catch (Exception ex)
                {
                    res.response = -1; res.responseMsg = ex.Message;
                }
            }
            return res;
        }
    }
}


public class DAL
{
    public async Task<QueryResponse<PRP_JR_Dashboard>> Display_Dashboard(IConfiguration _configuration, string empcd)
    {
        QueryResponse<PRP_JR_Dashboard> res = new QueryResponse<PRP_JR_Dashboard>();
        List<PRP_JR_Dashboard> s = new List<PRP_JR_Dashboard>();
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@Par", 24);
                para.Add("@emp_cd", empcd.ToString().Trim());
                var rec = await con.QueryAsync<PRP_JR_Dashboard>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                if (rec.Count()>0)
                {
                    res.responseObjectList = new List<PRP_JR_Dashboard>();
                    foreach (var item in rec.ToList())
                    {
                        res.responseObjectList.Add(new PRP_JR_Dashboard()
                        {
                            JR_count = item.JR_count,
                            tabname = item.tabname,
                            url = item.url,
                            color = item.color,
                            angularUrl = item.angularUrl
                        });
                        res.response = 1; res.responseMsg = "";
                        s = res.responseObjectList;
                    }
                }
                else
                {
                    res.response = -1; res.responseMsg = "Records not Found";
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        }
        return res;
    }

    public async Task<QueryResponse<PRP_JRHdr>> View_JRHDR(IConfiguration _configuration, string empcd, string deptcd, string desigcd)
    {
        QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@Par", 4);
                para.Add("@emp_cd", empcd);
                para.Add("@dept_cd", deptcd);
                para.Add("@desig_Cd",  desigcd);
                var rec = await con.QueryAsync<PRP_JRHdr>("JR_Display", para, null, 500, CommandType.StoredProcedure);
                var item = rec.FirstOrDefault();
                if (item != null)
                {                    
                    res.responseObject = new PRP_JRHdr()
                    {
                        emp_cd = empcd,
                        f_hdr_id = item.f_hdr_id,
                        revision_no = item.revision_no,
                        revision_date = item.revision_date,
                        supersede_no = item.supersede_no,
                        reason = item.reason,
                        jr_detail = item.jr_detail,
                        EntryExists = item.EntryExists
                    };
                    res.response = 1; res.responseMsg = "";
                }
                else
                {
                    res.response = -1; res.responseMsg = "Records not Found";
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        }
        return res;
    }


    public async Task<QueryResponse<PRP_JRMenu>> getJRMenusAngular(IConfiguration _configuration, string empcd)
    {
        QueryResponse<PRP_JRMenu> res = new QueryResponse<PRP_JRMenu>();
        QueryResponse<PRP_JRMenu> p = new QueryResponse<PRP_JRMenu>();
        QueryResponse<PRP_JRRole> resRole = new QueryResponse<PRP_JRRole>();
        Int32 i = 0;
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                resRole =await MenuRoleByUserCode(_configuration, empcd);
                if (resRole.response==-1)
                {
                    res.response = -1;
                    throw new Exception(resRole.responseMsg);
                }

                var para = new DynamicParameters();
                para.Add("@emp_cd", empcd.ToString().Trim());
                res = await new DAL().ShowJRMenus(_configuration, resRole.responseObject.RoleId, 0, empcd.ToString().Trim());
                if (res.response == 1)
                {
                    foreach (PRP_JRMenu row in res.responseObjectList)
                    {
                        row.submenus = new List<PRP_JRMenu>();
                        p = await new DAL().ShowJRMenus(_configuration, row.RoleId, row.MenuId, empcd);
                        if (p.response == -1)
                        {
                            res.responseObjectList[i].MenuId = row.MenuId;
                            res.responseObjectList[i].Title = row.Title;
                            res.responseObjectList[i].Url = row.UrlNew;
                            res.responseObjectList[i].RoleId = row.RoleId;
                            res.responseObjectList[i].Roles = row.Roles;
                            res.responseObjectList[i].icon = row.icon;
                            res.responseObjectList[i].UrlNew = "";
                        }
                        else if (p.response == 1)
                        {
                            foreach (PRP_JRMenu row1 in p.responseObjectList)
                            {
                                row.submenus.Add(new PRP_JRMenu()
                                {
                                    MenuId = row1.MenuId,
                                    Title = row1.Title,
                                    Url = row1.UrlNew,
                                    RoleId = row1.RoleId,
                                    Roles = row1.Roles,
                                    icon = row1.icon,
                                    UrlNew = ""
                                });
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        }
        return res;
    }

    public async Task<QueryResponse<PRP_JRRole>> UserRoleByUserCode(IConfiguration _configuration, string usercode)
    {
        QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@emp_cd", usercode.ToString().Trim());
                var rec = await con.QueryAsync<PRP_JRRole>("JR_UserRole", para, null, 500, CommandType.StoredProcedure);
                var item = rec.FirstOrDefault();
                if (item != null)
                {                   
                    res.responseObject = new PRP_JRRole()
                    {
                        emp_cd = item.emp_cd,
                        emp_nm = item.emp_nm,
                        RoleId =item.RoleId,
                        Roles = item.Roles
                    };
                    res.response = 1; res.responseMsg = "";
                }
                else
                {
                    res.response = -1; res.responseMsg = "Records not Found";
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        } 
        return res;
    }

    public async Task<QueryResponse<PRP_JRRole>> MenuRoleByUserCode(IConfiguration _configuration,  string usercode)
    {
        QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@emp_cd", usercode.ToString().Trim());
                var rec = await con.QueryAsync<PRP_JRHdr>("JR_menuRole", para, null, 500, CommandType.StoredProcedure);
                var item = rec.FirstOrDefault();
                if (item != null)
                {                   
                    res.responseObject = new PRP_JRRole()
                    {
                        RoleId=item.RoleId
                    };
                    res.response = 1; res.responseMsg = "";
                }
                else
                {
                    res.response = -1; res.responseMsg = "Records not Found";
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        }
        return res;
    }

    public async Task<QueryResponse<PRP_JRMenu>> ShowJRMenus(IConfiguration _configuration, Int64 roleID, Int64 parentId, string usercd)
    {
        QueryResponse<PRP_JRMenu> res = new QueryResponse<PRP_JRMenu>();
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("cnd")))
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@roleId", roleID);
                para.Add("@parentId", parentId);
                para.Add("@emp_cd", usercd.ToString().Trim());
                var rec = await con.QueryAsync<PRP_JRMenu>("JR_menuAccess", para, null, 500, CommandType.StoredProcedure);
                if (rec.Count()>0)
                {
                    res.responseObjectList = new List<PRP_JRMenu>();
                    foreach (var item in rec.ToList())
                    {
                        res.responseObjectList.Add(new PRP_JRMenu()
                        {
                            MenuId = item.MenuId,
                            Title = item.Title,
                            Url = (item.Url == null) ? "" : item.Url.ToString().Trim(),
                            RoleId = item.RoleId,
                            Roles = item.Roles,
                            icon = (item.icon==null)?"": item.icon.ToString().Trim(),
                            UrlNew =(item.UrlNew==null)? "":item.UrlNew.ToString().Trim()
                        });
                    }
                    res.response = 1; res.responseMsg = "";
                }
                else
                {
                    res.response = -1; res.responseMsg = "Records not Found";
                }
            }
            catch (Exception ex)
            {
                res.response = -1; res.responseMsg = ex.Message;
            }
        }
        return res;
    }
}
