using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Auriqo_Web_Api_Backend.Interfaces
{
   
public interface ICloudinaryService
{

public  Task <string> UploadImageAsync(IFormFile image);

public Task<string> UploadVideoAsync(IFormFile video);


public Task <string> UploadMultipleImageAsync(IFormFile imgArr);

}
}
