//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StutentsKyrs
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public Users()
        {
            this.Attendance = new HashSet<Attendance>();
            this.Distiplines = new HashSet<Distiplines>();
            this.Evaluation = new HashSet<Evaluation>();
            this.Group1 = new HashSet<Group>();
        }
    
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Familia { get; set; }
        public int Rang { get; set; }
        public Nullable<int> Group { get; set; }
        public string Photo { get; set; }
    
        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<Distiplines> Distiplines { get; set; }
        public virtual ICollection<Evaluation> Evaluation { get; set; }
        public virtual ICollection<Group> Group1 { get; set; }
        public virtual Group Group2 { get; set; }

        public override string ToString()
        {
            return Name + " " + Familia;
        }
    }
}
