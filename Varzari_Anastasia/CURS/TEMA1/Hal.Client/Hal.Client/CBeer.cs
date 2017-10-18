using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hal.Client
{
    public class CBeers
    {
        [JsonProperty("TotalResults")]
        public int totalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int totalPages { get; set; }

        [JsonProperty("Page")]
        public int page { get; set; }

        [JsonProperty("_links")]
        public CBeersLinks beersLinks { get; set; }

        [JsonProperty("_embedded")]
        public CBeersEmbedded beersEmbedded { get; set; }
    }

    public class CBeersLinks
    {
        [JsonProperty("self")]
        public CBeersSelfLinks beersSelf { get; set; }

        [JsonProperty("page")]
        public List<CBeersPage> beersPage { get; set; }

        [JsonProperty("beer")]
        public List<CBeersList> beersList { get; set; }
    }

    public class CBeersList
    {
        [JsonProperty("href")]
        public string beerHref { get; set; }
    }
    public class CBeersPage
    {
        [JsonProperty("href")]
        public string beersHref { get; set; }

        [JsonProperty("templated")]
        public bool templated { get; set; }
    }

    public class CBeersSelfLinks
    {
        [JsonProperty("href")]
        public string beersSelfHref { get; set; }

    }

    public class CBeersEmbedded
    {
        [JsonProperty("beer")]
        public CBeer beer { get; set; }

    }

    public class CBeer
    {
        [JsonProperty("Id")]
        public int beerId { get; set; }

        [JsonProperty("Name")]
        public string beerName { get; set; }

        [JsonProperty("BreweryId")]
        public int breweryId { get; set; }

        [JsonProperty("BreweryName")]
        public string breweryName { get; set; }

        [JsonProperty("StyleId")]
        public int styleId { get; set; }

        [JsonProperty("StyleName")]
        public string styleName { get; set; }

        [JsonProperty("_links")]
        public CBeerLinks beerLinks { get; set; }

    }

    public class CBeerLinks
    {
        [JsonProperty("self")]
        public CBeerSelfLinks beerSelf { get; set; }

        [JsonProperty("style")]
        public CBeerStyleLinks beerStyle { get; set; }

        [JsonProperty("brewery")]
        public CHomeBreweryLinks beerBreweryLinks { get; set; }

    }

    public class CBeerSelfLinks
    {
        [JsonProperty("href")]
        public string beerSelfHref { get; set; }
    }

    public class CBeerStyleLinks
    {
        [JsonProperty("href")]
        public string beerStyleHref { get; set; }
    }
}
