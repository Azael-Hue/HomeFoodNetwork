using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFoodNetwork.Models
{
    public class RecipeSteps
    {
        public int Id { get; set; }

        /// <summary>
        /// Foreign key for the recipe
        /// </summary>
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        /// <summary>
        /// Order of the step in the recipe
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// Description/Detail of the step
        /// </summary>
        public string StepDescription { get; set; }

        /// <summary>
        /// Navigation property for the recipe
        /// </summary>
        public Recipe Recipe { get; set; }
    }
}
