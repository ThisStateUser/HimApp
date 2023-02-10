//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HimApp.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int id { get; set; }
        public Nullable<int> executor_id { get; set; }
        public int order_group_id { get; set; }
        public int client_car_id { get; set; }
        public int status_id { get; set; }
        public Nullable<int> condition_id { get; set; }
        public string comments { get; set; }
        public double custom_cost { get; set; }
        public Nullable<double> prepayment { get; set; }
        public Nullable<System.DateTime> arrival_date { get; set; }
        public Nullable<System.DateTime> departure_date { get; set; }
    
        public virtual ClientCar ClientCar { get; set; }
        public virtual Conditions Conditions { get; set; }
        public virtual OrderGroup OrderGroup { get; set; }
        public virtual Status Status { get; set; }
        public virtual Users Users { get; set; }
    }
}
