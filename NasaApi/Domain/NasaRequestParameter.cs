using System.ComponentModel;

namespace NasaApi.Domain
{
    public class NasaRequestParameter
    {
        /// <summary>
        /// Search for key
        /// </summary>
        [DefaultValue("mars")]
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Starting year
        /// </summary>
        public string? YearStart { get; set; }

        /// <summary>
        /// End year
        /// </summary>
        public string? YearEnd { get; set; }

        /// <summary>
        /// Media type: image/audio
        /// </summary>
        public MediaType? MediaType { get; set; }

        /// <summary>
        /// Page 
        /// </summary>
        public  int? Page { get; set; }

    }
}
