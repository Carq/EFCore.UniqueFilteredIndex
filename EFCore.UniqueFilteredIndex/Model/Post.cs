using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCore.UniqueFilteredIndex.Model
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }
    }
}
