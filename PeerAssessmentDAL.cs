// ============================================================
// DAL/PeerAssessmentDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class PeerAssessmentDAL : BaseDAL
    {
        public List<PeerAssessment> GetByAssessor(int assessorId)
        {
            var list = new List<PeerAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetPeerAssessmentsByAssessor", new SqlParameter("@AssessorID", assessorId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<PeerAssessment> GetByAssessee(int assesseeId)
        {
            var list = new List<PeerAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetPeerAssessmentsByAssessee", new SqlParameter("@AssesseeID", assesseeId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<PeerAssessment> GetByProject(int projectId)
        {
            var list = new List<PeerAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetPeerAssessmentsByProject", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public PeerAssessment GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetPeerAssessmentById", new SqlParameter("@PeerAssessmentID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public bool Exists(int assessorId, int assesseeId, int projectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_CheckPeerAssessmentExists",
                new SqlParameter("@AssessorID", assessorId),
                new SqlParameter("@AssesseeID", assesseeId),
                new SqlParameter("@ProjectID", projectId)))
            {
                return r.Read();
            }
        }

        public int Create(PeerAssessment model)
        {
            SqlParameter outId = new SqlParameter("@NewPeerAssessmentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreatePeerAssessment",
                new SqlParameter("@AssessorID", model.AssessorID),
                new SqlParameter("@AssesseeID", model.AssesseeID),
                new SqlParameter("@ProjectID", model.ProjectID),
                new SqlParameter("@RubricID", (object)model.RubricID ?? DBNull.Value),
                new SqlParameter("@Score", model.Score),
                new SqlParameter("@Feedback", (object)model.Feedback ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(PeerAssessment model)
        {
            int rows = ExecuteNonQuery("sp_UpdatePeerAssessment",
                new SqlParameter("@PeerAssessmentID", model.PeerAssessmentID),
                new SqlParameter("@Score", model.Score),
                new SqlParameter("@Feedback", (object)model.Feedback ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeletePeerAssessment", new SqlParameter("@PeerAssessmentID", id)) > 0;
        }

        private PeerAssessment Map(SqlDataReader r)
        {
            return new PeerAssessment
            {
                PeerAssessmentID = GetValue<int>(r, "PeerAssessmentID"),
                AssessorID = GetValue<int>(r, "AssessorID"),
                AssesseeID = GetValue<int>(r, "AssesseeID"),
                ProjectID = GetValue<int>(r, "ProjectID"),
                RubricID = GetValue<int?>(r, "RubricID"),
                Score = GetValue<int>(r, "Score"),
                Feedback = GetValue<string>(r, "Feedback"),
                AssessedAt = GetValue<DateTime>(r, "AssessedAt"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private PeerAssessment MapWithContext(SqlDataReader r)
        {
            var pa = Map(r);
            pa.AssessorName = GetValue<string>(r, "AssessorName");
            pa.AssesseeName = GetValue<string>(r, "AssesseeName");
            pa.ProjectTitle = GetValue<string>(r, "ProjectTitle");
            return pa;
        }
    }
}