using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinjulMSBH_Blog.Data;
using SinjulMSBH_Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SinjulMSBH_Blog.Services;

namespace SinjulMSBH_Blog.Controllers
{
	public class ArticlesController: Controller
	{
		private readonly IArticlesRepository _articlesRepository;
		private readonly IRequestUserProvider _requestUserProvider;

		public ArticlesController ( )
		{
		}

		public ArticlesController ( IArticlesRepository articlesRepository , IRequestUserProvider requestUserProvider )
		{
			_articlesRepository = articlesRepository;
			_requestUserProvider = requestUserProvider;
		}

		// GET: /<controller>/
		public IActionResult List ( )
		{
			var hello = new Hello("Sinjul", "MSBH");
			Console.WriteLine( hello.HelloMan( ) );
			Console.ReadLine( );

			return View( "Index" );
		}

		// GET: Articles
		public async Task<IActionResult> Index ( )
		{
			return View( await _articlesRepository.GetAll( ) );
		}

		// GET: Articles/Details/5
		public async Task<IActionResult> Details ( int? id )
		{
			if ( id == null )
			{
				return NotFound( );
			}

			var article = await _articlesRepository.GetOne(id.Value);
			if ( article == null )
			{
				return NotFound( );
			}

			return View( article );
		}

		// GET: Articles/Create
		[Authorize]
		public IActionResult Create ( )
		{
			return View( );
		}

		// POST: Articles/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create ( [Bind( "Title, Abstract,Contents" )] Article article )
		{
			if ( ModelState.IsValid )
			{
				article.AuthorId = _requestUserProvider.GetUserId( );
				article.CreatedDate = DateTime.Now;
				_articlesRepository.Add( article );
				await _articlesRepository.SaveChanges( );
				return RedirectToAction( "Index" );
			}
			return View( article );
		}

		// GET: Articles/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete ( int? id )
		{
			if ( id == null )
			{
				return NotFound( );
			}

			var article = await _articlesRepository.GetOne(id.Value);
			if ( article == null )
			{
				return NotFound( );
			}

			return View( article );
		}

		// POST: Articles/Delete/5
		[HttpPost, ActionName( "Delete" )]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed ( int id )
		{
			var article = await _articlesRepository.GetOne(id);
			_articlesRepository.Remove( article );
			await _articlesRepository.SaveChanges( );
			return RedirectToAction( "Index" );
		}
	}
}