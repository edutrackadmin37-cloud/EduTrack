using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ResourceDAL : BaseDAL
    {
        public List<Resource> GetAllResources()
        {
            List<Resource> list = new List<Resource>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllResources"))
            {
                while (r.Read())
                {
                    list.Add(new Resource
                    {
                        ResourceID = GetValue<int>(r, "ResourceID"),
                        ResourceTitle = GetValue<string>(r, "ResourceTitle"),
                        ResourceType = GetValue<string>(r, "ResourceType"),
                        ResourcePath = GetValue<string>(r, "ResourcePath"),
                        UploadedBy = GetValue<int>(r, "UploadedBy"),
                        UploadDate = GetValue<DateTime>(r, "UploadDate"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        UploadedByName = GetValue<string>(r, "UploadedByName")
                    });
                }
            }
            return list;
        }

        public Resource GetResourceById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetResourceById", new SqlParameter("@ResourceID", id)))
            {
                if (!r.Read()) return null;
                return new Resource
                {
                    ResourceID = GetValue<int>(r, "ResourceID"),
                    ResourceTitle = GetValue<string>(r, "ResourceTitle"),
                    ResourceType = GetValue<string>(r, "ResourceType"),
                    ResourcePath = GetValue<string>(r, "ResourcePath"),
                    UploadedBy = GetValue<int>(r, "UploadedBy"),
                    UploadDate = GetValue<DateTime>(r, "UploadDate"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int CreateResource(Resource resource)
        {
            SqlParameter outId = new SqlParameter("@NewResourceID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateResource",
                new SqlParameter("@ResourceTitle", resource.ResourceTitle),
                new SqlParameter("@ResourceType", (object)resource.ResourceType ?? DBNull.Value),
                new SqlParameter("@ResourcePath", (object)resource.ResourcePath ?? DBNull.Value),
                new SqlParameter("@UploadedBy", resource.UploadedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateResource(Resource resource)
        {
            int rows = ExecuteNonQuery("sp_UpdateResource",
                new SqlParameter("@ResourceID", resource.ResourceID),
                new SqlParameter("@ResourceTitle", (object)resource.ResourceTitle ?? DBNull.Value),
                new SqlParameter("@ResourceType", (object)resource.ResourceType ?? DBNull.Value),
                new SqlParameter("@ResourcePath", (object)resource.ResourcePath ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDeleteResource(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteResource", new SqlParameter("@ResourceID", id)) > 0;
        }
    }
}