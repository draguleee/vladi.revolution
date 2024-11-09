using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vladi.revolution.Models;
using vladi.revolution.Data.Enums;

namespace vladi.revolution.Data.ViewModels
{
    public class NewPlayerVM
    {
        public int Id { get; set; }

        [Display(Name = "Poză Profil")]
        [Required(ErrorMessage = "Selectează o poză!")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Nume Complet")]
        [Required(ErrorMessage = "Numele complet este obligatoriu!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Numele complet trebuie să conțină între 5 și 50 de caractere!")]
        public string FullName { get; set; }

        [Display(Name = "Data Nașterii")]
        [Required(ErrorMessage = "Data nașterii este obligatorie!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly BirthDate { get; set; }

        [Display(Name = "Vârsta")]
        public int Age
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                int age = today.Year - BirthDate.Year;
                if (today < BirthDate.AddYears(age)) age--;
                return age;
            }
        }

        [Display(Name = "Poziție")]
        [Required(ErrorMessage = "Poziția jucătorului este obligatorie!")]
        public List<Positions> Position { get; set; }

        [Display(Name = "Număr tricou")]
        [Required(ErrorMessage = "Numărul de pe tricou este obligatoriu!")]
        public int ShirtNumber { get; set; }

        [Display(Name = "Facebook (Opțional)")]
        public string? FacebookAccount { get; set; }

        [Display(Name = "Instagram (Opțional)")]
        public string? InstagramAccount { get; set; }

        public List<Transfer>? Transfers { get; set; }
    }
}
