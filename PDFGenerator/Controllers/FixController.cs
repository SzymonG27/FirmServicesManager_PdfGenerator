using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDFGenerator.Models.AccountModels;
using PDFGenerator.Models.ClientModels;
using PDFGenerator.Models.ViewModels;
using PDFGenerator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Controllers
{
    public class FixController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IClientRepository _repo;
        private readonly IFixRepository _repoFix;
        private readonly IFirmRepository _repoFirm;
        private readonly IAccesoryRepository _repoAcc;
        private readonly IClientFirmRelationRepository _repoRel;
        public FixController(UserManager<AppUser> userManager, IClientRepository repo, IFixRepository repoFix,
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
        public async Task<IActionResult> CreateFix()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
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



            _repoFix.SaveFix(model.Fix);
            var name = model.Fix.ID.ToString();
            Accesory accesory = new Accesory();
            if (model.Fix.WhatAccesory != null)
            {
                var listOfAcc = model.Fix.WhatAccesory.Split(",");
                foreach (var a in listOfAcc)
                {
                    _repoAcc.SaveAccesory(new Accesory
                    {
                        NameOfAccesory = a,
                        FixID = model.Fix.ID,
                    });
                }
            }
            List<string> lsOfFix = new List<string>();
            if (model.Fix.WhatToFix != null)
            {
                var listOfFix = model.Fix.WhatToFix.Split(",");
                foreach (var a in listOfFix)
                {
                    lsOfFix.Add(a);
                }
            }
            //string htmlCode = System.IO.File.ReadAllText("Views/Client/PdfTemplate.cshtml");
            var resFirmRel = _repoRel.ClientFirmRelations
                .FirstOrDefault(p => p.ClientID == modelClient.Client.ID);
            PdfMakerModel pdfView;
            Firm firm = null;
            var accesories = _repoAcc.Accesories
                .Where(p => p.FixID == model.Fix.ID);
            if (resFirmRel != null)
            {
                var resFirm = _repoFirm.Firms
                    .FirstOrDefault(p => p.ID == resFirmRel.FirmID);
                resFirm = firm;
            }
            pdfView = new PdfMakerModel
            {
                Client = modelClient.Client,
                Fix = model.Fix,
                Firm = firm,
                Accesories = accesories,
                AppUser = usr.Result,
                FixNames = lsOfFix
            };

            PdfMaker.CreatePdf(name, pdfView);
            //PdfMaker.CreatePdf(name, model.Fix.Barcode, htmlCode);
            var stream = new FileStream(@"PDFs/" + name + ".pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ListOfFix()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            var fixes = _repoFix.Fixes
                    .OrderByDescending(p => p.DateOfAdmission);
            return View(new FixesViewModel
            {
                Fixes = fixes
            });
        }

        [HttpPost]
        public async Task<IActionResult> ListOfFix(FixesViewModel model)
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            TempData["FixModel"] = JsonConvert.SerializeObject(model);
            var resBar = _repoFix.Fixes
                .Where(p => p.Barcode == model.barcode);
            if (resBar != null)
            {
                return View(new FixesViewModel
                {
                    Fixes = resBar
                });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditFix()
        {
            var fixIDStr = Request.Query["ID"];
            var fixID = Convert.ToInt32(fixIDStr);
            var usrApp = await _userManager.GetUserAsync(User);
            
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            var resUpt = _repoFix.Fixes
                .FirstOrDefault(f => f.ID == fixID);
            if (resUpt != null)
            {
                FixesViewModel fix = new FixesViewModel
                {
                    Fix = resUpt
                };
                return View(fix);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditFix(FixesViewModel fixEdit)
        {
            var resUpt = _repoFix.Fixes
                .FirstOrDefault(f => f.ID == fixEdit.Fix.ID);
            if (resUpt != null)
            {
                _repoFix.SaveFix(fixEdit.Fix);
                TempData["Success"] = "Zedytowałeś naprawę nr. " + fixEdit.Fix.ID;
                return RedirectToAction("ListOfFix");
            }
            TempData["Fail"] = "Edytowanie nie powiodło się";
            return View(fixEdit);
        }
    }
}
