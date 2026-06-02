using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscGolfBag.Api.Migrations
{
    [DbContext(typeof(DiscGolfBag.Api.Common.Data.AppDbContext))]
    [Migration("20260601130000_AddDiscDescription")]
    public partial class AddDiscDescription : Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
        }
    }
}
