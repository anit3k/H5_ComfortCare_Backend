namespace ComfortCare.Data.Models
{
    public class CitizenDbModel : MongoBaseModel
    {
        public string CitizenName { get; set; }
        public int ResidenceId { get; set; }
        public string CitizenResidence { get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
    }
}
