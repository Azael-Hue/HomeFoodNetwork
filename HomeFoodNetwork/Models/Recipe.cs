using System.ComponentModel.DataAnnotations;

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
        public string NumSteps { get; set; }

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
        public string ServingSize { get; set; }

        /// <summary>
        /// How difficult the recipe is to make on a 1-5 scale
        /// </summary>
        [Required]
        public int Difficulty { get; set; }
    }
}
