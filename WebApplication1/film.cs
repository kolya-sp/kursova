﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebApplication1
{
    public class film
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public class reit
        {
            public string Source { get; set; }
            public string Value { get; set; }
        }
        List<reit> Ratings {get;set;}
        public string imdbRating { get; set; }
        public string Type { get; set; }
        public string DVD { get; set; }
        public string Production { get; set; }
    }
}
