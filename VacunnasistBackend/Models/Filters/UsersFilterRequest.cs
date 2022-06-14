namespace VacunassistBackend.Models.Filters
{
    public class UsersFilterRequest
    {
        public bool? IsActive { get; set; }
        public bool? BelongsToRiskGroup { get; set; }
        public string? Role { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}