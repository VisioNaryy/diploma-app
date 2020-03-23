using System;
using Microsoft.AspNetCore.Http;

namespace DiplomaApp.API.Helpers
{
    //There is no need to create new instances of Extensions class, so it will be static
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            //two methods below allow to display Application-Error header and message
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        //age calculating method
        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
            age--;

            return age;
        }
    }
}