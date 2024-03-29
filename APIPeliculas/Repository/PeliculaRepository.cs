﻿using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APIPeliculas.Repository
{
	public class PeliculaRepository : IPeliculaRepository
	{
		private readonly ApplicationDbContext _bd;

		public PeliculaRepository(ApplicationDbContext bd)
		{
			_bd = bd;
		}

		public bool ActualizarPelicula(Pelicula pelicula)
		{
			_bd.Pelicula.Update(pelicula);
			return Guardar();
		}

		public bool BorrarPelicula(Pelicula pelicula)
		{
			_bd.Pelicula.Remove(pelicula);
			return Guardar();
		}

		public IEnumerable<Pelicula> BuscarPelicula(string nombre)
		{
			IQueryable<Pelicula> query = _bd.Pelicula;

			if(!string.IsNullOrEmpty(nombre))
			{
				query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
			}

			return query.ToList();
		}

		public bool CrearPelicula(Pelicula pelicula)
		{
			_bd.Pelicula.Add(pelicula);
			return Guardar();
		}

		public bool ExistePelicula(string nombre)
		{
			bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
			return valor;
		}

		public bool ExistePelicula(int id)
		{
			return _bd.Pelicula.Any((System.Linq.Expressions.Expression<System.Func<Pelicula, bool>>)(c => c.Id == id));
		}

		public Pelicula GetPelicula(int PeliculaId)
		{
			return _bd.Pelicula.FirstOrDefault( (System.Linq.Expressions.Expression<System.Func<Pelicula, bool>>)(c => c.Id == PeliculaId));
		}

		public ICollection<Pelicula> GetPeliculas()
		{
			return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
		}

		public ICollection<Pelicula> GetPeliculasEnCategoria(int CatId)
		{
			return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == CatId).ToList();
		}

		public bool Guardar()
		{
			return _bd.SaveChanges() >= 0 ? true : false;
		}
	}
}
