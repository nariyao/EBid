using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace EBid.Models
{
    public class Client:Person
    {

        public Guid ClientId { get; set; } = Guid.NewGuid();
        public BankAccount BankAccount { get; set; }

        public ICollection<Product>? Products { get; set; }

        public ICollection<Auction>? Auctions { get; set; }

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

        public void CopyClientToClient(Client client)
        {
            this.ClientId = client.ClientId;
            this.FirstName = client.FirstName;
            this.MiddleName = client.MiddleName;
            this.LastName = client.LastName;
            this.Email = client.Email;
            this.Phone = client.Phone;
            this.PhotoName = client.PhotoName;
            this.PhotoUrl = client.PhotoUrl;
            this.BinaryPhoto =  client.BinaryPhoto;
            this.Line1 = client.Line1;
            this.Line2 = client.Line2;
            this.Landmark = client.Landmark;
            this.City = client.City;
            this.State = client.State;
            this.Country = client.Country;
            this.PostalCode = client.PostalCode;
            this.LastModifiedDate = DateTime.Now;
            this.IsDeleted = false;
        }
    }
}
