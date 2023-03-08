namespace Delta.Api.Model
{
    [Serializable]
    public class Schema
    {
        public string label { get; set; }
        public string displayName { get; set; }
        public string dataType { get; set; }
        public bool required { get; set; }
        public bool primary { get; set; }
        public bool unique { get; set; }
        public bool foreignField { get; set; }
        public string foreignDisplayName { get; set; }
        public string referenceTable { get; set; }
        public string referenceIdColumn { get; set; }
        public string referenceValueColumn { get; set; }
        public bool autoGenerate { get; set; }
        public string autoFormat { get; set; }

        public Schema() { }
    }
}
