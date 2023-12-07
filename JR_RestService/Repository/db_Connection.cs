using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JR_RestService.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace JR_RestService.Repository
{
    public abstract class db_Connection
    {
        protected SqlConnection con;
        protected SqlCommand cmd;
        protected SqlDataAdapter dat;
        protected SqlDataReader dr;
        public SqlConnection sql_con { get; set; }
        public IConfiguration configuration { get;  }
        public db_Connection(string constr, IConfiguration _configuration)
        {
            con = new SqlConnection();
            cmd = new SqlCommand();
            dat = new SqlDataAdapter();
            this.configuration = _configuration;
            con.ConnectionString = _configuration.GetConnectionString(constr);

        }
    }
}
