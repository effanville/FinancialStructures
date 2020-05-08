namespace FinancialStructures.StatsMakers
{
    /// <summary>
    /// Name paired with a boolean to determine whether the name should be displayed or not.
    /// </summary>
    public class VisibleName
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public VisibleName()
        { }

        public VisibleName(string name, bool vis)
        {
            Visible = vis;
            Name = name;
        }
    }
}
