﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uruntakip.db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class uruntakipdbEntities3 : DbContext
    {
        public uruntakipdbEntities3()
            : base("name=uruntakipdbEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCustomer> tblCustomer { get; set; }
        public virtual DbSet<tbllogin> tbllogin { get; set; }
        public virtual DbSet<tblmakinatipi> tblmakinatipi { get; set; }
        public virtual DbSet<tblsevk> tblsevk { get; set; }
        public virtual DbSet<tblsevkdetay> tblsevkdetay { get; set; }
        public virtual DbSet<tblteklif> tblteklif { get; set; }
        public virtual DbSet<tblteklifdetay> tblteklifdetay { get; set; }
        public virtual DbSet<tblurunler> tblurunler { get; set; }
        public virtual DbSet<tblmakina> tblmakina { get; set; }
        public virtual DbSet<tblarızasonuctipleri> tblarızasonuctipleri { get; set; }
        public virtual DbSet<tblarizaislemleri> tblarizaislemleri { get; set; }
        public virtual DbSet<tblarizakategorileri> tblarizakategorileri { get; set; }
        public virtual DbSet<tblarizalar> tblarizalar { get; set; }
        public virtual DbSet<tblkategori> tblkategori { get; set; }
    }
}
