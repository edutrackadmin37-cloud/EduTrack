// ============================================================
// Models/SettingsDTO.cs
// ============================================================
namespace EduTrack.Models
{
    public class SettingsDTO
    {
        public string SiteName { get; set; }
        public string InstitutionName { get; set; }
        public string ContactEmail { get; set; }
        public string GradingScale { get; set; }
        public bool ManualApproval { get; set; }
        public bool SendMail { get; set; }
        public string PlatformURL { get; set; }
        public bool MaintenanceMode { get; set; }
        public bool EnableChat { get; set; }
        public bool EnablePeerAssessment { get; set; }
        public string SchoolYear { get; set; }
    }
}