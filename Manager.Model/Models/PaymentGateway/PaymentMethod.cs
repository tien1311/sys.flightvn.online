namespace Manager.Model.Models.PaymentGateway
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public double Percent { get; set; }
        public double FixedCosts { get; set; }
        public string Source { get; set; }
        public bool IsActived { get; set; }
    }
}
