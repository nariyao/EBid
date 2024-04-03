using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class Photo
    {
        [Required]
        [Display(Name = "Photo Id")]
        public Guid PhotoId { get; set; } = Guid.NewGuid();
        [Display(Name = "Photo Name")]
        public string Name{ get; set; }
        [Display(Name = "Photo")]
        public byte[] BinaryPhoto { get; set; }
        [Required]
        [Display(Name="Photo url")]
        public string Url{ get; set; }
        [Display(Name = "Photo Size")]
        public string Size { get; set; }
        [Display(Name = "Upload Data & Time")]
        public DateTime UploadDateTime { get; set; } = DateTime.Now;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;


        public bool SavePhoto(IFormFile photo)
        {
            try
            {
                string image_directory = "wwwroot/product_images";
                if (!Directory.Exists(image_directory))
                {
                    Directory.CreateDirectory(image_directory);
                }
                if (this.Name != null)
                {
                    if (File.Exists($"{image_directory}/{this.Name}"))
                    {
                        File.Delete($"{image_directory}/{this.Name}");
                    }
                }
                string image_name = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filepath = image_directory + "/" + image_name;
                FileStream fs = new FileStream(filepath, FileMode.Create);
                photo.CopyToAsync(fs);
                fs.Close();
                this.Name= image_name;
                using (var file_buffer = new MemoryStream())
                {
                    photo.CopyToAsync(file_buffer);
                    this.BinaryPhoto = file_buffer.ToArray();
                }
                this.Size = photo.Length.ToString();
                this.Url = $"/product_images/{image_name}";
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeletePhoto() {
            try
            {
                string image_directory = "wwwroot/product_images";
                if (!Directory.Exists(image_directory))
                {
                    Directory.CreateDirectory(image_directory);
                }
                if (this.Name != null)
                {
                    if (File.Exists($"{image_directory}/{this.Name}"))
                    {
                        File.Delete($"{image_directory}/{this.Name}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
