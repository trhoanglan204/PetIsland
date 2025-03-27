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

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
}
