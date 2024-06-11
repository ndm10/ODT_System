using Microsoft.AspNetCore.Mvc;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsValidate(out Dictionary<string, string> validationErrors)
        {
            validationErrors = new Dictionary<string, string>();

            //validate input
            if (!ModelState.IsValid)
            {

                //Get all errors
                foreach (var modelStateEntry in ModelState)
                {
                    var propertyName = modelStateEntry.Key;
                    var errorMessages = modelStateEntry.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    //Add to validation errors
                    validationErrors.Add(propertyName.ToLower(), string.Join(", ", errorMessages));
                }

                // Return false if has validation errors
                return false;
            }

            // Return true if no validation errors
            return true;
        }
    }
}
