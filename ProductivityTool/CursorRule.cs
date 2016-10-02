namespace ProductivityTool
{
    internal class CursorRule
    {
        public string Name { get; set; }
        public string WindowTitle { get; set; }
        public string ProcessName { get; set; }
        public ComparisonType ComparisonType { get; set; }
        public ActivationMethod ActivationMethod { get; set; }
        public ParentSearchMode ParentSearchMode { get; set; }
        public long Duration { get; set; }

        public bool Expired { get; set; }

        public bool IsProcessMode
        {
            get { return string.IsNullOrWhiteSpace(WindowTitle); }
        }
    }
}