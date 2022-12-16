using AutoMapper;
using AutoMapper.Configuration.Annotations;
using MovieMaster.Data.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieMaster.Service.Model
{
    [AutoMap(typeof(MovieDto))]
    public class Movie
    {
        public string Id { get; set; }

        [SourceMember(nameof(MovieDto.Title))]
        public string MovieTitle { get; set; }
        public string Year { get; set; }
        public string Actors { get; set; }
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("Rating")]
        public string ImdbRating { get; set; }
    }
}