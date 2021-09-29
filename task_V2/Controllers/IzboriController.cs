using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using task_V2.Models;
using task_V2.ViewModels;

namespace task_V2.Controllers
{
    public class IzboriController : Controller
    {
        private ApplicationDbContext ctx;
        private IHostingEnvironment hostingEnvironment;
        private readonly ILogger<IzboriController> _logger;
        public IzboriController(ApplicationDbContext _ctx, IHostingEnvironment _hostingEnvironment, ILogger<IzboriController> logger)
        {
            ctx = _ctx;
            hostingEnvironment = _hostingEnvironment;
            _logger = logger;
        }
        public IActionResult Rezultat()
        {
            var model = new RezultatVM()
            {
                gradovi = ctx.Gradovi.Select(x => new SelectListItem
                {
                    Text = x.ImeGrada,
                    Value = x.ID.ToString()
                }).ToList(),

            };
            if (model.gradovi.Count() == 0)
            {
                model.prikaz = false;
            }
            else
            {
                model.prikaz = true;
            }
            return View(model);
        }
        public IActionResult Index()
        {

            return View();
        }
        public bool Postoji(int gradid, int kand)
        {
            var rezultati = ctx.Rezultati.Where(x => x.GradoviID == gradid && x.KandidatiID == kand).Include(x => x.Kandidat).FirstOrDefault();
            if (rezultati == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public IActionResult Prikaz(int gradID)
        {
            var id = ctx.Gradovi.Find(gradID);
            for (int i = 1; i <= 5; i++)
            {
                if (!Postoji(gradID, i))
                {
                    Rezultati novirez = new Rezultati()
                    {
                        GradoviID = gradID,
                        isGreska = true,
                        KandidatiID = i,

                    };
                    ctx.Rezultati.Add(novirez);
                    ctx.SaveChanges();
                }
            }    
            var model = new RezultatiPrikazVM()
            {
                GradID = id.ID,
                Rezultati = ctx.Rezultati.Where(x => x.GradoviID == id.ID).Include(x => x.Kandidat).ToList()
            };
            decimal suma = 0;
            
            foreach (var item in model.Rezultati)
            {
                if (item.BrojGlasova > 0)
                    suma += item.BrojGlasova;
                else
                    item.isGreska = true;
            }
            foreach (var item in model.Rezultati)
            {
                if (item.BrojGlasova > 0) { 
                item.Procenat = item.BrojGlasova / suma * 100;
                item.Procenat = Math.Round(item.Procenat, 2, MidpointRounding.ToEven); }
                else
                    item.isGreska = true;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "files");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                string csvData = System.IO.File.ReadAllText(filePath);



                List<string> podaci = System.IO.File.ReadAllLines(filePath).ToList();


                foreach (var podatak in podaci)
                {
                    string[] rezultat = podatak.Split(",");
                    bool glasN = false;
                    //grad ako ne postoji dodat ako postoji odabrat iz baze
                    Gradovi tempGrad = new Gradovi();
                    Gradovi novigrad = new Gradovi();
                    //provjerit dal je string na rezultat[0]                            !!!!!!!!!!!!!!
                    bool gradstring = Int32.TryParse(rezultat[0], out int siti);
                    if (gradstring == true || rezultat[0] == "")
                    {
                        _logger.LogWarning("Format je pogrešan.Na prvom mjestu nije grad!");
                        continue;
                    }
                    Gradovi grad = ctx.Gradovi.Where(x => x.ImeGrada.Equals(rezultat[0])).FirstOrDefault();
                    if (grad != null)
                    {
                        tempGrad = grad;
                    }
                    else
                    {    //dodat grad ako ne postoji
                        novigrad = new Gradovi
                        {
                            ImeGrada = rezultat[0].ToString()
                        };
                        ctx.Gradovi.Add(novigrad);
                        ctx.SaveChanges();
                        tempGrad = novigrad;
                    }
                    var tempglasovi = 0;
                    //provjera glasova i kandidata
                    for (int i = 1; i < rezultat.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Rezultati novirezultat = new Rezultati();
                            var temprez = new Rezultati();
                            bool edit = false;
                            var izbor = rezultat[i];
                            Kandidati kand = ctx.Kandidati.Where(x => x.Kratica.Equals(izbor)).FirstOrDefault();
                            if (kand == null || tempGrad == null)
                            {
                                _logger.LogWarning("Format je pogrešan!");
                                continue;
                            }
                            temprez = ctx.Rezultati.Where(x => x.Grad.ImeGrada.Equals(tempGrad.ImeGrada)
                            && x.GradoviID == tempGrad.ID && x.KandidatiID == kand.ID).FirstOrDefault();
                            if (temprez == null)
                            {
                                novirezultat = null;
                            }
                            else
                            {
                                novirezultat = temprez;
                            }
                            if (novirezultat != null)
                            {
                                if (novirezultat.KandidatiID == kand.ID)
                                {
                                    edit = true;
                                    if (novirezultat.BrojGlasova > 0)
                                    {
                                        novirezultat.BrojGlasova += tempglasovi;

                                    }
                                    else
                                    {
                                        novirezultat.BrojGlasova = tempglasovi;
                                        novirezultat.isGreska = glasN = false ? false : true;
                                    }                            
                                }
                                else
                                {
                                    edit = false;
                                    novirezultat = new Rezultati();
                                    novirezultat.KandidatiID = kand.ID;
                                    novirezultat.BrojGlasova = tempglasovi;
                                    novirezultat.GradoviID = tempGrad.ID;
                                    novirezultat.isGreska = glasN = false ? false : true;
                                }
                            }
                            else
                            {
                                edit = false;
                                novirezultat = new Rezultati();
                                novirezultat.KandidatiID = kand.ID;
                                novirezultat.BrojGlasova = tempglasovi;
                                novirezultat.GradoviID = tempGrad.ID;
                                novirezultat.isGreska = glasN = false ? false : true;
                            }
                            if (edit == false)
                            {
                                ctx.Rezultati.Add(novirezultat);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                ctx.Rezultati.Attach(novirezultat);
                                ctx.Rezultati.Update(novirezultat);
                                ctx.SaveChanges();
                            }
                        }
                        else
                        {                     
                            bool brojglasova = Int32.TryParse(rezultat[i], out int glasovi);
                            if (brojglasova == false)
                            {
                                _logger.LogWarning("Format je pogrešan.Format nije u dobrom nizu!");
                                tempglasovi = 0;
                                glasN = true;
                            }
                            else
                            {
                                tempglasovi = glasovi;
                            }
                        }
                    }
                }
            }
            return View();
        }
    }
}
