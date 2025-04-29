using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using PetIsland.Models.Validation;

namespace PetIsland.Models;

public class GroupMemberModel
{
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public required string MSSV { get; set; }
    public string? ImageUrl { get; set; }
    public string? LinkFB { get; set; }
    public string? LinkLinkedin { get; set; }
    public string? LinkInstagram { get; set; }
    public string? LinkTwitter { get; set; }
    public string? LinkGithub { get; set; }
    public string? LinkThread { get; set; }

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; } //for further use
                                                //gr member > 10, need store info in db
}
