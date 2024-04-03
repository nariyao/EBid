using EBid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.IO;

namespace EBid.Controllers
{
    [Route("/dashboard/clients/")]
    public class ClientController : Controller
    {
        private readonly EBidDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public ClientController(EBidDbContext db, IWebHostEnvironment environment) {
            this._db = db;
            this._environment = environment;
        }

        [Route("",Name ="ClientsIndex")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("add-client",Name ="AddClient")]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [RequestSizeLimit(10*1024*1024)]
        [Route("add-client",Name ="AddClient")]
        public IActionResult AddClient(Client client,IFormFile clientPhoto)
        {
            if (clientPhoto == null)
            {
                return RedirectToRoute("AddClient");
            }
            try
            {
                if (!client.SavePhoto(clientPhoto))
                {
                    throw new Exception("error occurs while handling photo.");
                }
                _db.clients.Add(client);
                _db.SaveChanges(true);
                BankAccount bankAccount = new BankAccount();
                bankAccount.Client = client;
                bankAccount.ClientId = client.ClientId;
                return View("~/views/Client/AddBankAccount.cshtml",bankAccount);
            }
            catch (Exception ex)
            {
                return View("Error",ex);
            }
           // return new RedirectToRouteResult($"/dashboard/clients/list-clients/clientId/{client.ClientId}",client);
        }
        [HttpPost,ValidateAntiForgeryToken]
        [Route("bank-account/add-bank-accout")]
        public IActionResult AddBankAccount(BankAccount bankAccount) {
            _db.bankAccounts.Add(bankAccount);
            _db.SaveChanges(true);
            return RedirectToAction("DisplayClient",new {ClientId=bankAccount.ClientId});
        }
        

        [Route("list-clients/{ClientId:bool?}",Name="ListClients")]
        public IActionResult ListClients(bool? isDeleted) {
            dynamic clients;
            if (isDeleted==null){
                ViewBag.IsDeleted = "All";
                clients = _db.clients.ToList();
            }
            else
            {
                ViewBag.IsDeleted = (isDeleted == true) ? "Deleted" : "NotDeleted";
                clients = _db.clients.Where(c=>c.IsDeleted==isDeleted).ToList();
            }
            return View(clients);
        }

        [Route("display-client-details/ClientId={ClientId:guid}",Name ="DisplayClient")]
        public IActionResult DisplayClient(Guid ClientId)
        {
            var client = _db.clients.Include(b=>b.BankAccount).Single(c=>c.ClientId == ClientId);
            return View(client);
        }

        [HttpGet]
        [Route("edit-client-details/ClientId={ClientId:guid}",Name ="EditClient")]
        public IActionResult EditClient(Guid ClientId)
        {
            var client = _db.clients.Include(b => b.BankAccount).Single( c => c.ClientId == ClientId);
            return View(client);
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("edit-client-details/ClientId={ClientId:guid}",Name ="EditClient")]
        public IActionResult EditClient(Client client,IFormFile clientPhoto)
        {
            if(clientPhoto != null) {
                client.SavePhoto(clientPhoto);
            }
            Client? oldClient = _db.clients.Find(client.ClientId);
            if(oldClient != null)
            {
                oldClient.CopyClientToClient(client);
            }
            BankAccount? bankAccount = _db.bankAccounts.SingleOrDefault(b => b.ClientId == client.ClientId);
            if(bankAccount != null)
            {
                bankAccount.CopyBankAccountToBankAccount(client.BankAccount);
            }
            else
            {
                client.BankAccount.ClientId = client.ClientId;
                _db.bankAccounts.Add(client.BankAccount);
            }
            _db.SaveChanges();
            return RedirectToAction("DisplayClient",new {ClientId=client.ClientId});
        }

        [Route("delete-client/ClientId={ClientId:guid}",Name ="DeleteClient")]
        public IActionResult DeleteClient(Guid ClientId) {
            Client? client = _db.clients.Find(ClientId);
            if (client != null) {
                client.IsDeleted = true;
            }
            _db.SaveChanges();
            return RedirectToAction("ListClients",new {isDeleted=false});
        }
    }
}
