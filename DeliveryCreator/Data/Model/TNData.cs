//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliveryCreator.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TNData
    {
        public int id { get; set; }
        public string sender_full_name { get; set; }
        public string cargo_name { get; set; }
        public string cargo_docs { get; set; }
        public string transport_type { get; set; }
        public string cargo_from_name { get; set; }//delete
        public string cargo_from_condition { get; set; }
        public string cargo_from_manager_position { get; set; }
        public string cargo_to_name { get; set; }
        public string transportation_condition { get; set; }
        public string transporter_name { get; set; }
        public string car_name { get; set; }
        public string car_number { get; set; }
        public string price_info { get; set; }
        public string receiver_full_name { get; set; }
        public string cargo_transporter_fio { get; set; }
    }
}
