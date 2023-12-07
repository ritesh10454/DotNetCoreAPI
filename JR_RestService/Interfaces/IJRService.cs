using JR_RestService.Models;
using JR_RestService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_RestService.Interfaces
{
    public interface IJRService
    {
        Task<QueryResponse<PRP_JRLogin>> Check_Login(string usercd, string pwd);
        Task<QueryResponse<PRP_JRLogin>> checkPassword(string usercd, string oldpwd);
        Task<QueryResponse<PRP_Employee>> checkJRAuth(string empcd);
        Task<QueryResponse<PRP_Employee>> getEmployee(string deptcd);
        Task<QueryResponse<PRP_Employee>> getEmployeeforJREntry(string deptcd, string empcd);
        Task<List<PRP_JRMenu>> getJRMenusAngular1(string usercd);
        Task<QueryResponse<PRP_JRAccessRights>> JR_AccessRights();
        Task<QueryResponse<PRP_Employee>> getAllEmployee_notExistinJRSys();
        Task<QueryResponse<PRP_Employee>> getAllEmployee_ExistinJRSys();
        Task<QueryResponse<PRP_EmployeeDetail>> GetEmployeeDetail(string empcd);
        Task<QueryResponse<PRP_users>> Set_Welcome_Message(string empcd);
        Task<QueryResponse<PRP_JRRole>> GetJRRoles();
        Task<QueryResponse<PRP_JRRole>> GetUserRole(string usercd);
        Task<QueryResponse<PRP_JRRole>> GetMenuRole(string usercd);
        Task<QueryResponse<PRP_JRRole>> JR_MaxgetUserRole(string empcd);
        Task<QueryResponse<PRP_JRRole>> JR_getUserRole(string empcd, string authcd);
        Task<QueryResponse<PRP_JRRole>> JR_MaxgetAuthRole(string empcd, string authcd);
        Task<QueryResponse<PRP_JRInbox>> GetJRInbox(string empcd);
        Task<QueryResponse<PRP_JRInbox>> GetJRFinalApprovalInbox(string empcd);
        Task<QueryResponse<PRP_JRInbox>> GetJRHODApprovalInbox(string empcd);
        Task<QueryResponse<PRP_JRInbox>> getJROutBox(string empcd);
        Task<QueryResponse<PRP_JRHdr>> SaveJREntry(PRP_JRHdr p);
        Task<QueryResponse<PRP_JRLogin>> UpdatePassword(PRP_JRLogin p);
        Task<QueryResponse<PRP_JRHdr>> JR_Approval(PRP_JRHdr p);
        Task<QueryResponse<PRP_JRHdr>> JR_MovetoInbox(PRP_JRHdr p);
        Task<QueryResponse<PRP_JRHdr>> getJRContents(string id);
        Task<QueryResponse<PRP_JRHdr>> GetJRHdr(string empcd, string deptcd, string desigcd, string jrID);
        Task<QueryResponse<PRP_JRHdr>> GetJRReport(string deptcd, string datfro, string datTo);
        Task<QueryResponse<PRP_JRHdr>> GetJRReportPrint(string empcd);
        Task<QueryResponse<PRP_JRHdr>> GetJRRevisionHistoryPrint(string empcd);
        Task<QueryResponse<PRP_JRHdr>> getPendingJR(string empcd);
        Task<QueryResponse<PRP_JRHdr>> getJRLists(string empcd, string tag);
        Task<QueryResponse<PRP_JRHdr>> getJRRevision(string empcd);
        Task<QueryResponse<string>> checkDuplicate(string empcd);
        Task<QueryResponse<PRP_Department>> getDepartmentListtoFirstAuthority(string empcd);
        Task<QueryResponse<PRP_Department>> getDepartment(string empcd);
        Task<QueryResponse<PRP_Department>> getDepartmentList(string empcd);
        Task<List<PRP_JR_Dashboard>> GetDashboardTabs(string empcd);
        string encryptQueryString(string str);
        string decryptQueryString(string str);
        Task<QueryResponse<PRP_Hit_Counter>> getHIT_Counter();
        Task<QueryResponse<PRP_Hit_Counter>> save_Hit_Counter(PRP_Hit_Counter args);




    }
}
