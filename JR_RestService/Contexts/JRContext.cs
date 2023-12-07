using JR_RestService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_RestService.Contexts
{
    public class JRContext : DbContext
    {
        public JRContext(DbContextOptions<JRContext> options) : base(options) { }
        public DbSet<JR_Emp_Dtl> JR_Emp_Dtl_1 { get; set; }
        public DbSet<JR_Emp_Hdr> JR_Emp_Hdr_1 { get; set; }
        public DbSet<JR_Audit_Trails> JR_Audit_Trails_1 { get; set; }
        public DbSet<JR_Menus> JR_Menus_1 { get; set; }
        public DbSet<JR_MenusAccess> JR_MenusAccess_1 { get; set; }
        public DbSet<JR_rights> JR_rights_1 { get; set; }
        public DbSet<JR_Roles> JR_Roles_1 { get; set; }
        public DbSet<JR_Status> JR_Status_1 { get; set; }
        public DbSet<JRInbox> JRInbox_1 { get; set; }
        public DbSet<JRLogin> JRLogin_1 { get; set; }

    }
}
