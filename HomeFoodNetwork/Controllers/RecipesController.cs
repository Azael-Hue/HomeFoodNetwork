
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
using System.Net;

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
                    TotalSteps = r.RecipeSteps.Count(),
                    TotalTime = r.TotalTime,
                    ServingSize = r.ServingSize,
                    Difficulty = r.Difficulty
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
                .Include(r => r.RecipeSteps) // Specifies the related objects to include in the query results.
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
                    // REFACTOR THIS ONCE RECIPE STEPS ARE IMPLEMENTED
                    // NumSteps = recipe.NumSteps,
                    CookTime = $"{recipe.CookTimeHours} hours {recipe.CookTimeMinutes} minutes",
                    PrepTime = $"{recipe.PrepTimeHours} hours {recipe.PrepTimeMinutes} minutes",
                    TotalTime = recipe.TotalTime,
                    ServingSize = recipe.ServingSize,
                    Difficulty = recipe.Difficulty,
                    User = await _userManager.GetUserAsync(User), // Get the current user
                    RecipeSteps = recipe.RecipeSteps.Select(s => new RecipeSteps
                    {
                        StepNumber = s.StepNumber,
                        StepDescription = s.StepDescription
                    }).ToList()
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

            var recipe = await _context.Recipe
                .Include(r => r.RecipeSteps)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            RecipeCreateViewModel recipeViewModel = new()
            {
                Id = recipe.Id,
                RecipeName = recipe.RecipeName,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients,
                // REFACTOR THIS ONCE RECIPE STEPS ARE IMPLEMENTED
                // NumSteps = recipe.NumSteps,
                CookTimeHours = int.Parse(recipe.CookTime.Split(" ")[0]),
                CookTimeMinutes = int.Parse(recipe.CookTime.Split(" ")[2]),
                PrepTimeHours = int.Parse(recipe.PrepTime.Split(" ")[0]),
                PrepTimeMinutes = int.Parse(recipe.PrepTime.Split(" ")[2]),
                ServingSize = recipe.ServingSize,
                Difficulty = recipe.Difficulty,
                RecipeSteps = recipe.RecipeSteps.Select(s => new RecipeStepCreateViewModel
                {
                    StepNumber = s.StepNumber,
                    StepDescription = s.StepDescription
                }).ToList()
            };
            return View(recipeViewModel);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = IdentityHelper.User)]
        public async Task<IActionResult> Edit(int id, RecipeCreateViewModel recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Recipe recipeToEdit = await _context.Recipe
                        .Include(r => r.RecipeSteps)
                        .FirstOrDefaultAsync(r => r.Id == id);

                    recipeToEdit.RecipeName = recipe.RecipeName;
                    recipeToEdit.Description = recipe.Description;
                    recipeToEdit.Ingredients = recipe.Ingredients;
                    // REFACTOR THIS ONCE RECIPE STEPS ARE IMPLEMENTED
                    // recipeToEdit.NumSteps = recipe.NumSteps;
                    recipeToEdit.CookTime = $"{recipe.CookTimeHours} hours {recipe.CookTimeMinutes} minutes";
                    recipeToEdit.PrepTime = $"{recipe.PrepTimeHours} hours {recipe.PrepTimeMinutes} minutes";
                    recipeToEdit.TotalTime = recipe.TotalTime;
                    recipeToEdit.ServingSize = recipe.ServingSize;
                    recipeToEdit.Difficulty = recipe.Difficulty;

                    // Update the steps
                    recipeToEdit.RecipeSteps = recipe.RecipeSteps.Select(s => new RecipeSteps
                    {
                        StepNumber = s.StepNumber,
                        StepDescription = s.StepDescription
                    }).ToList();

                    _context.Update(recipeToEdit);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        [Authorize(Roles = IdentityHelper.User)]
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
