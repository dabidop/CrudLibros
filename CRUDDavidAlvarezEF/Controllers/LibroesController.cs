﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDDavidAlvarezEF.Models;

namespace CRUDDavidAlvarezEF.Controllers
{
    public class LibroesController : Controller
    {
        private readonly DblibrosContext _context;

        public LibroesController(DblibrosContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index(string buscar, string filtro)
        {
            var libros = from Libro in _context.Libros select Libro;

            if (!String.IsNullOrEmpty(buscar))
            {
                libros = libros.Where(s => s.Titulo!.Contains(buscar));
            }
            ViewData["FiltroNombre"] = String.IsNullOrEmpty(filtro) ? "NombreDescendente" : "";
            ViewData["FiltroFecha"] = filtro == "FechaAscendente" ? "FechaDescendente" : "FechaAscendente";
            switch (filtro)
            {
                case "NombreDescendente":
                    libros = libros.OrderByDescending(libro => libro.Titulo);
                    break;
                case "FechaDescendente":
                    libros = libros.OrderByDescending(libro => libro.Titulo);
                    break;
                case "FechaAscendente":
                    libros = libros.OrderBy(libro => libro.Titulo);
                    break;
                default:
                    libros = libros.OrderBy(libro => libro.Titulo);
                    break;
            }
            return View(await libros.ToListAsync());
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.CodigoCategoriaNavigation)
                .Include(l => l.NitEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
            return View();
        }

        // POST: Libroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
            return View(libro);
        }

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {
            if (id != libro.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Isbn))
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
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.CodigoCategoriaNavigation)
                .Include(l => l.NitEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'DblibrosContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(string id)
        {
          return _context.Libros.Any(e => e.Isbn == id);
        }
    }
}
