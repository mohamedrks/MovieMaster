using AutoMapper.Configuration.Annotations;
using AutoMapper;
using MovieMaster.Data.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieMaster.Service.ViewModel
{

    public class MovieViewModel
    {
        [JsonPropertyName("MovieTitle")]
        [Required(ErrorMessage = "MovieTitle is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Actors is required")]
        public string Actors { get; set; }
        public string ImdbRating { get; set; }
    }
}
