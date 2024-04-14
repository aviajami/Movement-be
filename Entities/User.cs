using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Movement_be.Entities
{
    public class User
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(12)]
        public string? Password { get; set; }

        [Required, MaxLength(50)]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        

        //public bool IsDeleted { get; set; }

    }
}
