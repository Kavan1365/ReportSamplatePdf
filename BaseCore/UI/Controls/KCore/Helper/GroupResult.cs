﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BaseCore.UI.Controls.KCore.Helper
{
    public class GroupResult
    {
        //for more info check http://docs.telerik.com/Mvc-ui/api/javascript/data/datasource#configuration-schema.groups

        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "aggregates")]
        public object Aggregates { get; set; }

        [JsonProperty(PropertyName = "items")]
        public IEnumerable Items { get; set; }

        [JsonProperty(PropertyName = "hasSubgroups")]
        public bool HasSubgroups { get; set; }

    }
}