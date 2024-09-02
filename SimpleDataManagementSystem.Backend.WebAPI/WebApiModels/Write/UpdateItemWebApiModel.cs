﻿using SimpleDataManagementSystem.Backend.WebAPI.Validators;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write
{
    public class UpdateItemWebApiModel
    {
        public string Nazivproizvoda { get; set; }
        public string Opis { get; set; }
        public string Datumakcije { get; set; }
        public string Nazivretailera { get; set; }

        [MaxFileSizeValidator(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        public IFormFile? URLdoslike { get; set; }

        [DecimalValidator]
        public string Cijena { get; set; }
        public int Kategorija { get; set; }
    }
}
