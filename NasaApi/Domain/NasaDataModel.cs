namespace NasaApi.Domain
{
    public class Collection
    {
        public string? Version { get; set; }
        public string? Href { get; set; }
        public List<NasaLineItem>? Items { get; set; }
        public Metadata? Metadata { get; set; }
        public List<Link>? Links { get; set; }
    }

    public class Datum
    {
        public string? Center { get; set; }
        public string? Title { get; set; }
        public string? Nasa_id { get; set; }
        public DateTime Date_created { get; set; }
        public List<string>? Keywords { get; set; }
        public string? Media_type { get; set; }
        public string? Description_508 { get; set; }
        public string? Secondary_creator { get; set; }
        public string? Description { get; set; }
        public string? Photographer { get; set; }
        public List<string>? Album { get; set; }
        public string? Location { get; set; }
    }

    public class NasaLineItem
    {
        public string? Href { get; set; }
        public List<Datum>? Data { get; set; }
        public List<Link>? Links { get; set; }
    }

    public class Link
    {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Render { get; set; }
        public string? Prompt { get; set; }
    }

    public class Metadata
    {
        public int Total_hits { get; set; }
    }

    public class NasaDataModel
    {
        public Collection? Collection { get; set; }
    }


}
