using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeFoodNetwork.Data;
using HomeFoodNetwork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeFoodNetwork.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            List<RecipeIndexViewModel> recipeList = await (from r in _context.Recipe
                select new RecipeIndexViewModel
                {
                    Id = r.Id,
                    RecipeName = r.RecipeName,
                    User = r.User.UserName,
                    numSteps = r.NumSteps,
                    totalTime = r.TotalTime,
                    servingSize = r.ServingSize,
                    difficulty = r.Difficulty
                }).ToListAsync();

            return View(recipeList);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize(Roles = IdentityHelper.User)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = IdentityHelper.User)]
        public async Task<IActionResult> Create(RecipeCreateViewModel recipe)
        {
            if (ModelState.IsValid)
            {

                Recipe newRecipe = new()
                {
                    RecipeName = recipe.RecipeName,
                    Description = recipe.Description,
                    Ingredients = recipe.Ingredients,
                    NumSteps = recipe.NumSteps,
                    CookTime = $"{recipe.CookTimeHours} hours {recipe.CookTimeMinutes} minutes",
                    PrepTime = $"{recipe.PrepTimeHours} hours {recipe.PrepTimeMinutes} minutes",
                    TotalTime = recipe.TotalTime,
                    ServingSize = recipe.ServingSize,
                    Difficulty = recipe.Difficulty,
                    User = await _userManager.GetUserAsync(User), // Get the current user
                };

                _context.Add(newRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        [Authorize(Roles = IdentityHelper.User)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = IdentityHelper.User)]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        [Authorize(Roles = IdentityHelper.User)]
        [Authorize(Roles = IdentityHelper.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityHelper.User)]
        [Authorize(Roles = IdentityHelper.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
