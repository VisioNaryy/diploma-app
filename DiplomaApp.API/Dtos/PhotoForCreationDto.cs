using System;
using Microsoft.AspNetCore.Http;

namespace DiplomaApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        //Photo that is uploaded 
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
            
        }
    }
}