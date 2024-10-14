namespace Manager.Model.Models.Location
{
    public class Province
    {
        public string Code { get; set; }
        public string Full_Name { get; set; }
    }

    public class District
    {
        public string Code { get; set; }
        public string Full_Name { get; set; }
        public string Province_Code { get; set; }
    }

    public class Ward
    {
        public string Code { get; set; }
        public string Full_Name { get; set; }
        public string District_Code { get; set; }
    }
}
