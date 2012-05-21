namespace LHASocialWork.Models.Shared.JSON
{
    public class JQueryAutoCompleteResponseModel
    {
        public JQueryAutoCompleteResponseModel(string id, string label)
        {
            this.id = id;
            this.label = label;
        }

// ReSharper disable InconsistentNaming, jQuery syntax
        public string id { get; set; }
// ReSharper restore InconsistentNaming
// ReSharper disable InconsistentNaming
        public string label { get; set; }
// ReSharper restore InconsistentNaming
    }
}