using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;

namespace EduTrack
{
    public partial class Global : HttpApplication
    {
        // Static configuration for performance
        private static readonly string LogDir = null;
        private static readonly bool MaintenanceMode = ConfigurationManager.AppSettings["MaintenanceMode"]?.ToLower() == "true";

        static Global()
        {
            // Initialize log directory once during app startup
            try
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    LogDir = context.Server.MapPath("~/App_Data/Logs/");
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);
                }
            }
            catch { /* Ignore startup errors */ }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Register jQuery for unobtrusive validation
            RegisterjQueryBundle();

            LogApplicationEvent("Application started successfully");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Maintenance Mode Check
            if (MaintenanceMode && !IsAdminOrMaintenancePath())
            {
                Response.Redirect("~/Maintenance.aspx");
                return;
            }

            // Security Headers (additional)
            Response.AddHeader("X-Content-Type-Options", "nosniff");
            Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
            Response.AddHeader("X-XSS-Protection", "1; mode=block");

            // Prevent caching of sensitive pages
            if (IsSensitivePath())
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.Now);
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Log important events
            if (Response.StatusCode >= 400)
            {
                LogRequestEvent(string.Format("HTTP {0} - {1}", Response.StatusCode, Request.RawUrl));
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex == null) return;

            // Log the error
            LogError(ex);

            // Clear the error
            Server.ClearError();

            // Determine error type and redirect appropriately
            HttpException httpEx = ex as HttpException;
            int statusCode = httpEx?.GetHttpCode() ?? 500;

            try
            {
                string redirectUrl = GetErrorRedirectUrl(statusCode, ex);
                Response.Redirect(redirectUrl);
            }
            catch
            {
                // If redirect fails, show raw error
                ShowRawError(ex, statusCode);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialize session
            Session["SessionStartTime"] = DateTime.Now;
            Session["SessionID"] = Session.SessionID;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Cleanup session
            Session.Clear();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            LogApplicationEvent("Application ended");
        }

        // =====================================================
        // PRIVATE HELPER METHODS
        // =====================================================

        /// <summary>
        /// Register jQuery for unobtrusive validation
        /// </summary>
        private void RegisterjQueryBundle()
        {
            try
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                    new ScriptResourceDefinition
                    {
                        Path = "~/scripts/jquery-3.6.0.min.js",
                        DebugPath = "~/scripts/jquery-3.6.0.js",
                        CdnPath = "https://code.jquery.com/jquery-3.6.0.min.js",
                        CdnDebugPath = "https://code.jquery.com/jquery-3.6.0.js",
                        CdnSupportsSecureConnection = true
                    });
            }
            catch (Exception ex)
            {
                LogError(ex, "jQuery registration failed");
            }
        }

        /// <summary>
        /// Determine error redirect URL based on status code
        /// </summary>
        private string GetErrorRedirectUrl(int statusCode, Exception ex)
        {
            string baseUrl = GetBaseUrl();

            return statusCode switch
            {
                400 => $"{baseUrl}Error.aspx?code=400",
                401 => $"{baseUrl}Auth/Login.aspx",
                403 => $"{baseUrl}Error403.aspx",
                404 => $"{baseUrl}Error404.aspx",
                500 => $"{baseUrl}Error.aspx?code=500&msg=" + HttpUtility.UrlEncode(ex?.Message),
                503 => $"{baseUrl}Maintenance.aspx",
                _ => $"{baseUrl}Error.aspx?code={statusCode}"
            };
        }

        /// <summary>
        /// Check if current request is for a sensitive path (requires no caching)
        /// </summary>
        private bool IsSensitivePath()
        {
            string path = Request.Path.ToLower();
            string[] sensitivePaths = {
                "/auth/",
                "/admin/",
                "/user/",
                "/student/",
                "/teacher/",
                "/parent/",
                "/api/",
                "/messages",
                "/notifications"
            };

            foreach (string sensitiveP in sensitivePaths)
            {
                if (path.Contains(sensitiveP))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if path is admin or maintenance allowed
        /// </summary>
        private bool IsAdminOrMaintenancePath()
        {
            string path = Request.Path.ToLower();
            return path.Contains("/admin/") || path.Contains("/maintenance");
        }

        /// <summary>
        /// Get base URL for redirects
        /// </summary>
        private string GetBaseUrl()
        {
            HttpRequest request = Request;
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = request.Url.Scheme,
                Host = request.Url.Host,
                Port = request.Url.Port,
                Path = request.ApplicationPath + "/"
            };

            string baseUrl = uriBuilder.Uri.ToString();

            // Fallback to configured PlatformURL if available
            string configUrl = ConfigurationManager.AppSettings["PlatformURL"];
            if (!string.IsNullOrEmpty(configUrl))
                baseUrl = configUrl.TrimEnd('/') + "/";

            return baseUrl;
        }

        /// <summary>
        /// Display raw error page (fallback)
        /// </summary>
        private void ShowRawError(Exception ex, int statusCode)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "text/html; charset=utf-8";
                Response.StatusCode = statusCode;

                string html = GenerateErrorHtml(ex, statusCode);
                Response.Write(html);
                Response.End();
            }
            catch { /* Silent failure */ }
        }

        /// <summary>
        /// Generate HTML for error page
        /// </summary>
        private string GenerateErrorHtml(Exception ex, int statusCode)
        {
            string errorTitle = statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Page Not Found",
                500 => "Internal Server Error",
                _ => "Error"
            };

            string errorMessage = statusCode switch
            {
                400 => "The request was invalid or malformed.",
                401 => "You are not authorized to access this resource.",
                403 => "Access to this resource is forbidden.",
                404 => "The requested page could not be found.",
                500 => "An unexpected server error occurred.",
                _ => "An error has occurred."
            };

            bool showDetails = ConfigurationManager.AppSettings["EnableDetailedErrors"]?.ToLower() == "true";

            string html = $@"
<!DOCTYPE html>
<html>
<head>
	<meta charset='utf-8' />
	<meta name='viewport' content='width=device-width, initial-scale=1.0' />
	<title>EduTrack - {errorTitle}</title>
	<style>
		* {{ margin: 0; padding: 0; box-sizing: border-box; }}
		body {{
			font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
			background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
			min-height: 100vh;
			display: flex;
			align-items: center;
			justify-content: center;
			padding: 20px;
		}}
		.error-container {{
			background: white;
			border-radius: 12px;
			box-shadow: 0 20px 60px rgba(0,0,0,0.3);
			max-width: 600px;
			width: 100%;
			padding: 40px;
			text-align: center;
		}}
		.error-code {{
			font-size: 72px;
			font-weight: bold;
			color: #667eea;
			margin-bottom: 10px;
		}}
		.error-title {{
			font-size: 28px;
			color: #333;
			margin-bottom: 15px;
		}}
		.error-message {{
			font-size: 16px;
			color: #666;
			margin-bottom: 30px;
			line-height: 1.6;
		}}
		.error-details {{
			background: #f5f5f5;
			border-left: 4px solid #dc3545;
			padding: 15px;
			margin-bottom: 30px;
			text-align: left;
			font-family: 'Courier New', monospace;
			font-size: 12px;
			max-height: 300px;
			overflow-y: auto;
			color: #333;
			display: {(showDetails ? "block" : "none")};
		}}
		.error-actions {{
			display: flex;
			gap: 10px;
			justify-content: center;
			flex-wrap: wrap;
		}}
		.btn {{
			display: inline-block;
			padding: 10px 20px;
			border-radius: 6px;
			text-decoration: none;
			font-weight: 600;
			transition: all 0.3s;
			border: none;
			cursor: pointer;
			font-size: 14px;
		}}
		.btn-primary {{
			background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
			color: white;
		}}
		.btn-primary:hover {{
			transform: translateY(-2px);
			box-shadow: 0 6px 20px rgba(102,126,234,0.4);
		}}
		.btn-secondary {{
			background: #f0f0f0;
			color: #333;
		}}
		.btn-secondary:hover {{
			background: #e0e0e0;
		}}
		.logo {{
			margin-bottom: 20px;
		}}
		.logo img {{
			height: 40px;
			width: auto;
		}}
	</style>
</head>
<body>
	<div class='error-container'>
		<div class='logo'>
			<img src='/Image/DVT-0185.jpg' alt='EduTrack' />
		</div>
		<div class='error-code'>{statusCode}</div>
		<div class='error-title'>{errorTitle}</div>
		<div class='error-message'>{errorMessage}</div>
		{(showDetails ? $"<div class='error-details'><strong>Details:</strong><br/>{HttpUtility.HtmlEncode(ex?.ToString() ?? "No details available")}</div>" : "")}
		<div class='error-actions'>
			<a href='/' class='btn btn-primary'>Go Home</a>
			<a href='javascript:history.back()' class='btn btn-secondary'>Go Back</a>
		</div>
	</div>
</body>
</html>";

            return html;
        }

        // =====================================================
        // LOGGING METHODS
        // =====================================================

        private void LogApplicationEvent(string message)
        {
            if (string.IsNullOrEmpty(LogDir)) return;

            try
            {
                string logPath = Path.Combine(LogDir, "Application.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch { /* Ignore logging errors */ }
        }

        private void LogRequestEvent(string message)
        {
            if (string.IsNullOrEmpty(LogDir)) return;

            try
            {
                string logPath = Path.Combine(LogDir, "Request.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch { /* Ignore logging errors */ }
        }

        private void LogError(Exception ex, string additionalInfo = "")
        {
            if (ex == null || string.IsNullOrEmpty(LogDir)) return;

            try
            {
                string logPath = Path.Combine(LogDir, "Error.log");

                string url = "N/A";
                string ip = "N/A";
                string userAgent = "N/A";

                try
                {
                    if (Request?.RawUrl != null)
                        url = Request.RawUrl;
                    if (Request?.UserHostAddress != null)
                        ip = Request.UserHostAddress;
                    if (Request?.UserAgent != null)
                        userAgent = Request.UserAgent;
                }
                catch { /* Ignore request info errors */ }

                string logEntry = string.Format(
                    "{0:yyyy-MM-dd HH:mm:ss} | URL: {1} | IP: {2} | User-Agent: {3}{4}{5}{6}{7}",
                    DateTime.Now,
                    url,
                    ip,
                    userAgent,
                    !string.IsNullOrEmpty(additionalInfo) ? $" | Info: {additionalInfo}" : "",
                    Environment.NewLine,
                    ex,
                    new string('-', 80) + Environment.NewLine
                );

                File.AppendAllText(logPath, logEntry);
            }
            catch { /* Ignore all logging errors */ }
        }
    }
}