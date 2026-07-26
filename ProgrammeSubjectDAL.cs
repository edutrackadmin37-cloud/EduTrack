using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ProgrammeSubjectDAL : BaseDAL
    {
        public List<ProgrammeSubject> GetAll()
        {
            List<ProgrammeSubject> list = new List<ProgrammeSubject>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllProgrammeSubjects"))
            {
                while (r.Read())
                {
                    list.Add(new ProgrammeSubject
                    {
                        ProgrammeSubjectID = GetValue<int>(r, "ProgrammeSubjectID"),
                        ProgrammeID = GetValue<int>(r, "ProgrammeID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        IsElective = GetValue<bool>(r, "IsElective"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public List<ProgrammeSubject> GetByProgramme(int programmeId)
        {
            List<ProgrammeSubject> list = new List<ProgrammeSubject>();
            using (SqlDataReader r = ExecuteReader("sp_GetProgrammeSubjectsByProgramme", new SqlParameter("@ProgrammeID", programmeId)))
            {
                while (r.Read())
                {
                    list.Add(new ProgrammeSubject
                    {
                        ProgrammeSubjectID = GetValue<int>(r, "ProgrammeSubjectID"),
                        ProgrammeID = GetValue<int>(r, "ProgrammeID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        IsElective = GetValue<bool>(r, "IsElective"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        SubjectCode = GetValue<string>(r, "SubjectCode"),
                        SubjectIsCore = GetValue<bool?>(r, "IsCore")
                    });
                }
            }
            return list;
        }

        public int Create(int programmeId, int subjectId, bool isElective)
        {
            SqlParameter outId = new SqlParameter("@NewProgrammeSubjectID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateProgrammeSubject",
                new SqlParameter("@ProgrammeID", programmeId),
                new SqlParameter("@SubjectID", subjectId),
                new SqlParameter("@IsElective", isElective),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool SoftDelete(int programmeSubjectId)
        {
            return ExecuteNonQuery("sp_SoftDeleteProgrammeSubject", new SqlParameter("@ProgrammeSubjectID", programmeSubjectId)) > 0;
        }
    }
}