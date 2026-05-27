using System.Collections.Generic;

namespace phishing
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        
        // 1-N İlişkisi: Bir kullanıcının birden fazla dökümanı olabilir.
        public ICollection<Document> Documents { get; set; }
    }
}