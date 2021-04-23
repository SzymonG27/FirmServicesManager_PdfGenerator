using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDFGenerator.Models.AccountModels;
using PDFGenerator.Models.ClientModels;
using PDFGenerator.Models.ViewModels;
using PDFGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Controllers
{
    public class ClientController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IClientRepository _repo;
        private readonly IFixRepository _repoFix;
        private readonly IFirmRepository _repoFirm;
        private readonly IAccesoryRepository _repoAcc;
        private readonly IClientFirmRelationRepository _repoRel;
        public ClientController(UserManager<AppUser> userManager, IClientRepository repo, IFixRepository repoFix,
            IFirmRepository repoFirm, IClientFirmRelationRepository repoRel, IAccesoryRepository repoAcc)
        {
            _userManager = userManager;
            _repo = repo;
            _repoFix = repoFix;
            _repoFirm = repoFirm;
            _repoRel = repoRel;
            _repoAcc = repoAcc;
        }

        [HttpGet]
        public async Task<IActionResult> CreateClient()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            //TEST
            RandomBarcodeGenerator generator = new RandomBarcodeGenerator();
            var str = generator.RandomString(10);
            return View(new ClientFixViewModel
            {
                barcode = str
            });
        }

        [HttpPost]
        public IActionResult CreateClient(ClientFixViewModel model)
        {
            var isInDB = _repo.Clients
                .FirstOrDefault(p => p.PhoneNumber == model.Client.PhoneNumber);
            if (isInDB != null)
            {
                TempData["Success"] = "Osoba o tym numerze telefonu istnieje już w bazie danych";
                TempData["ClientModel"] = JsonConvert.SerializeObject(new ClientFixViewModel
                {
                    Client = isInDB
                });
                return RedirectToAction("CreateFix");
            }
            if (model.Client.isFirm == false)
            {
                _repo.SaveClient(model.Client);
                TempData["Success"] = "Klient został utworzony";
                TempData["ClientModel"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("CreateFix");
            }
            var isFirmInDB = _repoFirm.Firms
                .FirstOrDefault(p => p.NIP == model.Client.NIP);
            Firm firm;
            if (isFirmInDB == null)
            {
                if (model.Client.NIP.ToString().Length != 10)
                {
                    ModelState.AddModelError("NIP", "Nip musi posiadać równo 10 cyfr");
                    return View(model);
                }
                firm = new Firm
                {
                    NIP = model.Client.NIP,
                    FirmName = model.Client.FirmName
                };
                _repoFirm.SaveFirm(firm);
                _repoRel.SaveClientFirmRelation(new ClientFirmRelation
                {
                    ClientID = model.Client.ID,
                    FirmID = firm.ID
                });
            }

            _repo.SaveClient(model.Client);
            TempData["Success"] = "Klient został utworzony";
            TempData["ClientModel"] = JsonConvert.SerializeObject(model);
            return RedirectToAction("CreateFix");
        }

        [HttpGet]
        public async Task<IActionResult> CheckClient()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CheckClient(ClientFixViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Fail"] = "Nie zgadza się";
                return View(model);
            }
            var res = _repo.Clients
                .FirstOrDefault(p => p.FirstName == model.Client.FirstName && p.SurName == model.Client.SurName && 
                p.PhoneNumber == model.Client.PhoneNumber);
            if (res != null)
            {
                TempData["Success"] = "Znaleziono w bazie danych";
                TempData["ClientModel"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("CreateFix");
            }
            else
            {
                TempData["Fail"] = "Taka osoba nie istnieje w bazie danych";
                return View(model);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> CreateFix()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                //return RedirectToAction("Index", "Home");
            }
            var model = JsonConvert.DeserializeObject<ClientFixViewModel>((string)TempData["ClientModel"]);
            TempData["ClientModel"] = JsonConvert.SerializeObject(model);

            return View(new ClientFixViewModel
            {
                Client = model.Client
            });
        }

        [HttpPost]
        public IActionResult CreateFix(ClientFixViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Fail"] = "Nie zgadza się";
                return View(model);
            }
            var modelClient = JsonConvert.DeserializeObject<ClientFixViewModel>((string)TempData["ClientModel"]);
            var usr = _userManager.GetUserAsync(User);
            model.Fix.EmpWhoAcceptID = usr.Result.Id;
            model.Fix.ClientID = modelClient.Client.ID;
            RandomBarcodeGenerator generator = new RandomBarcodeGenerator();
            Fix res;
            do
            {
                model.Fix.Barcode = generator.RandomString(10);
                res = _repoFix.Fixes.FirstOrDefault(p => p.Barcode == model.Fix.Barcode);
            } while (res != null);
            Accesory accesory = new Accesory();
            if (model.Fix.WhatAccesory != null)
            {
                var listOfAcc = model.Fix.WhatAccesory.Split(",");
                _repoFix.SaveFix(model.Fix);
                foreach (var a in listOfAcc)
                {
                    _repoAcc.SaveAccesory(new Accesory
                    {
                        NameOfAccesory = a,
                        FixID = model.Fix.ID,
                    });
                }
            }
            else
            {
                _repoFix.SaveFix(model.Fix);
            }
            var name = model.Fix.ID.ToString();
            //string htmlCode = System.IO.File.ReadAllText("Views/Client/PdfTemplate.cshtml");
            PdfMaker.CreatePdf(name, model.Fix.Barcode);
            return View();
        }
    }
}
