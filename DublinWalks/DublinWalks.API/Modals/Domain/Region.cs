namespace DublinWalks.API.Modals.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
        //Navigation property
       // public IEnumerable<Walk> Walks { get; set; }



    }
}
