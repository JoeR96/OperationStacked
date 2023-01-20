namespace OperationStacked.Models.A2S
{
    public abstract class A2SBlockTemplateValue
    {
        public int[] amrapRepTarget { get; set; }
        public int[] repsPerSet { get; set; }
        public decimal[] intensity { get; set; }
        public int sets { get; set; }
        public bool aux { get; set; }
    }
}
