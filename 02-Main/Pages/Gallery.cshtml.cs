using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace StairwayDesigns.Pages
{
    public class Gallery_Model : PageModel
    {
        [FromRoute(Name = "id")] public int? Id { get; set; }

        public List<ImageGalleryItem> AllGalleryImages { get; private set; } = new();

        public void OnGet(int? id)
        {
            Id = id;
            LoadImages();
        }

        private void LoadImages()
        {
            // Populate gallery (moved from Blazor @code block)
            AllGalleryImages = new List<ImageGalleryItem>
            {
                // Arcedia 108 Project
                new("/wp-content/uploads/sites/SD/Arcedia-108/IMG_6233%202.jpg", "Arcedia 108 - Living Room", "Arcedia 108 Project - Modern Living Space"),
                new("/wp-content/uploads/sites/SD/Arcedia-108/IMG_6234%202.jpg", "Arcedia 108 - Bedroom", "Arcedia 108 Project - Elegant Bedroom Design"),
                new("/wp-content/uploads/sites/SD/Arcedia-108/IMG_6240.jpg", "Arcedia 108 - Kitchen", "Arcedia 108 Project - Modern Kitchen"),
                new("/wp-content/uploads/sites/SD/Arcedia-108/IMG_6243.jpg", "Arcedia 108 - Dining", "Arcedia 108 Project - Dining Area"),
                new("/wp-content/uploads/sites/SD/Arcedia-108/IMG_6256.jpg", "Arcedia 108 - Interior", "Arcedia 108 Project - Interior Details"),

                // Asit Nayak Project
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_9260.jpg", "Asit Nayak - Living Room", "Asit Nayak Project - Contemporary Design"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/8d43e7b2-7b48-43e0-9916-d36f450f1dfa.jpg", "Asit Nayak - Interior", "Asit Nayak Project - Modern Interior"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/675df6c9-af2c-4f1b-9874-7ae97881dded.jpg", "Asit Nayak - Design", "Asit Nayak Project - Stylish Design"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/ba7f1f11-5994-4a96-9c40-b43951579e0b.jpg", "Asit Nayak - Room", "Asit Nayak Project - Beautiful Room"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/f34607e2-2a8e-4397-8e36-62b84e088f4a.jpg", "Asit Nayak - Space", "Asit Nayak Project - Elegant Space"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7856.jpg", "Asit Nayak - Design Detail", "Asit Nayak Project - Design Details"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7860.jpg", "Asit Nayak - Interior View", "Asit Nayak Project - Interior View"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7879.jpg", "Asit Nayak - Modern Design", "Asit Nayak Project - Modern Design"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7884.jpg", "Asit Nayak - Living Area", "Asit Nayak Project - Living Area"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7889.jpg", "Asit Nayak - Bedroom Design", "Asit Nayak Project - Bedroom Design"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7894.jpg", "Asit Nayak - Kitchen Space", "Asit Nayak Project - Kitchen Space"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7896.jpg", "Asit Nayak - Interior Feature", "Asit Nayak Project - Interior Feature"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7913.jpg", "Asit Nayak - Design Element", "Asit Nayak Project - Design Element"),
                new("/wp-content/uploads/sites/SD/Asit-Nayak/IMG_7971.jpg", "Asit Nayak - Final Design", "Asit Nayak Project - Final Design"),

                // Bholanath Mohanty Project
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/d80b3c0e-3c92-49f7-828e-9dfb764b7dc4.jpg", "Bholanath Mohanty - Living Room", "Bholanath Mohanty Project - Luxury Living"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/0c5d3ed6-00e9-4cf9-8b68-da426864729c.jpg", "Bholanath Mohanty - Interior", "Bholanath Mohanty Project - Premium Interior"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/06b4f17d-29d8-4cac-8903-39cdeeeb7da5.jpg", "Bholanath Mohanty - Design", "Bholanath Mohanty Project - Contemporary Design"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/7fd3107b-e34a-423e-9b65-54d312621023.jpg", "Bholanath Mohanty - Space", "Bholanath Mohanty Project - Modern Space"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/9bb0663b-eb40-43c9-9f48-761f2ea43313.jpg", "Bholanath Mohanty - Room", "Bholanath Mohanty Project - Elegant Room"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/261d2bb2-2fd0-4a5e-bb83-9031bc168dab.jpg", "Bholanath Mohanty - Interior Design", "Bholanath Mohanty Project - Interior Design"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/6550c3b9-be8b-4056-9b2e-7a55d30462b7.jpg", "Bholanath Mohanty - Living Area", "Bholanath Mohanty Project - Living Area"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/73594620-340d-440b-9c69-d24ec9d31ea4.jpg", "Bholanath Mohanty - Modern Design", "Bholanath Mohanty Project - Modern Design"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/a7c377c0-62d2-4856-a5bb-8fcdd1c544ca.jpg", "Bholanath Mohanty - Luxury Interior", "Bholanath Mohanty Project - Luxury Interior"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/a0187ed8-5587-48c3-9583-85df4c7cdb3a.jpg", "Bholanath Mohanty - Design Feature", "Bholanath Mohanty Project - Design Feature"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/b1ab8a93-d0b8-4137-a2f9-9ee0c71d9348.jpg", "Bholanath Mohanty - Contemporary Space", "Bholanath Mohanty Project - Contemporary Space"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/d1ee9e4e-d973-443a-b4e4-f09bcb90a756.jpg", "Bholanath Mohanty - Interior Detail", "Bholanath Mohanty Project - Interior Detail"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/1c7a0135-f3f2-4e42-bff6-e927b09719b6.jpg", "Bholanath Mohanty - Premium Design", "Bholanath Mohanty Project - Premium Design"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/cb66641d-b85e-4c7d-a171-1a1747865a0b.jpg", "Bholanath Mohanty - Modern Interior", "Bholanath Mohanty Project - Modern Interior"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/1aa8fe08-c807-43ac-9dc0-5e7ad9c7f6ed.jpg", "Bholanath Mohanty - Stylish Design", "Bholanath Mohanty Project - Stylish Design"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/3bbe8622-d84e-48cf-9244-fdddec46cbea.jpg", "Bholanath Mohanty - Elegant Space", "Bholanath Mohanty Project - Elegant Space"),
                new("/wp-content/uploads/sites/SD/Bholanath-Mohanty/4cb35adc-f8e1-452e-891a-7ae180ed13f7.jpg", "Bholanath Mohanty - Final Design", "Bholanath Mohanty Project - Final Design"),

                // Dr. Jagat Project
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8955.jpg", "Dr. Jagat - Medical Office", "Dr. Jagat Project - Professional Medical Office"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8919.jpg", "Dr. Jagat - Reception Area", "Dr. Jagat Project - Reception Area"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8925.jpg", "Dr. Jagat - Consultation Room", "Dr. Jagat Project - Consultation Room"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8929.jpg", "Dr. Jagat - Interior Design", "Dr. Jagat Project - Interior Design"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8933.jpg", "Dr. Jagat - Professional Space", "Dr. Jagat Project - Professional Space"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8940.jpg", "Dr. Jagat - Medical Facility", "Dr. Jagat Project - Medical Facility"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8941.jpg", "Dr. Jagat - Office Interior", "Dr. Jagat Project - Office Interior"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8942.jpg", "Dr. Jagat - Modern Design", "Dr. Jagat Project - Modern Design"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8943.jpg", "Dr. Jagat - Clinical Space", "Dr. Jagat Project - Clinical Space"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8944.jpg", "Dr. Jagat - Professional Interior", "Dr. Jagat Project - Professional Interior"),
                new("/wp-content/uploads/sites/SD/Dr.Jagat/IMG_8938.jpg", "Dr. Jagat - Medical Office Design", "Dr. Jagat Project - Medical Office Design"),

                // Commercial Container Project
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_8768.jpg", "Commercial Container - Exterior", "Commercial Container Project - Modern Exterior Design"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7295.jpg", "Commercial Container - Interior", "Commercial Container Project - Contemporary Interior"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7303.jpg", "Commercial Container - Design", "Commercial Container Project - Innovative Design"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7304.jpg", "Commercial Container - Space", "Commercial Container Project - Functional Space"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7311.jpg", "Commercial Container - Layout", "Commercial Container Project - Efficient Layout"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7314.jpg", "Commercial Container - Modern Interior", "Commercial Container Project - Modern Interior"),
                new("/wp-content/uploads/sites/SD/Commercial-Container/IMG_7318.jpg", "Commercial Container - Final Design", "Commercial Container Project - Final Design")
            };
        }
    }

    public record ImageGalleryItem(string ImageUrl, string AltText, string Caption);
}
