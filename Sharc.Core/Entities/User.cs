namespace Sharc.Core.Entities; 

public class User {
    
    public Guid Id { get; set; }
    
    public string IdentityId { get; set; }
    
    public string CachedUsername { get; set; }
}