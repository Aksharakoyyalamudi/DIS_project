namespace DataGov_API_Intro_6.Models
{
    public class dummy
    {
    }
}

public class Rootobject
{
    public Class1[] Property1 { get; set; }
}

public class Class1
{
    public int fdcId { get; set; }
    public string description { get; set; }
    public string dataType { get; set; }
    public string publicationDate { get; set; }
    public string foodCode { get; set; }
    public Foodnutrient[] foodNutrients { get; set; }
    public string ndbNumber { get; set; }
}

public class Foodnutrient
{
    public string number { get; set; }
    public string name { get; set; }
    public float amount { get; set; }
    public string unitName { get; set; }
    public string derivationCode { get; set; }
    public string derivationDescription { get; set; }
}
