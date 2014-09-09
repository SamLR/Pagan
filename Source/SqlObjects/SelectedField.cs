namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a Field participating in the Selection
    /// </summary>
    class SelectedField
    {
        public string Alias { get; set; }
        public Field Field { get; set; }
    }
}