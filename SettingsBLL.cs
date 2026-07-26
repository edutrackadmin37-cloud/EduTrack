// ============================================================
// BLL/SettingsBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;

namespace EduTrack.BLL
{
    public static class SettingsBLL
    {
        private static readonly SettingsDAL _dal = new SettingsDAL();

        public static SettingsDTO GetSettings()
        {
            return _dal.GetSettings();
        }

        public static void SaveSettings(SettingsDTO settings)
        {
            _dal.SaveSettings(settings);
        }
    }
}