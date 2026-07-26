using System.Configuration;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public static class DbConnection
    {
        public static SqlConnection GetConnection()
        {
            string cs = ConfigurationManager.ConnectionStrings["EduTrackConnection"].ConnectionString;
            return new SqlConnection(cs);
        }
    }
}