namespace UDV_TEST.Entity
{
    public class Chat_List_item
    {
        public required int Id { get; set; }
        public required string Header { get; set; }
        public string? Author { get; set; }
        public string? Message { get; set; }
        public string? Date { get; set; }
    }
}
