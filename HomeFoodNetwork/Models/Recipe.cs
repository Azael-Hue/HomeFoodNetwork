using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFoodNetwork.Models
{
    public class Recipe
    {

        /// <summary>
        /// The unique identifier for the recipe
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the recipe
        /// </summary>
        [Required]
        public string RecipeName { get; set; }

        /// <summary>
        /// A brief description of the recipe
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The ingredients needed for the recipe
        /// </summary>
        [Required]
        public string Ingredients { get; set; }

        /// <summary>
        /// The amount of steps requiered to make the recipe
        /// </summary>
        [Required]
        public int NumSteps { get; set; }

        /// <summary>
        /// The amount of time needed to cook the recipe
        /// </summary>
        [Required]
        public string CookTime { get; set; }

        /// <summary>
        /// The amount of time needed to prepare the recipe
        /// </summary>
        [Required]
        public string PrepTime { get; set; }

        /// <summary>
        /// The total time needed to make the recipe (a sum of prep time and cook time)
        /// </summary>
        [Required]
        public string TotalTime { get; set; }

        /// <summary>
        /// The amount of food served by the recipe
        /// </summary>
        [Required]
        public int ServingSize { get; set; }

        /// <summary>
        /// How difficult the recipe is to make on a 1-5 scale
        /// </summary>
        [Required]
        public string Difficulty { get; set; }

        /// <summary>
        /// Who created the recipe
        /// </summary>
        [ForeignKey("UserId")]
        public required IdentityUser User { get; set; }
    }

    public class RecipeCreateViewModel
    {
        public int Id { get; set; }

        public string RecipeName { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public int NumSteps { get; set; }

        // Time properties
        /// <summary>
        /// These 4 properties are used to store the
        /// hours and minutes of the cook and prep time
        /// </summary>
        public int CookTimeHours { get; set; }
        public int CookTimeMinutes { get; set; }
        public int PrepTimeHours { get; set; }
        public int PrepTimeMinutes { get; set; }

        public string TotalTime
        {
            get
            {
                int totalHours = CookTimeHours + PrepTimeHours;
                int totalMinutes = CookTimeMinutes + PrepTimeMinutes;

                if (totalMinutes >= 60)
                {
                    totalHours += totalMinutes / 60;
                    totalMinutes = totalMinutes % 60;
                }
                return $"{totalHours} hours {totalMinutes} minutes";
            }
        }

        public int ServingSize { get; set; }

        public string Difficulty { get; set; }

    }

    public class RecipeIndexViewModel
    {
        public int Id { get; set; }

        public string RecipeName { get; set; }

        public string User { get; set; }

        public int numSteps { get; set; }

        public string totalTime { get; set; }

        public int servingSize { get; set; }

        public string Difficulty { get; set; }
    }
}