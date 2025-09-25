using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;

namespace StairwayDesigns.Pages
{
    public class About_UsModel : PageModel
    {
        private readonly string _cacheVersion = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);

        public void OnGet()
        {
            // Could preload or set up model data here if needed later.
        }

        public string GetImageWithCacheBuster(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return string.Empty;

            // Keep UTC for consistency; add both a version (stable per request) and ms for variability
            return $"{imageUrl}?v={_cacheVersion}&t={DateTime.UtcNow.Millisecond}";
        }

        public string GetImageErrorHandler(string projectName)
        {
            projectName ??= "Project";
            return $"this.style.border='2px solid #f0f0f0'; this.style.background='#f8f9fa'; this.alt='Image temporarily unavailable - {projectName}'; this.style.opacity='0.7'; console.log('Image load failed: ' + this.src);";
        }

        public string GetFounderImageErrorHandler()
        {
            return "this.style.border='2px solid #f0f0f0'; this.style.background='#f8f9fa'; this.alt='Founder image temporarily unavailable'; this.style.opacity='0.7'; this.parentElement.innerHTML += '<div style=\"position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);text-align:center;color:#666;\"><i class=\"fa fa-user\" style=\"font-size:48px;margin-bottom:10px;\"></i><br><small>Photo Loading...</small></div>'; console.log('Founder image load failed: ' + this.src);";
        }
    }
}
