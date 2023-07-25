using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly OniqueContext _context;

        public EmployeeController(OniqueContext context)
        {
            _context = context;
        }

        // GET: TestEmployees
        public  IActionResult Index()
        {
            List<EmployeeListDto> dto = (from e in _context.Employees
                                              join el in _context.EmployeeLevel
                                              on e.Position equals el.EmployeeLevelId
                                              select new EmployeeListDto
                                              {
                                                  EmployeeId = e.EmployeeId,
                                                  PhotoPath = e.PhotoPath,
                                                  EmployeeName = e.EmployeeName,
                                                  DateOfBirth = (DateTime)e.DateOfBirth,
                                                  Gender = e.Gender ? "女" : "男",
                                                  Phone = e.Phone,
                                                  Email = e.Email,
                                                  EmployeeLevelName = el.EmployeeLevelName
                                              }).ToList();
            return View(dto);
        }
       

        // GET: TestEmployees/Create
        public IActionResult Create()
        {
            ViewData["Areas"] = new SelectList(_context.Areas, "AreaId", "AreaName");
            ViewData["Citys"] = new SelectList(_context.Citys, "CityId", "CityName");
            ViewData["Position"] = new SelectList(_context.EmployeeLevel, "EmployeeLevelId", "EmployeeLevelName");
            return View();
        }

        // POST: TestEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employees employees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Areas"] = new SelectList(_context.Areas, "AreaId", "AreaName", employees.Areas);
            ViewData["Citys"] = new SelectList(_context.Citys, "CityId", "CityName", employees.Citys);
            ViewData["Position"] = new SelectList(_context.EmployeeLevel, "EmployeeLevelId", "EmployeeLevelName", employees.Position);
            return View(employees);
        }

        // GET: TestEmployees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            ViewData["Areas"] = new SelectList(_context.Areas, "AreaId", "AreaName", employees.Areas);
            ViewData["Citys"] = new SelectList(_context.Citys, "CityId", "CityName", employees.Citys);
            ViewData["Position"] = new SelectList(_context.EmployeeLevel, "EmployeeLevelId", "EmployeeLevelName", employees.Position);
            return View(employees);
        }

        // POST: TestEmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,Password,Phone,Email,Gender,Citys,Areas,Address,DateOfBirth,Position,VerificationCode,Verification,PhotoPath,RegisterDate")] Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.EmployeeId))
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
            ViewData["Areas"] = new SelectList(_context.Areas, "AreaId", "AreaName", employees.Areas);
            ViewData["Citys"] = new SelectList(_context.Citys, "CityId", "CityName", employees.Citys);
            ViewData["Position"] = new SelectList(_context.EmployeeLevel, "EmployeeLevelId", "EmployeeLevelName", employees.Position);
            return View(employees);
        }

        // GET: TestEmployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.AreasNavigation)
                .Include(e => e.CitysNavigation)
                .Include(e => e.PositionNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: TestEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'OniqueContext.Employees'  is null.");
            }
            var employees = await _context.Employees.FindAsync(id);
            if (employees != null)
            {
                _context.Employees.Remove(employees);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
          return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
