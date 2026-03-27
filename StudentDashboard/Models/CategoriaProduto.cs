using System;
using System.Collections.Generic;
using System.Text;

namespace StudentDashBoard.Models
{
    public class CategoriaProduto
    {
        public int CategoriaID { get; set; } = 0;
        public string Name { get; set; } = "All";
        public bool IsSelected { get; set; } // Optional if you want to track selection manually
    }
}
