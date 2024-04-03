using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class Customer:Person
    {
        [Required]
        [Display(Name ="Customer Id")]
        public Guid CustomerId { get; set; }= Guid.NewGuid();
        public ICollection<Bid>? Orders { get; set; }
        public User User { get; set; }

        public bool SavePhoto(IFormFile photo)
        {
            try
            {
                string image_directory = "wwwroot/images";
                if (!Directory.Exists(image_directory))
                {
                    Directory.CreateDirectory(image_directory);
                }
                if (this.PhotoName != null)
                {
                    if (File.Exists($"{image_directory}/{this.PhotoName}"))
                    {
                        File.Delete($"{image_directory}/{this.PhotoName}");
                    }
                }
                string image_name = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filepath = image_directory + "/" + image_name;
                FileStream fs = new FileStream(filepath, FileMode.Create);
                photo.CopyToAsync(fs);
                fs.Close();
                this.PhotoName = image_name;
                using (var file_buffer = new MemoryStream())
                {
                    photo.CopyToAsync(file_buffer);
                    this.BinaryPhoto = file_buffer.ToArray();
                }

                this.PhotoUrl = $"/images/{image_name}";
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CopyCustomerToCustomer(Customer customer)
        {
            this.CustomerId = customer.CustomerId;
            this.FirstName = customer.FirstName;
            this.MiddleName = customer.MiddleName;
            this.LastName = customer.LastName;
            this.Email = customer.Email;
            this.Phone = customer.Phone;
            this.PhotoName = customer.PhotoName;
            this.PhotoUrl = customer.PhotoUrl;
            this.BinaryPhoto = customer.BinaryPhoto;
            this.Line1 = customer.Line1;
            this.Line2 = customer.Line2;
            this.Landmark = customer.Landmark;
            this.City = customer.City;
            this.State = customer.State;
            this.Country = customer.Country;
            this.PostalCode = customer.PostalCode;
            this.LastModifiedDate = DateTime.Now;
            this.IsDeleted = false;
        }
    }
}

