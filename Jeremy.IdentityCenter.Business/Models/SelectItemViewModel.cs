namespace Jeremy.IdentityCenter.Business.Models
{
    public class SelectItemViewModel
    {
        public SelectItemViewModel(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }
    }
}
