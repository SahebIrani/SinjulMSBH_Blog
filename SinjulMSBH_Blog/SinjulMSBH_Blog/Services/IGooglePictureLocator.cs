using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SinjulMSBH_Blog.Services
{
    public interface IGooglePictureLocator
    {
        Task<string> GetProfilePictureAsync(ExternalLoginInfo info);
    }
}