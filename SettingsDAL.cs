// ============================================================
// DAL/SettingsDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SettingsDAL : BaseDAL
    {
        public SettingsDTO GetSettings()
        {
            var dto = new SettingsDTO();
            using (SqlDataReader r = ExecuteReader("sp_GetAllSystemSettings"))
            {
                while (r.Read())
                {
                    string name = r["SettingName"].ToString();
                    string value = r["SettingValue"].ToString();
                    switch (name)
                    {
                        case "InstitutionName": dto.InstitutionName = value; break;
                        case "ContactEmail": dto.ContactEmail = value; break;
                        case "GradingScale": dto.GradingScale = value; break;
                        case "ManualApproval": dto.ManualApproval = Convert.ToBoolean(value); break;
                        case "SendMail": dto.SendMail = Convert.ToBoolean(value); break;
                        case "PlatformURL": dto.PlatformURL = value; break;
                        case "MaintenanceMode": dto.MaintenanceMode = Convert.ToBoolean(value); break;
                        case "EnableChat": dto.EnableChat = Convert.ToBoolean(value); break;
                        case "EnablePeerAssessment": dto.EnablePeerAssessment = Convert.ToBoolean(value); break;
                        case "SchoolYear": dto.SchoolYear = value; break;
                    }
                }
            }
            return dto;
        }

        public void SaveSettings(SettingsDTO dto)
        {
            SaveSetting("InstitutionName", dto.InstitutionName);
            SaveSetting("ContactEmail", dto.ContactEmail);
            SaveSetting("GradingScale", dto.GradingScale);
            SaveSetting("ManualApproval", dto.ManualApproval.ToString());
            SaveSetting("SendMail", dto.SendMail.ToString());
            SaveSetting("PlatformURL", dto.PlatformURL);
            SaveSetting("MaintenanceMode", dto.MaintenanceMode.ToString());
            SaveSetting("EnableChat", dto.EnableChat.ToString());
            SaveSetting("EnablePeerAssessment", dto.EnablePeerAssessment.ToString());
            SaveSetting("SchoolYear", dto.SchoolYear);
        }

        private void SaveSetting(string name, string value)
        {
            ExecuteNonQuery("sp_UpsertSystemSetting",
                new SqlParameter("@SettingName", name),
                new SqlParameter("@SettingValue", value));
        }
    }
}