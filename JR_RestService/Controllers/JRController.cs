using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JR_RestService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using JR_RestService.ViewModel;

namespace JR_RestService.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JRController : ControllerBase
    {
        private readonly IJRService _jrService;
        public JRController(IJRService jrService)
        {
            _jrService = jrService;
        }


        [HttpGet("checkDuplicate/{empcd}")]
        public async Task<IActionResult> checkDuplicate(string empcd)
        {
            QueryResponse<string> res = new QueryResponse<string>();
            res = await _jrService.checkDuplicate(empcd);
            if (res.response == -1)
            {
                return NotFound(res.responseMsg);
            }
            else if (res.response == 1)
            {
                return Ok(res.responseMsg);

            }
            return NotFound();
        }

        [HttpGet("checkJRAuth/{empcd}")]
        public async Task<IActionResult> checkJRAuth(string empcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            res = await _jrService.checkJRAuth(empcd);
            if (res.response == -1)
            {
                return BadRequest(res);
            }
            else if (res.response == 1)
            {
                return Ok(res);
            }
            return NotFound(res);
        }

        [HttpGet("JRMenusAngular/{empcd}")]
        public async Task<IActionResult> getJRMenusAngular1(string empcd)
        {
            List<PRP_JRMenu> s = new List<PRP_JRMenu>();
            s = await _jrService.getJRMenusAngular1(empcd);
            if (s!=null)
            {
                if (s.Count == 0)
                {
                    return NotFound();
                }
                else if (s.Count > 0)
                {
                    return Ok(s);
                }
            }
            else
            {
                return NotFound();
            }
            return NotFound();
        }

        [HttpGet("checkPassword/{usercd}/{oldpwd}")]
        public async Task<IActionResult> checkPassword(string usercd, string oldpwd)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            res= await _jrService.checkPassword(usercd,oldpwd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("CheckLogin/{usercd}/{pwd}")]
        public async Task<IActionResult> CheckLogin(string usercd, string pwd)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            res = await _jrService.Check_Login(usercd, pwd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("AllEmployee_ExistinJRSys")]
        public async Task<IActionResult> getAllEmployee_ExistinJRSys()
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            res = await _jrService.getAllEmployee_ExistinJRSys();
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("AllEmployee_notExistinJRSys")]
        public async Task<IActionResult> getAllEmployee_notExistinJRSys()
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            res = await _jrService.getAllEmployee_notExistinJRSys();
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("DashboardTabs/{empcd}")]
        public async Task<IActionResult> GetDashboardTabs(string empcd)
        {
            List<PRP_JR_Dashboard> s = new List<PRP_JR_Dashboard>();
            s = await _jrService.GetDashboardTabs(empcd);
            if (s != null)
            {
                return Ok(s);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("Department/{empcd}")]
        public async Task<IActionResult> getDepartment(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            res = await _jrService.getDepartment(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("DepartmentList/{empcd}")]
        public async Task<IActionResult> getDepartmentList(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            res = await _jrService.getDepartmentList(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("FirstAuthorityDepartmentList/{empcd}")]
        public async Task<IActionResult> getDepartmentListtoFirstAuthority(string empcd)
        {
            QueryResponse<PRP_Department> res = new QueryResponse<PRP_Department>();
            res = await _jrService.getDepartmentListtoFirstAuthority(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("Employee/{empcd}")]
        public async Task<IActionResult> getEmployee(string deptcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            res = await _jrService.getEmployee(deptcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("EmployeeDetail/{empcd}")]
        public async Task<IActionResult> GetEmployeeDetail(string empcd)
        {
            QueryResponse<PRP_EmployeeDetail> res = new QueryResponse<PRP_EmployeeDetail>();
            res = await _jrService.GetEmployeeDetail(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("EmployeeforJREntry/{deptcd}/{empcd}")]
        public async Task<IActionResult> getEmployeeforJREntry(string deptcd, string empcd)
        {
            QueryResponse<PRP_Employee> res = new QueryResponse<PRP_Employee>();
            res = await _jrService.getEmployeeforJREntry(deptcd,empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("getHIT_Counter")]
        public async Task<IActionResult> getHIT_Counter()
        {
            QueryResponse<PRP_Hit_Counter> res = new QueryResponse<PRP_Hit_Counter>();
            res = await _jrService.getHIT_Counter();
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRContents/{id}")]
        public async Task<IActionResult> getJRContents(string id)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.getJRContents(id);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRFinalApprovalInbox/{empcd}")]
        public async Task<IActionResult> GetJRFinalApprovalInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            res = await _jrService.GetJRFinalApprovalInbox(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRHdr/{empcd}/{deptcd}/{desigcd}/{jrID}")]
        public async Task<IActionResult> GetJRHdr(string empcd, string deptcd, string desigcd, string jrID)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.GetJRHdr(empcd, deptcd,desigcd,jrID);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRHODApprovalInbox/{empcd}")]
        public async Task<IActionResult> GetJRHODApprovalInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            res = await _jrService.GetJRHODApprovalInbox(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRInbox/{empcd}")]
        public async Task<IActionResult> GetJRInbox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            res = await _jrService.GetJRInbox(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRLists/{empcd}/{tag}")]
        public async Task<IActionResult> getJRLists(string empcd, string tag)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.getJRLists(empcd,tag);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JROutBox/{empcd}")]
        public async Task<IActionResult> getJROutBox(string empcd)
        {
            QueryResponse<PRP_JRInbox> res = new QueryResponse<PRP_JRInbox>();
            res = await _jrService.getJROutBox(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("JRReport/{deptcd}/{datfro}/{datTo}")]
        public async Task<IActionResult> GetJRReport(string deptcd, string datfro, string datTo)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.GetJRReport(deptcd, datfro, datTo);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRReportPrint/{empcd}")]
        public async Task<IActionResult> GetJRReportPrint(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.GetJRReportPrint(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRRevision/{empcd}")]
        public async Task<IActionResult> getJRRevision(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.getJRRevision(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRRevisionHistoryPrint/{empcd}")]
        public async Task<IActionResult> GetJRRevisionHistoryPrint(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.GetJRRevisionHistoryPrint(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRRoles")]
        public async Task<IActionResult> GetJRRoles()
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.GetJRRoles();
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("MenuRole/{usercd}")]
        public async Task<IActionResult> GetMenuRole(string usercd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.GetMenuRole(usercd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("PendingJR/{empcd}")]
        public async Task<IActionResult> getPendingJR(string empcd)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.getPendingJR(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("UserRole/{usercd}")]
        public async Task<IActionResult> GetUserRole(string usercd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.GetUserRole(usercd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRAccessRights")]
        public async Task<IActionResult> JR_AccessRights()
        {
            QueryResponse<PRP_JRAccessRights> res = new QueryResponse<PRP_JRAccessRights>();
            res = await _jrService.JR_AccessRights();
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("JRApproval")]
        public async Task<IActionResult> JR_Approval([FromQuery] PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.JR_Approval(p);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRUserRole/{empcd}/{authcd}")]
        public async Task<IActionResult> JR_getUserRole(string empcd, string authcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.JR_getUserRole(empcd, authcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JRMaxAuthRole/{empcd}/{authcd}")]
        public async Task<IActionResult> JR_MaxgetAuthRole(string empcd, string authcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.JR_MaxgetAuthRole(empcd, authcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("JR_MaxgetUserRole/{empcd}")]
        public async Task<IActionResult> JR_MaxgetUserRole(string empcd)
        {
            QueryResponse<PRP_JRRole> res = new QueryResponse<PRP_JRRole>();
            res = await _jrService.JR_MaxgetUserRole(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("MovetoInbox")]
        public async Task<IActionResult> JR_MovetoInbox([FromQuery]PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.JR_MovetoInbox(p);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("SaveJREntry")]
        public async Task<IActionResult> SaveJREntry([FromQuery]PRP_JRHdr p)
        {
            QueryResponse<PRP_JRHdr> res = new QueryResponse<PRP_JRHdr>();
            res = await _jrService.SaveJREntry(p);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("save_Hit_Counter")]
        public async Task<IActionResult> save_Hit_Counter([FromQuery] PRP_Hit_Counter args)
        {
            QueryResponse<PRP_Hit_Counter> res = new QueryResponse<PRP_Hit_Counter>();
            res = await _jrService.save_Hit_Counter(args);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("WelcomeMessage/{empcd}")]
        public async Task<IActionResult> Set_Welcome_Message(string empcd)
        {
            QueryResponse<PRP_users> res = new QueryResponse<PRP_users>();
            res = await _jrService.Set_Welcome_Message(empcd);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromQuery]PRP_JRLogin p)
        {
            QueryResponse<PRP_JRLogin> res = new QueryResponse<PRP_JRLogin>();
            res = await _jrService.UpdatePassword(p);
            if (res != null)
            {
                if (res.response == -1)
                {
                    return NotFound(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            else
            {
                return BadRequest();
            }
        }     
    }
}
