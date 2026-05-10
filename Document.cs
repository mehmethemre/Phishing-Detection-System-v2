public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    
    // Yabancı Anahtar (Foreign Key)
    public int UserId { get; set; }
    public User User { get; set; }
    public string AnalysisResult { get; set; }

}