using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PuncherPlus.Models;
using Dapper;
using static NuGet.Packaging.PackagingConstants;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace PuncherPlus.Controllers
{
    public class DpointersController : Controller
    {
        private readonly PuncherplusContext _context;

        public DpointersController(PuncherplusContext context)
        {
            _context = context;
        }



        // GET: Dpointers
        public async Task<IActionResult> Index(string buscar)
        {
            DateTime fechaDelDia = DateTime.Now;
            fechaDelDia = fechaDelDia.AddMonths(-1);
            List<tablaDpointer> parts = new List<tablaDpointer>();
            
            var dpointerList = await (from muser in _context.Musers
                        join dpointer in _context.Dpointers on muser.Id equals dpointer.IdUser
                        where dpointer.CreateAt > fechaDelDia
                        orderby dpointer.CreateAt descending
                        select new tablaDpointer
                        {
                            Nick = muser.Nick,
                            Id =    dpointer.Id,
                            CreateAt = dpointer.CreateAt,
                            Motivo = dpointer.Motivo,
                            IdUser = dpointer.IdUser,
                            pointerType = dpointer.pointerType,
                            latepunch = dpointer.latepunch
                        }).ToListAsync();
      
            return View(dpointerList);


            //var UsuarioPuncher = from tablaDpointer in _context.Dpointers select tablaDpointer;

            // if (!string.IsNullOrEmpty(buscar))
            // {
            //     var buscar1 = int.Parse(buscar);
            //     UsuarioPuncher = UsuarioPuncher.Where(s => s.IdUser.Equals(buscar1));
            // }
            // DateTime fechaDelDia= DateTime.Now;
            // fechaDelDia = fechaDelDia.AddMonths(-1);
            // UsuarioPuncher = UsuarioPuncher.Where(x => x.CreateAt > fechaDelDia );            
            //var puncherDataList = await UsuarioPuncher.OrderByDescending(clase => clase.CreateAt).ToListAsync();



            //return View(tdp);
        }

        // GET: Dpointers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Dpointers == null)
            {
                return NotFound();
            }

            var dpointer = await _context.Dpointers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dpointer == null)
            {
                return NotFound();
            }

            return View(dpointer);
        }

        // GET: Dpointers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dpointers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Dpointer dpointer)
        {
            
            try
            {
                var sql2 = "INSERT INTO dpointer (motivo,idUser,pointerType)" +
                    "VALUES ('" + dpointer.Motivo + "'," + dpointer.IdUser + ",(select RegistroPointerType(" + dpointer.IdUser + ")))";

                //var sql3 = "UPDATE dpointer SET inTime = (select semaforo(" + dpointer.IdUser + "))";
                
                await _context.Database.ExecuteSqlRawAsync(sql2);
                var sql4 = await _context.Database.ExecuteSqlRawAsync("UPDATE dpointer SET latepunch =(SELECT LatePunch(" + dpointer.IdUser + ")) WHERE idUser = " + dpointer.IdUser + " AND DAY(createAT) = DAY(NOW()) ORDER BY id DESC LIMIT 1");
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                throw;
            }
                                                                                    
          
            return View(dpointer);          


        }



        // GET: Dpointers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Dpointers == null)
            {
                return NotFound();
            }

            var dpointer = await _context.Dpointers.FindAsync(id);
            if (dpointer == null)
            {
                return NotFound();
            }
            return View(dpointer);
        }

        // POST: Dpointers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CreateAt,Motivo,IdUser,pointerType")] Dpointer dpointer)
        {
            if (id != dpointer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dpointer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DpointerExists(dpointer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dpointer);
        }

        // GET: Dpointers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Dpointers == null)
            {
                return NotFound();
            }

            var dpointer = await _context.Dpointers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dpointer == null)
            {
                return NotFound();
            }

            return View(dpointer);
        }

        // POST: Dpointers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Dpointers == null)
            {
                return Problem("Entity set 'PuncherplusContext.Dpointers'  is null.");
            }
            var dpointer = await _context.Dpointers.FindAsync(id);
            if (dpointer != null)
            {
                _context.Dpointers.Remove(dpointer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DpointerExists(long id)
        {
            return (_context.Dpointers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<Muser>> GetAllUser()
        {
            var userList = await _context.Musers.Where(x => x.DeletedAt == null).ToListAsync();
            return userList;
            //var db = dbConnection();
            //var sql = @"select ID,Nombre,Apellido FROM usuario";
            //return await db.QueryAsync<User>(sql, new { });
        }

        public async Task<IActionResult> printTable(int? id)
        {
        
            DateTime fechaDelDia = DateTime.Now;
            fechaDelDia = fechaDelDia.AddMonths(-12);
            List<tablaDpointer> parts = new List<tablaDpointer>();            
            var dpointerList = await (from muser in _context.Musers
                                      join dpointer in _context.Dpointers on muser.Id equals dpointer.IdUser
                                      where dpointer.IdUser == id
                                      where dpointer.CreateAt > fechaDelDia
                                      orderby dpointer.CreateAt descending
                                      select new tablaDpointer
                                      {
                                          Nick = muser.Nick,
                                          Id = dpointer.Id,
                                          CreateAt = dpointer.CreateAt,
                                          Motivo = dpointer.Motivo,
                                          IdUser = dpointer.IdUser,
                                          pointerType = dpointer.pointerType,
                                      }).ToListAsync();
            var GivenName = await (from muser in _context.Musers
                                 where muser.Id == id
                                 select muser.GivenName
                                 ).FirstOrDefaultAsync();
            ViewBag.nickName = GivenName;
            return View(dpointerList);

        }
    }
}
