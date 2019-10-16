namespace HammondsIBMS_2.ViewModels.ModelView
{
    public class DeleteModelVM
    {
        public int ModelId { get; set; }
        public string DeleteMessage { get; set; }

        public string Prompt { get; set; }

        public bool MakeObsolete { get; set; }
    }
}