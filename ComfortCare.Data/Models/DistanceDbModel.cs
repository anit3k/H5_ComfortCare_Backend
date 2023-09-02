namespace ComfortCare.Data.Models
{
    public class DistanceDbModel : MongoBaseModel
    {
        public int ResidenceOneId{ get; set; }
        public int ResidenceTwoId { get; set; }
        public double Distance { get; set; }
    }
}
